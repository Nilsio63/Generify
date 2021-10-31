using Blazored.LocalStorage;
using Generify.External.Extensions.DependencyInjection;
using Generify.Repositories.Extensions.DependencyInjection;
using Generify.Services;
using Generify.Services.Abstractions.Management;
using Generify.Services.Extensions.DependencyInjection;
using Generify.Services.Management;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Generify
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGenerifyRepos(dbOptions =>
            {
                var conSection = Configuration.GetSection("Generify").GetSection("Connection");

                string accountEndpoint = conSection.GetValue<string>("Endpoint")
                    .Replace("%GENERIFY_DB_ENDPOINT%", Configuration.GetValue<string>("GENERIFY_DB_ENDPOINT"));

                string accountKey = conSection.GetValue<string>("AccountKey")
                    .Replace("%GENERIFY_DB_ACCOUNT_KEY%", Configuration.GetValue<string>("GENERIFY_DB_ACCOUNT_KEY"));

                string dbName = conSection.GetValue<string>("DataBase")
                    .Replace("%GENERIFY_DB_DATABASE%", Configuration.GetValue<string>("GENERIFY_DB_DATABASE"));

                dbOptions.UseCosmos(accountEndpoint, accountKey, dbName);
            });

            string saltString = Configuration
                .GetSection("Generify")
                .GetSection("Encoding")
                .GetValue<string>("Salt")
                .Replace("%GENERIFY_PASSWORD_SALT%", Configuration.GetValue<string>("GENERIFY_PASSWORD_SALT"));

            services.AddGenerifyServices(saltString, s =>
            {
                NavigationManager navManager = s.GetRequiredService<NavigationManager>();

                string hostAddress = navManager.BaseUri.ToString().Replace("https://", "").TrimEnd('/');

                string clientId = Configuration
                    .GetSection("Generify")
                    .GetSection("External")
                    .GetValue<string>("ClientId")
                    .Replace("%GENERIFY_CLIENT_ID%", Configuration.GetValue<string>("GENERIFY_CLIENT_ID"));

                string clientSecret = Configuration
                    .GetSection("Generify")
                    .GetSection("External")
                    .GetValue<string>("ClientSecret")
                    .Replace("%GENERIFY_CLIENT_SECRET%", Configuration.GetValue<string>("GENERIFY_CLIENT_SECRET"));

                return new ExternalAuthSettings(clientId, clientSecret, $"https://{hostAddress}/authCallback");
            });

            services.AddExternalServices();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();
            services.AddHttpContextAccessor();

            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IUserContextAccessor, UserAuthService>();

            services.AddScoped<IGenerifyAuthenticationStateProvider, GenerifyAuthenticationStateProvider>();
            services.AddScoped(s => (AuthenticationStateProvider)s.GetRequiredService<IGenerifyAuthenticationStateProvider>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
