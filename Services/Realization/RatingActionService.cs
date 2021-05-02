using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoardAPI.EF;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.Services.Realization
{
    public class RatingActionService : IRatingActionService
    {
        private BulletinBoardContext _context;
        public RatingActionService(BulletinBoardContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RatingAction>> GetAllAsync()
        {
            return await _context.RatingActions.ToListAsync();
        }
        public async Task<IEnumerable<RatingAction>> GetAllByAdIdAsync(Guid id)
        {
            return await _context.RatingActions.Where(i => i.AdId == id).ToListAsync();
        }
        public async Task<RatingAction> GetByAdIdAndUserNameAsync(Guid id, string userName)
        {
            return await _context.RatingActions.Where(i => i.AdId == id)
                .FirstOrDefaultAsync(i => i.UserName == userName);
        }
        public async Task AddAsync(RatingAction ratingAction)
        {
            ratingAction.Id = Guid.NewGuid();
            ratingAction.Time = DateTime.Now;
            await _context.RatingActions.AddAsync(ratingAction);
            await _context.SaveChangesAsync();
            var ratedByIdAd = await GetAllByAdIdAsync(ratingAction.AdId);
            var ratedByIdAdCount = ratedByIdAd.Count();
            var currentRatedAd = await _context.Ads.FindAsync(ratingAction.AdId);
            currentRatedAd.Rating = ratedByIdAdCount;
            _context.Ads.Update(currentRatedAd);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAsync(RatingAction ratingAction)
        {
            _context.RatingActions.Remove(ratingAction);
            await _context.SaveChangesAsync();
            var ratedByIdAd = await GetAllByAdIdAsync(ratingAction.AdId);
            var ratedByIdAdCount = ratedByIdAd.Count();
            var currentRatedAd = await _context.Ads.FindAsync(ratingAction.AdId);
            currentRatedAd.Rating = ratedByIdAdCount;
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
