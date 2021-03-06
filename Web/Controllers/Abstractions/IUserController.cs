using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.User;

namespace Web.Controllers.Abstractions
{
    public interface IUserController
    {
        Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto);
        Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto userRegisterDto);
        Task<IActionResult> RegisterAdminAsync(string adminRegistrationKey, [FromBody] UserRegisterDto userRegisterDto);
        Task<IEnumerable<UserGetDto>> GetAllAsync();
        Task<IActionResult> GetAsync(string id);
        Task<IActionResult> GetByUserNameAsync(string userName);
        Task<IActionResult> UpdateEmailAsync([FromBody] UserUpdateEmailDto userPutDto);
        Task<IActionResult> UpdatePhoneNumberAsync([FromBody] UserUpdatePhoneNumberDto updatePhoneNumberDto);
        Task<IActionResult> DeleteAsync();
    }
}