using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
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
        [HttpGet("all", Name = "ManagerGetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyusername/{name}", Name = "ManagerGetAdByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyadid/{id}", Name = "ManagerGetAd")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            Ad ad = await _adService.GetAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return new ObjectResult(ad);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync([FromBody] Ad ad)
        {
            if (ad == null)
            {
                return BadRequest();
            }

            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return Unauthorized();
            }
            ad.UserName = userName;
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("ManagerGetAd", new { id = ad.Id }, ad);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Ad updatedAd)
        {
            if (updatedAd == null)
            {
                return NotFound("Ad for update not found");
            }
            if (updatedAd == null || updatedAd.Id != id)
            {
                return BadRequest();
            }
            await _adService.UpdateAsync(updatedAd);
            return CreatedAtRoute("ManagerGetAd", new { id = updatedAd.Id }, updatedAd);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var adForDelete = await _adService.GetAsync(id);
            if (adForDelete == null)
            {
                return NotFound("Ad for delete not found");
            }
            var deletedAd = await _adService.DeleteAsync(adForDelete);

            return new ObjectResult(deletedAd);
        }
    }
}
