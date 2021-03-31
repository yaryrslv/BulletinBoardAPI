using System;

namespace BulletinBoardAPI.Models.Implementations
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
        bool IsAdmin { get; set; }
    }
}
