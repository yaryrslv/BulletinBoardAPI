using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("all", Name = "GetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        [Authorize]
        [HttpGet("allactual", Name = "GetAllActualAds")]
        public async Task<IEnumerable<Ad>> GetAllActualAsync()
        {
            return await _adService.GetAllActualAsync();
        }
        [Authorize]
        [HttpGet("getbyusername/{name}", Name = "GetAdsByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        [Authorize]
        [HttpGet("getactualbyusername/{name}", Name = "GetActualAdsByName")]
        public async Task<IEnumerable<Ad>> GetActualByNameAsync(string name)
        {
            return await _adService.GetActualByNameAsync(name);
        }
        [Authorize]
        [HttpGet("getbyadid/{id}", Name = "GetAd")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var ad = await _adService.GetAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return new ObjectResult(ad);
        }
        [Authorize]
        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync([FromBody] AdDto adDto)
        {
            if (adDto == null)
            {
                return BadRequest();
            }

            var userName = HttpContext.User.Identity?.Name;
            if (userName == null)
            {
                return Unauthorized();
            }
            var ad = _mapper.Map<Ad>(adDto);
            ad.UserName = userName;
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("GetAd", new { id = ad.Id}, ad);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto)
        {
            if (updatedAdDto == null)
            {
                return NotFound("Ad for update not found");
            }
            var ad = await _adService.GetAsync(id);
            if (ad == null || ad.Id != id)
            {
                return BadRequest();
            }

            if (HttpContext.User.Identity?.Name != ad.UserName)
            {
                return BadRequest("Users don't match");
            }

            ad = _mapper.Map(updatedAdDto, ad);
            await _adService.UpdateAsync(ad);
            return CreatedAtRoute("GetAd", new { id = ad.Id }, ad);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var adForDelete = await _adService.GetAsync(id);
            if (adForDelete == null)
            {
                return NotFound("Ad for delete not found");
            }
            if (HttpContext.User.Identity?.Name != adForDelete.UserName)
            {
                return BadRequest("Users don't match");
            }
            var deletedAd = await _adService.DeleteAsync(adForDelete);

            return new ObjectResult(deletedAd);
        }
    }
}
