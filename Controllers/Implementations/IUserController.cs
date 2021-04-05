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
            Task<IEnumerable<User>> GetAll();
            Task<IActionResult> Get(Guid id);
            Task<IActionResult> Create(CreateUserDto createUserDto);
            Task<IActionResult> Update(Guid id, UserDto updatedUserDto);
            Task<IActionResult> Delete(Guid id);
            Task<IActionResult> CreateTokenAsync([FromBody] UserDto userDto);
    }
}
