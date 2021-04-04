using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoardAPI.EF;
using BulletinBoardAPI.Models.Realizations;
using BulletinBoardAPI.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.Services.Realization
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
            return await _context.AdItems.Include(i => i.User).ToListAsync();
        }
        public async Task<Ad> GetAsync(Guid id)
        {
            return await _context.AdItems.FindAsync(id);
        }
        
        public async Task CreateAsync(Ad ad)
        {
            ad.CreateDate = DateTime.Now;
            ad.ExpirationDite = ad.CreateDate.AddMonths(1);
            ad.Rating = 0;
            ad.Number = await GetPostCount();
            await _context.AdItems.AddAsync(ad);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Ad ad, Ad updatedAd)
        {
            ad.Text = updatedAd.Text;
            ad.ImageUrl = ad.ImageUrl;
            _context.AdItems.Update(ad);
            await _context.SaveChangesAsync();
        }

        public async Task<Ad> DeleteAsync(Guid id)
        {
            Ad adItem = await GetAsync(id);

            if (adItem != null)
            {
                _context.AdItems.Remove(adItem);
                await _context.SaveChangesAsync();
            }

            return adItem;
        }

        private async Task<int> GetPostCount()
        {
            return await _context.AdItems.CountAsync();
        }
    }
}
