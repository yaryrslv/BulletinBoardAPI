using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    interface IAdController
    {
        Task<IEnumerable<Ad>> GetAll();
        Task<IActionResult> Get();
        Task<IActionResult> Create();
        Task<IActionResult> Update();
        Task<IActionResult> Delete();
    }
}
