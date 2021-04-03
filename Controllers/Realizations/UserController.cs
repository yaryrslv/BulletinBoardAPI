using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
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
    }
}
