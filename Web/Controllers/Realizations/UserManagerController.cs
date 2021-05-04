using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Abstractions;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.DTO.UserManager;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        /// <summary>
        /// [AdminRightsRequrered] Get all identity Users in extended format.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getall", Name = "ManagerGetAll")]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        /// <summary>
        /// [AdminRightsRequrered] Get identity User by Id in extended format.
        /// </summary>
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
        /// <summary>
        /// [AdminRightsRequrered] Get identity User by UserName in extended format.
        /// </summary>
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
        /// <summary>
        /// [AdminRightsRequrered] Get role of any identity User by Id.
        /// </summary>
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
        /// <summary>
        /// [AdminRightsRequrered] Update Email of any identity User by Id.
        /// </summary>
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
        /// [AdminRightsRequrered] Update PhoneNumber of any identity User by Id.
        /// </summary>
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
        /// <summary>
        /// [AdminRightsRequrered] Update role of any identity User by Id.
        /// </summary>
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
        /// [AdminRightsRequrered] Delete any identity User by Id.
        /// </summary>
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
