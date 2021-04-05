using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IMapper mapper, IConfiguration config)
        {
            _userService = userService;
            _mapper = mapper;
            _config = config;
        }
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Data is null");
            }

            if (await _userService.IsUserNameExistsAsync(userDto.Name))
            {
                return Conflict("User already exists");
            }
            var user = _mapper.Map<User>(userDto);
            await _userService.RegisterAsync(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync([FromBody] RegisterUserDto userDto)
        {
            if (userDto == null) return Unauthorized();
            string tokenString;
            bool validUser = await AuthenticateAsync(userDto);
            if (validUser)
            {
                tokenString = BuildToken();
            }
            else
            {
                return Unauthorized("Wrong data");
            }
            return Ok(new { Token = tokenString });
        }
        [Authorize]
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllAsync();
        }
        [Authorize]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id)
        {
            User user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return new ObjectResult(user);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto updatedUserDto)
        {
            //if (updatedUserDto.Role != UserRoles.User && updatedUserDto.Role != UserRoles.Admin)
            //{
            //    return BadRequest("Unknown user role");
            //}
            var user = await _userService.GetAsync(id);
            if (user == null || user.Id != id)
            {
                return BadRequest("Wrong data");
            }
            if (user.Name != updatedUserDto.Name)
            {
                if (_userService.GetUserByName(updatedUserDto.Name) != null)
                {
                    return BadRequest("This name already exits");
                }
            }
            
            var updatedUser = _mapper.Map<User>(updatedUserDto);
            updatedUser = await _userService.UpdateAsync(user, updatedUser);
            return CreatedAtRoute("GetUser", new { id = updatedUser.Id }, updatedUser);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedUser = await _userService.DeleteAsync(id);

            if (deletedUser == null)
            {
                return BadRequest("Data is null");
            }

            return new ObjectResult(deletedUser);
        }
        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JwtToken:Issuer"],
                _config["JwtToken:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<bool> AuthenticateAsync(RegisterUserDto userDto)
        {
            bool validUser = false;

            var user = await _userService.GetUserByName(userDto.Name);
            if (user != null && user.Password == _userService.GetHash(userDto.Password))
            {
                validUser = true;
            }
            return validUser;
        }
    }
}
