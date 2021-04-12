using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authentication;
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
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "User not found"
                });
            }
            return new ObjectResult(user);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyusername/{username}", Name = "ManagerGetUserByName")]
        public async Task<IActionResult> GetByNameAsync(string userName)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == userName);
            if (user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "User not found"
                });
            }
            return new ObjectResult(user);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getuserrolesbyid/{id}", Name = "ManagerGetUserRolesById")]
        public async Task<IActionResult> GetRolesAsync(string id)
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

            var roles = await _userManager.GetRolesAsync(user);
            return new ObjectResult(roles);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updateemailbyid/{id}")]
        public async Task<IActionResult> UpdateEmailAsync(string id, [FromBody] UserUpdateEmailDto userUpdateEMailDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(userUpdateEMailDto.Email);
            if (emailExists != null)
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "Email already exists"
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

            var user = await _userManager.FindByIdAsync(id);
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
            return new ObjectResult(response);
        }
        [Authorize]
        [HttpPut("updatphonenumberbyid/{id}")]
        public async Task<IActionResult> UpdatePhoneNumberAsync(string id, [FromBody] UserUpdatePhoneNumberDto updatePhoneNumberDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Current user not found"
                });
            }
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, updatePhoneNumberDto.PhoneNumber);
            var response = await _userManager.ChangePhoneNumberAsync(user, updatePhoneNumberDto.PhoneNumber, token);
            return new ObjectResult(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updaterolebyid/{id}")]
        public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] UserManagerUpdateRoleDto updated)
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

            if (updated.Role != UserRoles.User || updated.Role != UserRoles.Admin)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Role not found"
                });
            }
            var response = await _userManager.AddToRoleAsync(user, updated.Role);
            return new ObjectResult(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
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
            var response = await _userManager.DeleteAsync(user);
            await HttpContext.SignOutAsync();
            return new ObjectResult(response);
        }
    }
}
