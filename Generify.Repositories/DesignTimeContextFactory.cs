using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Generify.Repositories;

internal class DesignTimeContextFactory : IDesignTimeDbContextFactory<GenerifyDataContext>
{
    public GenerifyDataContext CreateDbContext(string[] args)
    {
        string connectionString = "Server=localhost;Port=3306;Database=leeklog;User=root;Password=TestPassword123!";
        var optionsBuilder = new DbContextOptionsBuilder<GenerifyDataContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.0"));

        return new GenerifyDataContext(optionsBuilder.Options);
    }
}
