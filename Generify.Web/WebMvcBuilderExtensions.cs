using Microsoft.Extensions.DependencyInjection;

namespace Generify.Web
{
    public static class WebMvcBuilderExtensions
    {
        public static IMvcBuilder AddGenerifyPages(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(typeof(WebMvcBuilderExtensions).Assembly);

            return mvcBuilder;
        }
    }
}
