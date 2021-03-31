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
        void CreateAsync(Ad item);
        void UpdateAsync(Ad item);
        Task<Ad> DeleteAsync(Guid id);
    }
}
