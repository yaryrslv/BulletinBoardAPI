using System;
using BulletinBoardAPI.Models.Implementations;

namespace BulletinBoardAPI.Models.Realizations
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
