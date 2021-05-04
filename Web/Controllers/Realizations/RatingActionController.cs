using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BulletinBoardAPI.Controllers.Abstractions;
using BulletinBoardAPI.DTO.Rating;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardAPI.Controllers.Realizations
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingActionController : ControllerBase, IRatingActionController
    {
        private readonly IRatingActionService _ratingActionService;
        private readonly IAdService _adService;
        private readonly IMapper _mapper;
        public RatingActionController(IRatingActionService ratingActionService, IAdService adService, IMapper mapper)
        {
            _ratingActionService = ratingActionService;
            _adService = adService;
            _mapper = mapper;
        }
        /// <summary>
        /// [AuthorizeRequired] Receives all RatingActions for the entire time.
        /// </summary>
        [Authorize]
        [HttpGet("getall", Name = "GetAllRatingActions")]
        public async Task<IEnumerable<RatingAction>> GetAllAsync()
        {
            return await _ratingActionService.GetAllAsync();
        }
        /// <summary>
        /// [AuthorizeRequired] Receives all Ad RatingActions by Ad Id.
        /// </summary>
        [Authorize]
        [HttpGet("getallbyadid/{adId}", Name = "GetAllRatingActionsByAdId")]
        public async Task<IActionResult> GetAllByAdIdAsync(Guid adId)
        {
            if (await _adService.GetByIdAsync(adId) == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var response = await _ratingActionService.GetAllByAdIdAsync(adId);
            return new JsonResult(response);
        }
        /// <summary>
        /// [AuthorizeRequired] Creates new RatingAction.
        /// </summary>
        [Authorize]
        [HttpPost("addbyid")]
        public async Task<IActionResult> AddByIdAsync([FromBody] RatingActionDto ratingActionDto)
        {
            if (ratingActionDto == null)
            {
                return BadRequest(new Response()
                {
                    Status = "BadRequest",
                    Message = "Wrong input data"
                });
            }
            var ad = await _adService.GetByIdAsync(ratingActionDto.AdId);
            if (ad == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var currentUserName = HttpContext.User.Identity?.Name;
            if (await _ratingActionService.IsRated(ad.Id, currentUserName))
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "The ad has already been rated"
                });
            } 
            var ratingAction = new RatingAction();
            ratingAction = _mapper.Map(ratingActionDto, ratingAction);
            ratingAction.UserName = currentUserName;
            await _ratingActionService.AddAsync(ratingAction);
            return CreatedAtRoute("GetAdById", new { id = ad.Id }, ad);
        }
        /// <summary>
        /// [AuthorizeRequired] Removes existing RatingAction.
        /// </summary>
        [Authorize]
        [HttpDelete("removebyid/{adId}")]
        public async Task<IActionResult> RemoveByIdAsync(Guid adId)
        {
            var ad = await _adService.GetByIdAsync(adId);
            if (ad == null)
            {
                return NotFound(new Response()
                {
                    Status = "NotFound",
                    Message = "Ad not found"
                });
            }
            var currentUserName = HttpContext.User.Identity?.Name;
            if (await _ratingActionService.IsRated(ad.Id, currentUserName) == false)
            {
                return Conflict(new Response()
                {
                    Status = "Conflict",
                    Message = "This ad has not yet been rated"
                });
            }
            var ratingAction = await _ratingActionService.GetByAdIdAndUserNameAsync(adId, currentUserName);
            await _ratingActionService.RemoveAsync(ratingAction);
            return CreatedAtRoute("GetAdById", new { id = ad.Id }, ad);
        }
    }
}
