using BulletinBoardAPI.Models.Realizations;
using Data.Models.Realizations;
using Microsoft.AspNetCore.Identity;
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
            this.SeedRoles(builder);
            builder.Entity<Ad>(b =>
            {
                b.HasIndex(p => p.Id);
                b.Property(p => p.Number).IsRequired().HasMaxLength(512);
                b.Property(p => p.Text).IsRequired().HasMaxLength(2048);
                b.Property(p => p.Rating).IsRequired().HasDefaultValue(0);
                b.Property(p => p.CreateDate).IsRequired();
                b.Property(p => p.ExpirationDite).IsRequired();
                b.Property(p => p.City).HasMaxLength(512);
                b.Property(p => p.ImageUrl).HasMaxLength(1048);
                b.ToTable("Ads");
            });
            builder.HasSequence("DBSequence")
                .StartsAt(0).IncrementsBy(1);
            builder.Entity<Ad>()
                .Property(p => p.Number)
                .HasDefaultValueSql("NEXT VALUE FOR DBSequence");
            builder.Entity<RatingAction>(b =>
            {
                b.Property(p => p.AdId).IsRequired();
                b.Property(p => p.UserName).IsRequired().HasMaxLength(512);
                b.Property(p => p.Time).IsRequired();
                b.ToTable("RatingActions");
            });

        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "0", NormalizedName = "Admin" },
                new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "User", ConcurrencyStamp = "1", NormalizedName = "User" }
            );
        }

    }
}
