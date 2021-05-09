using System;

namespace Data.Models.Abstractions
{
    public interface IAd
    {
        Guid Id { get; set; }
        int Number { get; set; }
        public string UserName { get; set; }
        string City { get; set; }
        string Text { get; set; }
        string ImageUrl { get; set; }
        int Rating { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ExpirationDite { get; set; }
    }
}
