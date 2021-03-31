using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoardAPI.Models.Implementations
{
    public interface IAd
    {
        int Id { get; set; }
        int Number { get; set; }
        User User { get; set; }
        string Text { get; set; }
        string ImageURL { get; set; }
        int Rating { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ExpirationDite { get; set; }
    }
}
