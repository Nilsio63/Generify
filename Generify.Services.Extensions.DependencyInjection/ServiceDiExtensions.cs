using Generify.Services.Abstractions.Management;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Abstractions.Security;
using Generify.Services.Internal;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Management;
using Generify.Services.Playlists;
using Generify.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services, string passwordSalt, Func<IServiceProvider, IExternalAuthSettings> settingsFactory)
        {
            return services
                .AddSettings(passwordSalt, settingsFactory)
                .AddSecurity()
                .AddServices();
        }

        private static IServiceCollection AddSettings(this IServiceCollection services, string passwordSalt, Func<IServiceProvider, IExternalAuthSettings> settingsFactory)
        {
            return services
                .AddTransient(settingsFactory)
                .AddSingleton<ISaltSettings>(new SaltSettings(passwordSalt));
        }

        private static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            return services
                .AddTransient<IHashEncoder, Sha256Encoder>()
                .AddTransient<IPasswordValidator, PasswordValidator>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IExternalAuthService, ExternalAuthService>()
                .AddTransient<IPlaylistGenerator, PlaylistGenerator>()
                .AddTransient<ISpotifyClientFactory, SpotifyClientFactory>()
                .AddTransient<IPlaylistDefinitionService, PlaylistDefinitionService>()
                .AddTransient<IPlaylistOverviewService, PlaylistOverviewService>()
                .AddTransient<IUserService, UserService>();
        }
    }
}
