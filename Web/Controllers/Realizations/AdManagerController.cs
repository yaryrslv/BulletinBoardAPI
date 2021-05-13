using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTO.Ad;
using Web.Services.Realization;

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
        public async Task<IEnumerable<AdFullDto>> GetAllAsync()
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
            var adFullDto = await _adService.GetByIdAsync(id);
            if (adFullDto == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            return new ObjectResult(adFullDto);
        }
        /// <summary>
        /// [AdminRightsRequrered] Get Ad of any User by Id in extended format.
        /// </summary>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getbyusername/{name}", Name = "ManagerGetAdByName")]
        public async Task<IEnumerable<AdFullDto>> GetByNameAsync(string name)
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
            var updatedAdFullDto = await _adService.GetByIdAsync(id);
            if (updatedAdFullDto == null || updatedAdFullDto.Id != id)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong input data"
                });
            }
            updatedAdFullDto = _mapper.Map(updatedAdDto, updatedAdFullDto);
            await _adService.UpdateAsync(updatedAdFullDto);
            return CreatedAtRoute("ManagerGetAd", new { id = updatedAdFullDto.Id }, updatedAdFullDto);
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
            var deletedAdFullDto = await _adService.DeleteAsync(adForDelete);

            return new ObjectResult(deletedAdFullDto);
        }
    }
}
