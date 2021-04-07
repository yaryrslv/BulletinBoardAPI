using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Implementation
{
    public interface IAdService
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IEnumerable<Ad>> GetAllActualAsync();
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IEnumerable<Ad>> GetActualByNameAsync(string name);
        Task<Ad> GetAsync(Guid id);
        Task CreateAsync(Ad ad);
        Task UpdateAsync(Ad ad);
        Task<Ad> DeleteAsync(Ad ad);
    }
}