using System;
using Data.Models.Abstractions;

namespace Data.Models.Realizations
{
    public class Ad : IAd
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string City { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDite { get; set; }
    }
}
