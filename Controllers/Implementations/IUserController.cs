using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IUserController
    {
        Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto);
        Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto userRegisterDto);
        Task<IActionResult> RegisterAdminAsync([FromBody] UserRegisterDto userRegisterDto);
        Task<IEnumerable<UserGetDto>> GetAllAsync();
        Task<IActionResult> GetAsync(string id);
        Task<IActionResult> GetByNameAsync(string userName);
        Task<IActionResult> UpdateEmailAsync([FromBody] UserUpdateEmailDto userPutDto);
        Task<IActionResult> DeleteAsync();
    }
}