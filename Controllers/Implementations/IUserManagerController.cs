﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IUserManagerController
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IActionResult> GetFullAsync(string id);
        Task<IActionResult> UpdateRoleAsync(string id, [FromBody] UserManagerUpdateRoleDto updated);
        Task<IActionResult> UpdateEMailAsync(string id, [FromBody] User updatedUser);
        Task<IActionResult> DeleteAsync(string id);
    }
}