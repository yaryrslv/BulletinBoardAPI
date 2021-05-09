using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;

namespace Web.Services.Abstractions
{
    public interface IRatingActionService
    {
        Task<IEnumerable<RatingAction>> GetAllAsync();
        Task<IEnumerable<RatingAction>> GetAllByAdIdAsync(Guid id);
        Task<RatingAction> GetByAdIdAndUserNameAsync(Guid id, string userName);
        Task AddAsync(RatingAction ratingAction);
        Task RemoveAsync(RatingAction ratingAction);
        Task<bool> IsRated(Guid adId, string userName);
    }
}