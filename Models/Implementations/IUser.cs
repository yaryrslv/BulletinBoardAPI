using System;

namespace BulletinBoardAPI.Models.Implementations
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
