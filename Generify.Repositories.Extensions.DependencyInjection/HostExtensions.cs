using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Generify.Repositories.Extensions.DependencyInjection;

public static class HostExtensions
{
    public static async Task UseDatabaseAsync(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();

        IDbMigrator dbMigrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();

        await dbMigrator.MigrateAsync();
    }
}
