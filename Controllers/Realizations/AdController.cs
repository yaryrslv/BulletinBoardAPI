using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using ObjectResult = Microsoft.AspNetCore.Mvc.ObjectResult;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase, IAdController
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;
        public AdController(IAdService adService, IMapper mapper)
        {
            _adService = adService;
            _mapper = mapper;
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
        public async Task<IActionResult> Create([FromBody] AdDto adDto)
        {
            if (adDto == null)
            {
                return BadRequest();
            }

            var ad = _mapper.Map<Ad>(adDto);
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("GetAd", new { id = ad.Id, user = ad.User}, ad);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AdDto updatedAdDto)
        {
            if (updatedAdDto == null)
            {
                return BadRequest();
            }
            var ad = await _adService.GetAsync(id);
            if (ad == null || ad.Id != id)
            {
                return BadRequest();
            }

            var updatedAd = _mapper.Map<Ad>(updatedAdDto);
            await _adService.UpdateAsync(ad, updatedAd);
            return RedirectToRoute("GetAllAds");
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
