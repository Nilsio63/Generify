using Generify.Models.Management;
using Microsoft.EntityFrameworkCore;

namespace Generify.Repositories
{
    public class GenerifyDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public GenerifyDataContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
