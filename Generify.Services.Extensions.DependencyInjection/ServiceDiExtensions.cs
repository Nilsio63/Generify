using Generify.Services.Interfaces.Management;
using Generify.Services.Interfaces.Security;
using Generify.Services.Management;
using Generify.Services.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IHashEncoder, Sha256Encoder>()
                .AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
