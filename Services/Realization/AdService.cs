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
            return await _context.AdItems.Where(i => i != null).ToListAsync();
        }
        public async Task<Ad> GetAsync(Guid id)
        {
            return await _context.AdItems.FindAsync(id);
        }
        
        public async Task CreateAsync(Ad item)
        {
            await _context.AdItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Ad item)
        {
            _ = await GetAsync(item.Id);
            Ad currentItem = item;
            _context.AdItems.Update(currentItem);
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

            return await _context.AdItems.FindAsync(adItem.Id);
        }
    }
}
