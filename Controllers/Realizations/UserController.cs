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
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllAsync();
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id)
        {
            User user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            if (await _userService.IsUserNameExistsAsync(userDto.Name))
            {
                return Conflict();
            } 
            var user = _mapper.Map<User>(userDto);
            await _userService.CreateAsync(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto updatedUserDto)
        {
            if (updatedUserDto == null)
            {
                return BadRequest();
            }
            var user = await _userService.GetAsync(id);
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }
            var updatedUser = _mapper.Map<User>(updatedUserDto);
            await _userService.UpdateAsync(user, updatedUser);
            return RedirectToRoute("GetAllUsers");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedUser = await _userService.DeleteAsync(id);

            if (deletedUser == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedUser);
        }
        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync([FromBody] UserDto userDto)
        {
            if (userDto == null) return Unauthorized();
            string tokenString = string.Empty;
            bool validUser = await AuthenticateAsync(userDto);
            if (validUser)
            {
                tokenString = BuildToken();
            }
            else
            {
                return Unauthorized();
            }
            return Ok(new { Token = tokenString });
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

        private async Task<bool> AuthenticateAsync(UserDto userDto)
        {
            bool validUser = false;

            var user = await _userService.GetUserByName(userDto.Name);
            if (user != null)
            {
                validUser = true;
            }
            return validUser;
        }
    }
}
