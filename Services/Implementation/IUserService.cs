using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Implementation
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(Guid id);
        Task CreateAsync(User user);
        Task UpdateAsync(User user, User updatedUser);
        Task<User> DeleteAsync(Guid id);
        Task<bool> IsUserNameExistsAsync(string name);
        Task<User> GetUserByName(string name);
    }
}
