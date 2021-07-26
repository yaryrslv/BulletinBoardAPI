using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.RatinAction;

namespace Web.Controllers.Abstractions
{
    public interface IRatingActionController
    {
        /// <summary>
        /// [AuthorizeRequired] Receives all RatingActions for the entire time.
        /// </summary>
        Task<IEnumerable<RatingActionFullDto>> GetAllAsync();

        /// <summary>
        /// [AuthorizeRequired] Receives all Ad RatingActions by Ad Id.
        /// </summary>
        Task<IActionResult> GetAllByAdIdAsync(Guid adId);

        /// <summary>
        /// [AuthorizeRequired] Creates new RatingAction.
        /// </summary>
        Task<IActionResult> AddByIdAsync([FromBody] RatingActionDto ratingActionDto);

        /// <summary>
        /// [AuthorizeRequired] Removes existing RatingAction.
        /// </summary>
        Task<IActionResult> RemoveByIdAsync(Guid adId);
    }
}