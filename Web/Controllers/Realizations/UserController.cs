using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Web.Controllers.Abstractions;
using Web.DTO.User;
using Web.FluentValidator;

namespace Web.Controllers.Realizations
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
        /// <summary>
        /// [AllowAnonymous] Creates new identity User authorization JWT token in Bearer format, by UserName and Password.
        /// </summary>
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
            return Unauthorized(new Response()
            {
                Status = "Unauthorized",
                Message = "Login, or password, is incorrect, the user is not authorized"
            });
        }
        /// <summary>
        /// [AllowAnonymous] Create new identity User with User role.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto userRegisterDto)
        {
            var validator = new UserRegisterDtoValidator();
            var result = validator.Validate(userRegisterDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username); 
            if (userExists != null) 
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "User already exists"
                });
            }
            var emailExists = await _userManager.FindByEmailAsync(userRegisterDto.Email); 
            if (emailExists != null) 
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "Email already exists"
                });
            } 
            User user = new User 
            {
                SecurityStamp = Guid.NewGuid().ToString()
            }; 
            user = _mapper.Map(userRegisterDto, user); 
            var managerResult = await _userManager.CreateAsync(user, userRegisterDto.Password); 
            if (!managerResult.Succeeded) 
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = result.ToString()
                });
            }
            await _userManager.AddToRoleAsync(user, UserRoles.User);
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }
        /// <summary>
        /// [AllowAnonymous] Create new identity User with Admin role, AdminRegistrationKey required.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdminAsync(string adminRegistrationKey, [FromBody] UserRegisterDto userRegisterDto)
        {
            var validator = new UserRegisterDtoValidator();
            var result = validator.Validate(userRegisterDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(adminRegistrationKey));
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            if (_configuration["AdminRegistrationKeyMD5Hash"].ToLower() != hashString.ToLower())
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong admin registration key"
                });
            }
            var userExists = await _userManager.FindByNameAsync(userRegisterDto.Username); 
            if (userExists != null) 
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "User already exists"
                });
            } 
            var emailExists = await _userManager.FindByEmailAsync(userRegisterDto.Email); 
            if (emailExists != null) 
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "Email already exists"
                });
            } 
            User user = new User 
            {
                SecurityStamp = Guid.NewGuid().ToString()
            }; 
            user = _mapper.Map(userRegisterDto, user); 
            var managerResult = await _userManager.CreateAsync(user, userRegisterDto.Password); 
            if (!managerResult.Succeeded) 
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = result.ToString()
                });
            }
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }
        /// <summary>
        /// [AuthorizeRequired] Get all Users.
        /// </summary>
        [Authorize]
        [HttpGet("getall", Name = "GetAllUsers")]
        public async Task<IEnumerable<UserGetDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserGetDto>>(users);
        }
        /// <summary>
        /// [AuthorizeRequired] Get all Users by Id.
        /// </summary>
        [Authorize]
        [HttpGet("getbyid/{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "User not found"
                });
            }
            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Get all Users by UserName.
        /// </summary>
        [Authorize]
        [HttpGet("getbyusername/{userName}", Name = "GetUserByName")]
        public async Task<IActionResult> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName.ToLower());
            if (user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "User not found"
                });
            }
            var response = _mapper.Map<UserGetDto>(user);
            return new ObjectResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Updates Email of current Authorized identity User.
        /// </summary>
        [Authorize]
        [HttpPut("updateemail")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UserUpdateEmailDto userUpdateEMailDto)
        {
            var validator = new UserUpdateEmailDtoValidator();
            var result = validator.Validate(userUpdateEMailDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var emailExists = await _userManager.FindByEmailAsync(userUpdateEMailDto.Email);
            if (emailExists != null)
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "Email already exists!"
                });
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Current user not found"
                });
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "User is null"
                });
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, userUpdateEMailDto.Email);
            var response = await _userManager.ChangeEmailAsync(user, userUpdateEMailDto.Email, token);
            if (response.Succeeded == false)
            {
                return BadRequest(new Response()
                {
                    Status = "Error",
                    Message = JsonConvert.SerializeObject(response.Errors)
                });
            }
            return new ObjectResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Updates PhoneNumber of current Authorized identity identity User.
        /// </summary>
        [Authorize]
        [HttpPut("updatphonenumber")]
        public async Task<IActionResult> UpdatePhoneNumberAsync([FromBody] UserUpdatePhoneNumberDto updatePhoneNumberDto)
        {
            var validator = new UserUpdatePhoneNumberDtoValidator();
            var result = validator.Validate(updatePhoneNumberDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var userName = HttpContext.User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (userName == null || user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Current user not found"
                });
            }
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, updatePhoneNumberDto.PhoneNumber);
            var response = await _userManager.ChangePhoneNumberAsync(user, updatePhoneNumberDto.PhoneNumber, token);
            if (response.Succeeded == false)
            {
                return BadRequest(new Response()
                {
                    Status = "Error",
                    Message = JsonConvert.SerializeObject(response.Errors)
                });
            }
            return new ObjectResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Updates Password of current Authorized identity identity User.
        /// </summary>
        [Authorize]
        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UserUpdatePasswordDto userUpdatePasswordDto)
        {
            var validator = new UserUpdatePasswordDtoValidator();
            var result = validator.Validate(userUpdatePasswordDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == string.Empty)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Token not found"
                });
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Current user not found"
                });
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "User mot found"
                });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var response = await _userManager.ResetPasswordAsync(user,
                token, userUpdatePasswordDto.NewPassword);
            if (response.Succeeded == false)
            {
                return BadRequest(new Response()
                {
                    Status = "Error",
                    Message = JsonConvert.SerializeObject(response.Errors)
                });
            }
            return new ObjectResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Deletes current Authorized identity identity User.
        /// </summary>
        [Authorize]
        [HttpDelete("deletcurrenteuser")]
        public async Task<IActionResult> DeleteAsync()
        {
            var userToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (userToken == string.Empty)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Token not found"
                });
            }
            var userName = HttpContext.User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (userName == null || user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Current user not found"
                });
            }
            var response = await _userManager.DeleteAsync(user);
            if (response.Succeeded == false)
            {
                return BadRequest(new Response()
                {
                    Status = "Error",
                    Message = JsonConvert.SerializeObject(response.Errors)
                });
            }
            await HttpContext.SignOutAsync();
            return new ObjectResult(response);
        }
    }
}