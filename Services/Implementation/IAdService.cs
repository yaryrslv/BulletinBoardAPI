using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Realization
{
    public interface IAdService
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<IEnumerable<Ad>> GetAllActualAsync();
        Task<Ad> GetByIdAsync(Guid id);
        Task<Ad> GetByNumberAsync(int number);
        Task<IEnumerable<Ad>> GetByNameAsync(string name);
        Task<IEnumerable<Ad>> GetByCityAsync(string city);
        Task CreateAsync(Ad ad);
        Task UpdateAsync(Ad ad);
        Task<Ad> DeleteAsync(Ad ad);
    }
}