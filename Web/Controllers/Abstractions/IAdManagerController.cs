using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.Ad;

namespace Web.Controllers.Abstractions
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