using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IAdManagerController
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IActionResult> GetAsync(Guid id);
        Task<IActionResult> CreateAsync([FromBody] Ad ad);
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] Ad updatedAd);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}