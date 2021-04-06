using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IUserController
    {
        Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto);
        Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto);
        Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDto userRegisterDto);
        Task<IEnumerable<UserGetDto>> GetAll();
        Task<IActionResult> Get(string id);
        Task<IActionResult> GetByName(string userName);
        Task<IActionResult> Update([FromBody] UserUpdateDto userPutDto);
        Task<IActionResult> Delete();
    }
}