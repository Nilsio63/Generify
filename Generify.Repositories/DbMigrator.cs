using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Generify.Repositories;

public interface IDbMigrator
{
    Task MigrateAsync();
}

public class DbMigrator(
    ILogger<DbMigrator> logger,
    GenerifyDataContext dbContext)
    : IDbMigrator
{
    public async Task MigrateAsync()
    {
        IEnumerable<string> appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

        logger.LogInformation("Applied migrations: {appliedMigrations}", appliedMigrations.DefaultIfEmpty("<None>").ToArray());

        IEnumerable<string> pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        logger.LogInformation("Pending migrations: {appliedMigrations}", pendingMigrations.DefaultIfEmpty("<None>").ToArray());

        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Migrated database successfully");
    }
}
