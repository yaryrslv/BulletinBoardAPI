using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.EF;
using Data.Models.Realizations;
using Microsoft.EntityFrameworkCore;
using Web.Services.Abstractions;

namespace Web.Services.Realization
{
    public class AdService : IAdService
    {
        private BulletinBoardContext _context;
        public AdService(BulletinBoardContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _context.Ads.ToListAsync();
        }
        public async Task<IEnumerable<Ad>> GetAllActualAsync()
        {
            return await _context.Ads.Where(i => i.ExpirationDite > DateTime.Now).ToListAsync();
        }
        public async Task<Ad> GetByIdAsync(Guid id)
        {
            return await _context.Ads.FindAsync(id);
        }
        public async Task<Ad> GetByNumberAsync(int number)
        {
            return await _context.Ads.FirstOrDefaultAsync(i => i.Number == number);
        }
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _context.Ads.Where(i => i.UserName == name).ToListAsync();
        }
        public async Task<IEnumerable<Ad>> GetByCityAsync(string city)
        {
            return await _context.Ads.Where(i => i.City == city).ToListAsync();
        }
        public async Task CreateAsync(Ad ad)
        {
            ad.CreateDate = DateTime.Now;
            ad.ExpirationDite = ad.CreateDate.AddMonths(1);
            ad.Rating = 0;
            await _context.Ads.AddAsync(ad);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Ad ad)
        {
            _context.Ads.Update(ad);
            await _context.SaveChangesAsync();
        }
        public async Task<Ad> DeleteAsync(Ad ad)
        {

            if (ad != null)
            {
                _context.Ads.Remove(ad);
                await _context.SaveChangesAsync();
            }

            return ad;
        }
        private async Task<int> GetPostCount()
        {
            return await _context.Ads.CountAsync();
        }
    }
}
