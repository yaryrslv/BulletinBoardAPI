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
        Task<IEnumerable<Ad>> GetAll();
        Task<IEnumerable<Ad>> GetByName(string name);
        Task<IActionResult> Get(Guid id);
        Task<IActionResult> Create([FromBody] AdDto adDto);
        Task<IActionResult> Update(Guid id, [FromBody] AdDto updatedAdDto);
        Task<IActionResult> Delete(Guid id);
    }
}
