using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.Ad;

namespace Web.Controllers.Realizations
{
    public interface IAdManagerController
    {
        /// <summary>
        /// [AdminRightsRequrered] Get all existing Ads in extended format.
        /// </summary>
        Task<IEnumerable<AdFullDto>> GetAllAsync();

        /// <summary>
        /// [AdminRightsRequrered] Get Ad of any User by Id in extended format.
        /// </summary>
        Task<IActionResult> GetByIdAsync(Guid id);

        /// <summary>
        /// [AdminRightsRequrered] Get Ad of any User by Id in extended format.
        /// </summary>
        Task<IEnumerable<AdFullDto>> GetByNameAsync(string name);

        /// <summary>
        /// [AdminRightsRequrered] Updates extended Ad fields of any User by UserName.
        /// </summary>
        Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto);

        /// <summary>
        /// [AdminRightsRequrered] Deletes any Ad of any User by Id.
        /// </summary>
        Task<IActionResult> DeleteAsync(Guid id);
    }
}