using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase, IUserManagerController
    {
        private readonly UserManager<User> _userManager;

        public UserManagerController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("all", Name = "ManagerGetAll")]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyid/{id}", Name = "ManagerGetUser")]
        public async Task<IActionResult> GetFullAsync(string id)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return new ObjectResult(user);
        }
        [Authorize]
        [HttpGet("getbyusername/{username}", Name = "ManagerGetUserByName")]
        public async Task<IActionResult> GetByNameAsync(string userName)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == userName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return new ObjectResult(user);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("new")]
        public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] UserManagerUpdateRoleDto updated)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (updated.Role != UserRoles.User || updated.Role != UserRoles.Admin)
            {
                return NotFound("Role not found");
            }
            var response = await _userManager.AddToRoleAsync(user, updated.Role);
            return new ObjectResult(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateEMailAsync(string id, [FromBody] User updatedUser)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            if (currentUser == null)
            {
                return NotFound("Current user not found");
            }
            var response = await _userManager.UpdateAsync(currentUser);
            return new ObjectResult(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = _userManager.DeleteAsync(user);
            return new ObjectResult(response);
        }
    }
}
