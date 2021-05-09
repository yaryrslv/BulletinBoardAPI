using BulletinBoardAPI.Models.Realizations;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.EF
{
    public sealed class BulletinBoardContext : IdentityDbContext<User>
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<RatingAction> RatingActions { get; set; }
        public BulletinBoardContext(DbContextOptions<BulletinBoardContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ad>(b =>
            {
                b.HasIndex(p => p.Id).IsUnique();
                b.Property(p => p.Number).IsRequired().HasMaxLength(512);
                b.Property(p => p.UserName).IsRequired().HasMaxLength(512);
                b.Property(p => p.Text).IsRequired().HasMaxLength(2048);
                b.Property(p => p.Rating).IsRequired().HasDefaultValue(0);
                b.Property(p => p.CreateDate).IsRequired();
                b.Property(p => p.ExpirationDite).IsRequired();
                b.Property(p => p.City).HasMaxLength(512);
                b.Property(p => p.ImageUrl).HasMaxLength(1048);
                b.ToTable("Ads");
            });
            builder.Entity<RatingAction>(b =>
            {
                b.HasIndex(p => p.Id).IsUnique();
                b.Property(p => p.AdId).IsRequired();
                b.Property(p => p.UserName).IsRequired().HasMaxLength(512);
                b.Property(p => p.Time).IsRequired();
                b.ToTable("RatingActions");
            });

        }
    }
}
