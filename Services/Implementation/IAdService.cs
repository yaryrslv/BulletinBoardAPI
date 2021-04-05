using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Services.Implementation
{
    public interface IAdService
    {
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<Ad> GetAsync(Guid id);
        Task CreateAsync(Ad ad);
        Task UpdateAsync(Ad ad, Ad updatedAd);
        Task<Ad> DeleteAsync(Guid id);
    }
}
