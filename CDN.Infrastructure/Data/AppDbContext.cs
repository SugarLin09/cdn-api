using CDN.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDN.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

}