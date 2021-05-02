using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.DTO.Rating;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Abstractions
{
    public interface IRatingActionController
    {
        Task<IEnumerable<RatingAction>> GetAllAsync();
        Task<IActionResult> GetAllByAdIdAsync(Guid adId);
        Task<IActionResult> AddByIdAsync([FromBody] RatingActionDto ratingActionDto);
        Task<IActionResult> RemoveByIdAsync(Guid adId);
    }
}