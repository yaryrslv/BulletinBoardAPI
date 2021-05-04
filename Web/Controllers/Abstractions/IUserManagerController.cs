using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO.User;
using BulletinBoardAPI.DTO.UserManager;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Abstractions
{
    public interface IUserManagerController
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IActionResult> GetFullAsync(string id);
        Task<IActionResult> GetByNameAsync(string userName);
        Task<IActionResult> GetRolesAsync(string id);
        Task<IActionResult> UpdateEmailAsync(string id, [FromBody] UserUpdateEmailDto userUpdateEMailDto);
        Task<IActionResult> UpdatePhoneNumberAsync(string id, [FromBody] UserUpdatePhoneNumberDto updatePhoneNumberDto);
        Task<IActionResult> UpdateRoleAsync(string id, [FromBody] UserManagerUpdateRoleDto updated);
        Task<IActionResult> DeleteAsync(string id);
    }
}