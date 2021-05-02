using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Abstractions
{
    public interface IAdController
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IEnumerable<Ad>> GetAllActualAsync();
        Task<IActionResult> GetByIdAsync(Guid id);
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IActionResult> GetByCityAsync(string city);
        Task<IActionResult> CreateAsync([FromBody] AdDto adDto);
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}