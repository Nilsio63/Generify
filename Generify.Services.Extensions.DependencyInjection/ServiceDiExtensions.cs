﻿using Generify.Services.Abstractions.Management;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Abstractions.Security;
using Generify.Services.Internal;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Management;
using Generify.Services.Playlists;
using Generify.Services.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services, string passwordSalt)
        {
            return services
                .AddSettings(passwordSalt)
                .AddSecurity()
                .AddServices();
        }

        private static IServiceCollection AddSettings(this IServiceCollection services, string passwordSalt)
        {
            return services
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
                .AddTransient<IPlaylistSyncWorker, PlaylistSyncWorker>()
                .AddTransient<IPlaylistGenerationContextBuilder, PlaylistGenerationContextBuilder>()
                .AddTransient<IPlaylistDefinitionService, PlaylistDefinitionService>()
                .AddTransient<IPlaylistOverviewService, PlaylistOverviewService>()
                .AddTransient<IUserService, UserService>();
        }
    }
}
