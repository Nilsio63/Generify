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
    }
}
