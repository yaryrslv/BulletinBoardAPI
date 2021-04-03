using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Implementations;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IAdController
    {
        Task<IEnumerable<Ad>> GetAll();
        Task<IActionResult> Get(Guid id);
        Task<IActionResult> Create(Ad ad);
        Task<IActionResult> Update(Guid id, Ad updatedAd);
        Task<IActionResult> Delete(Guid id);
    }
}
