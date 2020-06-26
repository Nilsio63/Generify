using Generify.Repositories;
using Generify.Repositories.Extensions.DependencyInjection;
using Generify.Services.Extensions.DependencyInjection;
using Generify.Web;
using Microsoft.AspNetCore.Builder;
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

            services.AddGenerifyServices();

            services.AddRazorPages().AddGenerifyPages();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseGenerifyStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            CreateDataBase(app);
        }

        private void CreateDataBase(IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            GenerifyDataContext context = scope.ServiceProvider.GetRequiredService<GenerifyDataContext>();

            context.Database.EnsureCreated();
        }
    }
}
