using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdManagerController : ControllerBase, IAdManagerController
    {
        private readonly IAdService _adService;

        public AdManagerController(IAdService adService)
        {
            _adService = adService;
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getall", Name = "ManagerGetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyadid/{id}", Name = "ManagerGetAd")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Ad ad = await _adService.GetByIdAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return new ObjectResult(ad);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyusername/{name}", Name = "ManagerGetAdByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updatebyid/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto)
        {
            if (updatedAdDto == null)
            {
                return NotFound("Ad for update not found");
            }

            var updatedAd = await _adService.GetByIdAsync(id);
            if (updatedAd == null || updatedAd.Id != id)
            {
                return BadRequest();
            }
            await _adService.UpdateAsync(updatedAd);
            return CreatedAtRoute("ManagerGetAd", new { id = updatedAd.Id }, updatedAd);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var adForDelete = await _adService.GetByIdAsync(id);
            if (adForDelete == null)
            {
                return NotFound("Ad for delete not found");
            }
            var deletedAd = await _adService.DeleteAsync(adForDelete);

            return new ObjectResult(deletedAd);
        }
    }
}
