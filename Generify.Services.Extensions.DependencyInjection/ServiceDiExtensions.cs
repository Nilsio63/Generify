using Generify.Services.Interfaces.Management;
using Generify.Services.Management;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Services.Extensions.DependencyInjection
{
    public static class ServiceDiExtensions
    {
        public static IServiceCollection AddGenerifyServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IAuthenticationService, AuthenticationService>();
        }
    }
}
