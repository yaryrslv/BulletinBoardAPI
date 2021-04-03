using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
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
        UserController(IUserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            await _userService.CreateAsync(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User updatedUser)
        {
            if (updatedUser == null || updatedUser.Id != id)
            {
                return BadRequest();
            }

            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.UpdateAsync(updatedUser);
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
