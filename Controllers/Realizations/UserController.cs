using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserController(IMapper mapper, IConfiguration configuration,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:SecretKey"]));
                var token = new JwtSecurityToken(
                    _configuration["JWTToken:Issuer"],
                    _configuration["JWTToken:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto userRegisterDto)
        {
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username); 
            if (userExists != null) 
            { 
                return Conflict("User already exists!");
            } 
            var emailExists = await _userManager.FindByEmailAsync(userRegisterDto.Email); 
            if (emailExists != null) 
            { 
                return Conflict("Email already exists!");
            } 
            User user = new User 
            {
                SecurityStamp = Guid.NewGuid().ToString()
            }; 
            user = _mapper.Map(userRegisterDto, user); 
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password); 
            if (!result.Succeeded) 
            { 
                return BadRequest(result.ToString());
            }
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdminAsync(string adminRegistrationKey, [FromBody] UserRegisterDto userRegisterDto)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(adminRegistrationKey));
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            if (_configuration["AdminRegistrationKeyMD5Hash"].ToLower() != hashString.ToLower())
            {
                return BadRequest("Wrong admin registration key");
            }
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username); 
            if (userExists != null) 
            { 
                return Conflict("User already exists!");
            } 
            var emailExists = await _userManager.FindByEmailAsync(userRegisterDto.Email); 
            if (emailExists != null) 
            { 
                return Conflict("Email already exists!");
            } 
            User user = new User 
            {
                SecurityStamp = Guid.NewGuid().ToString()
            }; 
            user = _mapper.Map(userRegisterDto, user); 
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password); 
            if (!result.Succeeded) 
            { 
                return BadRequest(result.ToString());
            } 
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin)) 
            {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            
            if (!await _roleManager.RoleExistsAsync(UserRoles.User)) 
            {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            } 
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin)) 
            { 
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }

        [Authorize]
        [HttpGet("getall", Name = "GetAllUsers")]
        public async Task<IEnumerable<UserGetDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserGetDto>>(users);
        }

        [Authorize]
        [HttpGet("getbyid/{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }

        [Authorize]
        [HttpGet("getbyusername/{userName}", Name = "GetUserByName")]
        public async Task<IActionResult> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName.ToLower());
            if (user == null) return NotFound("User not found");
            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }

        [Authorize]
        [HttpPut("updateemail")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UserUpdateEmailDto userUpdateEMailDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(userUpdateEMailDto.Email);
            if (emailExists != null)
            {
                return Conflict("Email already exists!");
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("User is null");
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, userUpdateEMailDto.Email);
            var response = await _userManager.ChangeEmailAsync(user, userUpdateEMailDto.Email, token);
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpPut("updatphonenumber")]
        public async Task<IActionResult> UpdatePhoneNumberAsync([FromBody] UserUpdatePhoneNumberDto updatePhoneNumberDto)
        {
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("Current user not found");
            }
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, updatePhoneNumberDto.PhoneNumber);
            var response = await _userManager.ChangePhoneNumberAsync(user, updatePhoneNumberDto.PhoneNumber, token);
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UserUpdatePasswordDto userUpdatePasswordDto)
        {
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == string.Empty)
            {
                return NotFound("Token not found");
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("User is null");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var response = await _userManager.ResetPasswordAsync(user,
                token, userUpdatePasswordDto.NewPassword);
            return new ObjectResult(response);
        }

        [Authorize]
        [HttpDelete("deletcurrenteuser")]
        public async Task<IActionResult> DeleteAsync()
        {
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == string.Empty)
            {
                return NotFound("Token not found");
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = await _userManager.DeleteAsync(user);
            await HttpContext.SignOutAsync();
            return new ObjectResult(response);
        }
    }
}