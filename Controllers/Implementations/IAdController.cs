using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Implementations
{
    public interface IAdController
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IActionResult> GetAsync(Guid id);
        Task<IActionResult> CreateAsync([FromBody] AdDto adDto);
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto);
        Task<IActionResult> DeleteAsync(Guid id);
    }
}
