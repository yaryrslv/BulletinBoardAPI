using System;

namespace BulletinBoardAPI.Models.Abstractions
{
    public interface IRatingAction
    {
        Guid Id { get; set; }
        DateTime Time { get; set; }
        Guid AdId { get; set; }
        string UserName { get; set; }
    }
}