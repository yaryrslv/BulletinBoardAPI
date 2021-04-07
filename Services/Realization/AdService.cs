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
            return await _context.AdItems.ToListAsync();
        }
        public async Task<IEnumerable<Ad>> GetAllActualAsync()
        {
            return await _context.AdItems.Where(i => i.ExpirationDite > DateTime.Now).ToListAsync();
        }
        public async Task<IEnumerable<Ad>> GetByNameAsync(string name)
        {
            return await _context.AdItems.Where(i => i.UserName == name).ToListAsync();
        }
        public async Task<IEnumerable<Ad>> GetActualByNameAsync(string name)
        {
            return await _context.AdItems.Where(i => i.UserName == name)
                .Where(i => i.ExpirationDite > DateTime.Now).ToListAsync();
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
        public async Task UpdateAsync(Ad ad)
        {
            _context.AdItems.Update(ad);
            await _context.SaveChangesAsync();
        }

        public async Task<Ad> DeleteAsync(Ad ad)
        {

            if (ad != null)
            {
                _context.AdItems.Remove(ad);
                await _context.SaveChangesAsync();
            }

            return ad;
        }
        private async Task<int> GetPostCount()
        {
            return await _context.AdItems.CountAsync();
        }
    }
}
