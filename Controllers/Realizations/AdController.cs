using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Implementations;
using BulletinBoardAPI.DTO.Ad;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Realization;
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
        [HttpGet("getall", Name = "GetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        [Authorize]
        [HttpGet("getallactual", Name = "GetAllActualAds")]
        public async Task<IEnumerable<Ad>> GetAllActualAsync()
        {
            return await _adService.GetAllActualAsync();
        }
        [Authorize]
        [HttpGet("getbyadid/{id}", Name = "GetAdById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var ad = await _adService.GetByIdAsync(id);
            if (ad == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            return new ObjectResult(ad);
        }
        [Authorize]
        [HttpGet("getbyusername/{name}", Name = "GetAdsByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        [Authorize]
        [HttpGet("getbycity/{city}", Name = "GetAdByCity")]
        public async Task<IActionResult> GetByCityAsync(string city)
        {
            var ad = await _adService.GetByCityAsync(city);
            if (ad == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            return new ObjectResult(ad);
        }
        [Authorize]
        [HttpPost("postnew")]
        public async Task<IActionResult> CreateAsync([FromBody] AdDto adDto)
        {
            if (adDto == null)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong input data"
                });
            }
            var userName = HttpContext.User.Identity?.Name;
            var ad = _mapper.Map<Ad>(adDto);
            ad.UserName = userName;
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("GetAdById", new { id = ad.Id}, ad);
        }
        [Authorize]
        [HttpPut("updatebyid/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto)
        {
            if (updatedAdDto == null)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong input data"
                });
            }
            var ad = await _adService.GetByIdAsync(id);
            if (ad == null || ad.Id != id)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            if (HttpContext.User.Identity?.Name != ad.UserName)
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "Users don't match"
                });
            }
            ad = _mapper.Map(updatedAdDto, ad);
            await _adService.UpdateAsync(ad);
            return CreatedAtRoute("GetAdById", new { id = ad.Id }, ad);
        }
        [Authorize]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var adForDelete = await _adService.GetByIdAsync(id);
            if (adForDelete == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            if (HttpContext.User.Identity?.Name != adForDelete.UserName)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var deletedAd = await _adService.DeleteAsync(adForDelete);
            return new ObjectResult(deletedAd);
        }
    }
}
