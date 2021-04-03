using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase, IAdController
    {
        private readonly IAdService _adService;
        public AdController(IAdService adService)
        {
            _adService = adService;
        }
        [HttpGet(Name = "GetAllAds")]
        public async Task<IEnumerable<Ad>> GetAll()
        {
            return await _adService.GetAllAsync();
        }
        [HttpGet("{id}", Name = "GetAd")]
        public async Task<IActionResult> Get(Guid id)
        {
            Ad ad = await _adService.GetAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return new ObjectResult(ad);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ad ad)
        {
            if (ad == null)
            {
                return BadRequest();
            }
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("GetAd", new { id = ad.Id }, ad);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Ad updatedAd)
        {
            if (updatedAd == null || updatedAd.Id != id)
            {
                return BadRequest();
            }

            var ad = await _adService.GetAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            await _adService.UpdateAsync(updatedAd);
            return RedirectToRoute("GetAll");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedAd = await _adService.DeleteAsync(id);

            if (deletedAd == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedAd);
        }
    }
}
