using Generify.Controllers.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Generify.Controllers.Extensions.DependencyInjection
{
    public static class ControllerDiExtensions
    {
        public static IMvcBuilder AddGenerifyControllers(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder.AddApplicationPart(typeof(AuthenticationController).Assembly);
        }
    }
}
