using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.Models.Realizations
{
    public class User : IdentityUser
    {
        public ICollection<Ad> Ads { get; set; }

        public User()
        {
            Ads = new List<Ad>();
        }
    }
}
