using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.Rating;

namespace Web.Controllers.Abstractions
{
    public interface IRatingActionController
    {
        Task<IEnumerable<RatingAction>> GetAllAsync();
        Task<IActionResult> GetAllByAdIdAsync(Guid adId);
        Task<IActionResult> AddByIdAsync([FromBody] RatingActionDto ratingActionDto);
        Task<IActionResult> RemoveByIdAsync(Guid adId);
    }
}