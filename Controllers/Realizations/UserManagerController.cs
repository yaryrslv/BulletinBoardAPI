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
        [HttpGet(Name = "ManagerGetAll")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userManager.Users.ToListAsync();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("{id}", Name = "ManagerGetUser")]
        public async Task<IActionResult> GetFull(string id)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return new ObjectResult(user);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UserManagerUpdateRoleDto updated)
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
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateEMail(string id, [FromBody] User updatedUser)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            if (currentUser == null)
            {
                return NotFound("Current user not found");
            }
            var response = await _userManager.UpdateAsync(currentUser);
            return new ObjectResult(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
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
