using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Abstractions;
using BulletinBoardAPI.Models.Realizations;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.DTO.Ad;
using Web.Services.Abstractions;
using ObjectResult = Microsoft.AspNetCore.Mvc.ObjectResult;

namespace Web.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase, IAdController
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;
        public IConfiguration Configuration { get; }
        public AdController(IAdService adService, IMapper mapper, IConfiguration configuration)
        {
            _adService = adService;
            _mapper = mapper;
            Configuration = configuration;
        }
        /// <summary>
        /// [AuthorizeRequired] Get all existing Ads.
        /// </summary>
        [Authorize]
        [HttpGet("getall", Name = "GetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        /// <summary>
        /// [AuthorizeRequired] Show only Ads whose ExpirationDite has not yet arrived.
        /// </summary>
        [Authorize]
        [HttpGet("getallactual", Name = "GetAllActualAds")]
        public async Task<IEnumerable<Ad>> GetAllActualAsync()
        {
            return await _adService.GetAllActualAsync();
        }
        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its Id.
        /// </summary>
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
        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its UserName.
        /// </summary>
        [Authorize]
        [HttpGet("getbyusername/{name}", Name = "GetAdsByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        /// <summary>
        /// [AuthorizeRequired] Finds an Ad by its City.
        /// </summary>
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
        /// <summary>
        /// [AuthorizeRequired] Creates a new Ad.
        /// </summary>
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
            var userAds = await _adService.GetByNameAsync(userName);
            var userAdsCount = userAds.Count();
            if (userAdsCount >= int.Parse(Configuration["MaxUserAds"]))
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "User Ads count quota exceeded"
                });
            }
            await _adService.CreateAsync(ad);
            return CreatedAtRoute("GetAdById", new { id = ad.Id}, ad);
        }
        /// <summary>
        /// [AuthorizeRequired] Updates the Ad of the current current Authorized identity User by its Id.
        /// </summary>
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
        /// <summary>
        /// [AuthorizeRequired] Deletes the Ad of the current Authorized identity User by its Id.
        /// </summary>
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
