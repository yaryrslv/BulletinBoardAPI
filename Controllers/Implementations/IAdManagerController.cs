using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IAdManagerController
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IActionResult> GetByIdAsync(Guid id);
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}