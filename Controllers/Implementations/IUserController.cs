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
        Task<IActionResult> CreateTokenAsync([FromBody] RegisterUserDto userDto);
        Task<IActionResult> Register(RegisterUserDto createUserDto);
        Task<IEnumerable<User>> GetAll();
        Task<IActionResult> Get(Guid id);
        Task<IActionResult> Update(Guid id, UserDto updatedUserDto);
        Task<IActionResult> Delete(Guid id);
    }
}