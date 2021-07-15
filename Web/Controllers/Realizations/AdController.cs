using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.DTO.Ad;
using Web.FluentValidator;
using Web.Services.Realization;
using ObjectResult = Microsoft.AspNetCore.Mvc.ObjectResult;

namespace Web.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase, IAdController
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public IConfiguration Configuration { get; }
        public AdController(IAdService adService, IMapper mapper, 
            IConfiguration configuration, UserManager<User> userManager)
        {
            _adService = adService;
            _mapper = mapper;
            Configuration = configuration;
            _userManager = userManager;
        }
        /// <summary>
        /// [AuthorizeRequired] Get all existing Ads.
        /// </summary>
        [Authorize]
        [HttpGet("getall", Name = "GetAllAds")]
        public async Task<IEnumerable<AdFullDto>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        /// <summary>
        /// [AuthorizeRequired] Show only Ads whose ExpirationDite has not yet arrived.
        /// </summary>
        [Authorize]
        [HttpGet("getallactual", Name = "GetAllActualAds")]
        public async Task<IEnumerable<AdFullDto>> GetAllActualAsync()
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
        public async Task<IEnumerable<AdFullDto>> GetByNameAsync(string name)
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
            var validator = new AdDtoValidator();
            var result = validator.Validate(adDto);
            if (!result.IsValid)
            {
                string errorsString = "";
                foreach (var error in result.Errors)
                {
                    errorsString += error + " | ";
                }
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = errorsString
                });
            }
            var userName = HttpContext.User.Identity.Name;
            var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName);
            var adFullDto = _mapper.Map<AdFullDto>(adDto);
            adFullDto.UserId = user.Id;
            //var userAds = await _adService.GetByNameAsync(userName);
            //var userAdsCount = userAds.Count();
            //if (userAdsCount >= int.Parse(Configuration["MaxUserAds"]))
            //{
            //    return BadRequest(new Response()
            //    {
            //        Status = "BadRequest",
            //        Message = "User Ads count quota exceeded"
            //    });
            //}
            adFullDto.Id = Guid.NewGuid();
            await _adService.CreateAsync(adFullDto);
            var updatedAdFullDto = _adService.GetByIdAsync(adFullDto.Id);
            if (updatedAdFullDto.Result == null)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "User Ads count quota exceeded"
                });
            }
            return CreatedAtRoute("GetAdById", new { id = updatedAdFullDto.Id}, updatedAdFullDto);
        }
        /// <summary>
        /// [AuthorizeRequired] Updates the Ad of the current current Authorized identity User by its Id.
        /// </summary>
        [Authorize]
        [HttpPut("updatebyid/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName);
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
            if (user.Id != ad.UserId)
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
            var adForDeleteDto = await _adService.GetByIdAsync(id);
            if (adForDeleteDto == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }

            var userName = HttpContext.User.Identity?.Name;
            var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName);
            if (user.Id != adForDeleteDto.UserId)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var deletedAdDto = await _adService.DeleteAsync(adForDeleteDto);
            return new ObjectResult(deletedAdDto);
        }
    }
}
