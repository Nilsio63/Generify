using Generify.External.Abstractions.Services;
using Generify.External.Abstractions.Settings;
using Generify.External.Internal;
using Generify.External.Internal.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Generify.External.Extensions.DependencyInjection
{
    public static class ExternalDiExtensions
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, Func<IServiceProvider, IExternalAuthSettings> settingsFactory)
        {
            return services
                .AddTransient(settingsFactory)
                .AddTransient<ISpotifyClientFactory, SpotifyClientFactory>()
                .AddTransient<ILoginService, LoginService>()
                .AddTransient<IPlaylistEditService, PlaylistEditService>()
                .AddTransient<IPlaylistInfoService, PlaylistInfoService>()
                .AddTransient<ITrackInfoService, TrackInfoService>();
        }
    }
}
