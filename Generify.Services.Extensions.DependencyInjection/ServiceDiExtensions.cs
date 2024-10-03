using Generify.Services.Abstractions.Management;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Internal;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Management;
using Generify.Services.Playlists;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IExternalAuthService, ExternalAuthService>()
                .AddTransient<IPlaylistGenerator, PlaylistGenerator>()
                .AddTransient<IPlaylistSyncWorker, PlaylistSyncWorker>()
                .AddTransient<IPlaylistGenerationContextBuilder, PlaylistGenerationContextBuilder>()
                .AddTransient<IPlaylistDefinitionService, PlaylistDefinitionService>()
                .AddTransient<IPlaylistOverviewService, PlaylistOverviewService>()
                .AddTransient<IUserService, UserService>();
        }
    }
}
