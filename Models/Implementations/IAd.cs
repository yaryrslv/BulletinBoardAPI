using System;
using BulletinBoardAPI.Models.Realizations;

namespace BulletinBoardAPI.Models.Implementations
{
    public interface IAd
    {
        Guid Id { get; set; }
        int Number { get; set; }
        User User { get; set; }
        string Text { get; set; }
        string ImageUrl { get; set; }
        int Rating { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ExpirationDite { get; set; }
    }
}
