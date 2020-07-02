using Generify.Services.Abstractions.Management;
using Generify.Services.Abstractions.Security;
using Generify.Services.Management;
using Generify.Services.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services, string passwordSalt = null)
        {
            return services
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IUserService, UserService>()
                .AddSingleton<ISaltSettings>(new SaltSettings(passwordSalt))
                .AddTransient<IHashEncoder, Sha256Encoder>()
                .AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
