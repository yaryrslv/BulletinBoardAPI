using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Implementation
{
    interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(Guid id);
        Task CreateAsync(User item);
        Task UpdateAsync(User item);
        Task<User> DeleteAsync(Guid id);
    }
}
