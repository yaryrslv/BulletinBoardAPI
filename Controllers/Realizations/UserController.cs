using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:SecretKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWTToken:Issuer"],
                    audience: _configuration["JWTToken:Audience"],
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
            if (await _userManager.Users.CountAsync() > 0)
            {
                var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response {Status = "Error", Message = "User already exists!"});

                User user = new User()
                {
                    Email = userRegisterDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = userRegisterDto.Username
                };
                var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response {Status = "Error", Message = result.ToString()});
                user.EmailConfirmed = true;
                return CreatedAtRoute("GetUser", new {id = user.Id}, user);
            }
            else
            {
                return BadRequest("User registeradmin for create administrator account");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] UserRegisterDto userRegisterDto)
        {
            if (await _userManager.Users.CountAsync() == 0)
            {
                var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                User user = new User()
                {
                    Email = userRegisterDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = userRegisterDto.Username
                };
                var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.ToString() });

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                user.EmailConfirmed = true;
                return CreatedAtRoute("GetUser", new { id = user.Id }, user);
            }
            return BadRequest("Administrator has already been created");
        }
        [Authorize]
        [HttpGet("all", Name = "GetAllUsers")]
        public async Task<IEnumerable<UserGetDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserGetDto>>(users);
        }
        [Authorize]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetAsync(string id)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpGet("{username}", Name = "GetUserByName")]
        public async Task<IActionResult> GetByNameAsync(string userName)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == userName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userPutDto)
        {
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == String.Empty)
            {
                return NotFound("Token not found");
            }

            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("User is null");
            }
            if (!IsValidEmail(userPutDto.Email))
            {
                return NotFound("Invalid Email");
            }
            var response = await _userManager.SetEmailAsync(user, userPutDto.Email);
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == String.Empty)
            {
                return NotFound("Token not found");
            }

            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = await _userManager.DeleteAsync(user);
            return new ObjectResult(response);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
