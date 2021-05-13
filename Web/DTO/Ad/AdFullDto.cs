using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Ad
{
    public class AdFullDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDite { get; set; }
    }
}
