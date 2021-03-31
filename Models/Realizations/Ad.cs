using System;
using BulletinBoardAPI.Models.Implementations;

namespace BulletinBoardAPI.Models.Realizations
{
    public class Ad : IAd
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public string ImageURL { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDite { get; set; }
    }
}
