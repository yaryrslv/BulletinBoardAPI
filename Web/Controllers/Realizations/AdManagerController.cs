using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Models.Realizations;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Abstractions;
using Web.DTO.Ad;
using Web.Services.Abstractions;

namespace Web.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdManagerController : ControllerBase, IAdManagerController
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;

        public AdManagerController(IAdService adService, IMapper mapper)
        {
            _adService = adService;
            _mapper = mapper;
        }
        /// <summary>
        /// [AdminRightsRequrered] Get all existing Ads in extended format.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getall", Name = "ManagerGetAllAds")]
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adService.GetAllAsync();
        }
        /// <summary>
        /// [AdminRightsRequrered] Get Ad of any User by Id in extended format.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyadid/{id}", Name = "ManagerGetAd")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Ad ad = await _adService.GetByIdAsync(id);
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
        /// [AdminRightsRequrered] Get Ad of any User by Id in extended format.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyusername/{name}", Name = "ManagerGetAdByName")]
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _adService.GetByNameAsync(name);
        }
        /// <summary>
        /// [AdminRightsRequrered] Updates extended Ad fields of any User by UserName.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("updatebyid/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AdDto updatedAdDto)
        {
            if (updatedAdDto == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var updatedAd = await _adService.GetByIdAsync(id);
            if (updatedAd == null || updatedAd.Id != id)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong input data"
                });
            }
            updatedAd = _mapper.Map(updatedAdDto, updatedAd);
            await _adService.UpdateAsync(updatedAd);
            return CreatedAtRoute("ManagerGetAd", new { id = updatedAd.Id }, updatedAd);
        }
        /// <summary>
        /// [AdminRightsRequrered] Deletes any Ad of any User by Id.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
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
            var deletedAd = await _adService.DeleteAsync(adForDelete);

            return new ObjectResult(deletedAd);
        }
    }
}
