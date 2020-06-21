using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generify.Web
{
    public static class WebAppBuilderExtensions
    {
        private const string _webRoot = ".wwwroot.";
        private const string _areas = ".Areas.";

        public static IApplicationBuilder UseGenerifyStaticFiles(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            var fileProviders = new List<IFileProvider>
            {
                env.WebRootFileProvider
            };

            Assembly assembly = typeof(WebAppBuilderExtensions).Assembly;

            fileProviders.AddRange(assembly.GetManifestResourceNames()
                .Where(o => o.Contains(_webRoot))
                .Select(resourceName =>
                {
                    if (resourceName.Contains(_areas))
                    {
                        int webRootIndex = resourceName.LastIndexOf(_webRoot);
                        int areasIndex = resourceName.IndexOf(_areas);

                        return resourceName.Substring(areasIndex, webRootIndex - areasIndex + _webRoot.Length)
                            .Trim('.')
                            .Replace('.', '/');
                    }
                    else
                    {
                        return _webRoot.Trim('.');
                    }
                })
                .Distinct()
                .Select(o => new ManifestEmbeddedFileProvider(assembly, o)));

            env.WebRootFileProvider = new CompositeFileProvider(fileProviders);

            return app;
        }
    }
}
