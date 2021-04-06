using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    public interface IAdManagerController
    {
        Task<IEnumerable<Ad>> GetAll();
        Task<IEnumerable<Ad>> GetByName(string name);
        Task<IActionResult> Get(Guid id);
        Task<IActionResult> Create([FromBody] Ad ad);
        Task<IActionResult> Update(Guid id, [FromBody] Ad updatedAd);
        Task<IActionResult> Delete(Guid id);
    }
}