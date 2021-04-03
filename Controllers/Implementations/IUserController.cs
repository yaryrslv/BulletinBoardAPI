using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    interface IUserController
    { 
            Task<IEnumerable<User>> GetAll();
            Task<IActionResult> Get(Guid id);
            Task<IActionResult> Create(User ad);
            Task<IActionResult> Update(Guid id, User updatedUser);
            Task<IActionResult> Delete(Guid id);
    }
}
