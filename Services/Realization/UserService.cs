using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public async Task RegisterAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.Role = UserRoles.User;
            user.Password = GetHash(user.Password);
            await _context.UserItems.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.UserItems.Where(i => i != null).ToListAsync();
        }
        public async Task<User> GetAsync(Guid id)
        {
            return await _context.UserItems.FindAsync(id);
        }

        public async Task<User> UpdateAsync(User user, User updatedUser)
        {
            user.Name = updatedUser.Name;
            user.Role = updatedUser.Role;
            _context.UserItems.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            User userItem = await GetAsync(id);

            if (userItem != null)
            {
                _context.UserItems.Remove(userItem);
                await _context.SaveChangesAsync();
            }

            return userItem;
        }
        public async Task<bool> IsUserNameExistsAsync(string name)
        {
            return await _context.UserItems.AnyAsync(i => i.Name == name);
        }
        public async Task<User> GetUserByName(string name)
        {
            if (await IsUserNameExistsAsync(name))
            {
                return await _context.UserItems.FirstAsync(i => i.Name == name);
            }

            return null;
        }

        public string GetHash(string userPassword)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(userPassword));
            return Convert.ToBase64String(hash);
        }
    }
}

