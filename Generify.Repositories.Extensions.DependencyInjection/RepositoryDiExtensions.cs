using Generify.Repositories.Interfaces.Management;
using Generify.Repositories.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Generify.Repositories.Extensions.DependencyInjection
{
    public static class RepositoryDiExtensions
    {
        public static IServiceCollection AddGenerifyRepos(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptionsAction)
        {
            return services
                .AddDbContext<GenerifyDataContext>(dbOptions =>
                {
                    dbOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                    dbOptionsAction.Invoke(dbOptions);
                })
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
