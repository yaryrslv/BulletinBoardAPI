using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.User;
using Web.DTO.UserManager;

namespace Web.Controllers.Abstractions
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