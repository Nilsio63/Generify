using Generify.External.Abstractions;
using Generify.External.Abstractions.Services;
using Generify.External.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.External.Extensions.DependencyInjection
{
    public static class ExternalDiExtensions
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ISpotifyClientFactory, SpotifyClientFactory>()
                .AddTransient<IPlaylistEditService, PlaylistEditService>()
                .AddTransient<IPlaylistInfoService, PlaylistInfoService>()
                .AddTransient<ITrackInfoService, TrackInfoService>();
        }
    }
}
