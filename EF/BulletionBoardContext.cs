using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoardAPI.Models.Realizations;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI.EF
{
    public class BulletionBoardContext : DbContext
    {
        public BulletionBoardContext(DbContextOptions<BulletionBoardContext> options) : base(options)
        { }
        public DbSet<Ad> AdItems { get; set; }
        public DbSet<User> UserItems { get; set; }
    }
}
