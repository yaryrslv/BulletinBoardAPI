using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IUserManagerController
    {
        Task<IEnumerable<User>> GetAll();
        Task<IActionResult> GetFull(string id);
        Task<IActionResult> UpdateRole(string id, [FromBody] UserManagerUpdateRoleDto updated);
        Task<IActionResult> UpdateEMail(string id, [FromBody] User updatedUser);
        Task<IActionResult> Delete(string id);
    }
}