using System.Collections;
using System.Collections.Generic;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoardAPI.Models.Realizations
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
