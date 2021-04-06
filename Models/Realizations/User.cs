using Microsoft.AspNetCore.Identity;
using System;

namespace BulletinBoardAPI.Models.Realizations
{
    public class User : IdentityUser
    {
        public static implicit operator string(User v)
        {
            throw new NotImplementedException();
        }
    }
}
