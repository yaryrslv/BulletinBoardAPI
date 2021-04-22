using System;
using BulletinBoardAPI.Models.Implementations;

namespace BulletinBoardAPI.Models.Realizations
{
    public class RatingAction : IRatingAction
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Guid AdId { get; set; }
        public string UserName { get; set; }
    }
}
