using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.Ad;

namespace Web.Controllers.Realizations
{
    public interface IAdController
    {
        /// <summary>
        /// [AuthorizeRequired] Get all existing Ads.
        /// </summary>
        Task<IEnumerable<AdFullDto>> GetAllAsync();

        /// <summary>
        /// [AuthorizeRequired] Show only Ads whose ExpirationDite has not yet arrived.
        /// </summary>
        Task<IEnumerable<AdFullDto>> GetAllActualAsync();

        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its Id.
        /// </summary>
        Task<IActionResult> GetByIdAsync(Guid id);

        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its UserName.
        /// </summary>
        Task<IEnumerable<AdFullDto>> GetByNameAsync(string name);

        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its City.
        /// </summary>
        Task<IActionResult> GetByCityAsync(string city);

        /// <summary>
        /// [AuthorizeRequired] Creates a new Ad.
        /// </summary>
        Task<IActionResult> CreateAsync([FromBody] AdDto adDto);

        /// <summary>
        /// [AuthorizeRequired] Updates the Ad of the current current Authorized identity User by its Id.
        /// </summary>
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto);

        /// <summary>
        /// [AuthorizeRequired] Deletes the Ad of the current Authorized identity User by its Id.
        /// </summary>
        Task<IActionResult> DeleteAsync(Guid id);
    }
}