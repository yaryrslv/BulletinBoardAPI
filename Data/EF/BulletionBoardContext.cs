using BulletinBoardAPI.Models.Realizations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.EF
{
    public class BulletinBoardContext : IdentityDbContext<User>
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<RatingAction> RatingActions { get; set; }
        public BulletinBoardContext(DbContextOptions<BulletinBoardContext> options) : base(options)
        {
        }
    }
}
