using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.EF;
using Data.Models.Realizations;
using Microsoft.EntityFrameworkCore;
using Web.DTO.RatinAction;
using Web.Services.Abstractions;

namespace Web.Services.Realization
{
    public class RatingActionService : IRatingActionService
    {
        private BulletinBoardContext _context;
        private readonly IMapper _mapper;
        public RatingActionService(BulletinBoardContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RatingActionFullDto>> GetAllAsync()
        {
            var ratinActions = await _context.RatingActions.ToListAsync();
            IEnumerable<RatingActionFullDto> ratingActionsFullDtos;
            ratingActionsFullDtos = _mapper.Map(ratinActions, (IEnumerable<RatingActionFullDto>) null);
            return ratingActionsFullDtos;
        }
        public async Task<IEnumerable<RatingActionFullDto>> GetAllByAdIdAsync(Guid id)
        {
            var ratinActions = await _context.RatingActions.Where(i => i.AdId == id).ToListAsync();
            IEnumerable<RatingActionFullDto> ratingActionsFullDtos;
            ratingActionsFullDtos = _mapper.Map(ratinActions, (IEnumerable<RatingActionFullDto>)null);
            return ratingActionsFullDtos;
        }
        public async Task<RatingActionFullDto> GetByAdIdAndUserNameAsync(Guid id, string userName)
        {
            var ratinAction = await _context.RatingActions.Where(i => i.AdId == id)
                .FirstOrDefaultAsync(i => i.UserName == userName);
            RatingActionFullDto ratingActionFullDto = _mapper.Map<RatingActionFullDto>(ratinAction);
            return ratingActionFullDto;
        }
        public async Task AddAsync(RatingActionFullDto ratingActionFullDto)
        {
            var ratingAction = _mapper.Map<RatingAction>(ratingActionFullDto);
            ratingAction.Time = DateTime.Now;
            await _context.RatingActions.AddAsync(ratingAction);
            var ratedByIdAd = await GetAllByAdIdAsync(ratingAction.AdId);
            var ratedByIdAdCount = ratedByIdAd.Count();
            var currentRatedAd = await _context.Ads.FindAsync(ratingAction.AdId);
            currentRatedAd.Rating = ratedByIdAdCount;
            currentRatedAd.Rating++;
            _context.Ads.Update(currentRatedAd);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAsync(RatingActionFullDto ratingActionFullDto)
        {
            var ratingAction = _mapper.Map<RatingAction>(ratingActionFullDto);
            _context.RatingActions.Remove(ratingAction);
            var ratedByIdAd = await GetAllByAdIdAsync(ratingAction.AdId);
            var ratedByIdAdCount = ratedByIdAd.Count();
            var currentRatedAd = await _context.Ads.FindAsync(ratingAction.AdId);
            currentRatedAd.Rating = ratedByIdAdCount;
            currentRatedAd.Rating--;
            _context.Ads.Update(currentRatedAd);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsRated(Guid adId, string userName)
        {
            var ratingActionsByAdId = await GetAllByAdIdAsync(adId);
            var isRated = ratingActionsByAdId.Any(i => i.UserName == userName);
            return isRated;
        }
    }
}
