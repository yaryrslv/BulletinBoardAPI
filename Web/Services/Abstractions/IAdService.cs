using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.DTO.Ad;

namespace Web.Services.Realization
{
    public interface IAdService
    {
        Task<IEnumerable<AdFullDto>> GetAllAsync();
        Task<IEnumerable<AdFullDto>> GetAllActualAsync();
        Task<AdFullDto> GetByIdAsync(Guid id);
        Task<AdFullDto> GetByNumberAsync(int number);
        Task<IEnumerable<AdFullDto>> GetByNameAsync(string name);
        Task<IEnumerable<AdFullDto>> GetByCityAsync(string city);
        Task CreateAsync(AdFullDto adFullDto);
        Task UpdateAsync(AdFullDto adFullDto);
        Task<AdFullDto> DeleteAsync(AdFullDto adFullDto);
    }
}