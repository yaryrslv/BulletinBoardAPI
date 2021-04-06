using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IUserController
    {
        Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto);
        Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto);
        Task<IEnumerable<User>> GetAll();
        Task<IActionResult> Get(string id);
        Task<IActionResult> Delete(string id);
    }
}