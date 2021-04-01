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
    public class UserService : IUserService
    {
        private BulletinBoardContext _context;
        public UserService(BulletinBoardContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.UserItems.Where(i => i != null).ToListAsync();
        }
        public async Task<User> GetAsync(Guid id)
        {
            return await _context.UserItems.FindAsync(id);
        }

        public async Task CreateAsync(User item)
        {
            await _context.UserItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User item)
        {
            _ = await GetAsync(item.Id);
            User currentItem = item;
            _context.UserItems.Update(currentItem);
            await _context.SaveChangesAsync();
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            User userItem = await GetAsync(id);

            if (userItem != null)
            {
                _context.UserItems.Remove(userItem);
                await _context.SaveChangesAsync();
            }

            return await _context.UserItems.FindAsync(userItem.Id);
        }
    }
}

