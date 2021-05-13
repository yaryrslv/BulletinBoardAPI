using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.DTO.RatinAction;

namespace Web.Services.Abstractions
{
    public interface IRatingActionService
    {
        Task<IEnumerable<RatingActionFullDto>> GetAllAsync();
        Task<IEnumerable<RatingActionFullDto>> GetAllByAdIdAsync(Guid id);
        Task<RatingActionFullDto> GetByAdIdAndUserNameAsync(Guid id, string userName);
        Task AddAsync(RatingActionFullDto ratingActionFullDto);
        Task RemoveAsync(RatingActionFullDto ratingActionFullDto);
        Task<bool> IsRated(Guid adId, string userName);
    }
}