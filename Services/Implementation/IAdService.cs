using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Implementations;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Implementation
{
    public interface IAdService
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<Ad> GetAsync(Guid id);
        Task CreateAsync(Ad item);
        Task UpdateAsync(Ad item);
        Task<Ad> DeleteAsync(Guid id);
    }
}
