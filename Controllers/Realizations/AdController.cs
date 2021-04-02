using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        IAdService AdService;
        public AdController(IAdService adService)
        {
            AdService = adService;
        }
        [HttpGet(Name = "GetAllAds")]
        public async Task<IEnumerable<Ad>> GetAll()
        {
            return await AdService.GetAllAsync();
        }
        [HttpGet("{id}", Name = "GetAd")]
        public async Task<IActionResult> Get(Guid id)
        {
            Ad ad = await AdService.GetAsync(id);
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
            await AdService.CreateAsync(ad);
            return CreatedAtRoute("GetAd", new { id = ad.Id }, ad);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Ad updatedAd)
        {
            if (updatedAd == null || updatedAd.Id != id)
            {
                return BadRequest();
            }

            var ad = await AdService.GetAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            await AdService.UpdateAsync(updatedAd);
            return RedirectToRoute("GetAll");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedAd = await AdService.DeleteAsync(id);

            if (deletedAd == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedAd);
        }
    }
}
