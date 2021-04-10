﻿using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
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
                return NotFound("User not found");
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
                return NotFound("User not found");
            }
            return new ObjectResult(user);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updateemail")]
        public async Task<IActionResult> UpdateEmailAsync(string id, [FromBody] UserUpdateEmailDto userUpdateEMailDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(userUpdateEMailDto.Email);
            if (emailExists != null)
            {
                return Conflict("Email already exists!");
            }
            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return NotFound("Current user not found");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("User is null");
            }
            if (!IsValidEmail(userUpdateEMailDto.Email))
            {
                return NotFound("Invalid Email");
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, userUpdateEMailDto.Email);
            var response = await _userManager.ChangeEmailAsync(user, userUpdateEMailDto.Email, token);
            return new ObjectResult(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updaterolebyid")]
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
        [HttpDelete("deletebyid")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = await _userManager.DeleteAsync(user);
            await HttpContext.SignOutAsync();
            return new ObjectResult(response);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
