using BulletinBoardAPI.Models.Realizations;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.EF
{
    public class BulletinBoardContext : DbContext
    {
        public BulletinBoardContext(DbContextOptions<BulletinBoardContext> options) : base(options)
        { }
        public DbSet<Ad> AdItems { get; set; }
        public DbSet<User> UserItems { get; set; }
    }
}
