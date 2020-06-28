using Generify.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Generify.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddGenerifyPages(this IServiceCollection services)
        {
            services.AddRazorPages().AddApplicationPart(typeof(WebServiceCollectionExtensions).Assembly);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Index");
                    options.ExpireTimeSpan = TimeSpan.FromDays(120);
                });

            services.AddTransient<IUserAuthService, UserAuthService>();

            return services.AddHttpContextAccessor()
                .AddTransient<IUserAuthService, UserAuthService>();
        }
    }
}
