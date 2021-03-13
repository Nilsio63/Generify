using Generify.Repositories.Abstractions.Management;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Repositories.Management;
using Generify.Repositories.Playlists;
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
                    dbOptionsAction.Invoke(dbOptions);
                })
                .AddTransient<IPlaylistDefinitionRepository, PlaylistDefinitionRepository>()
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
