using Generify.Controllers.Extensions.DependencyInjection;
using Generify.Repositories;
using Generify.Repositories.Extensions.DependencyInjection;
using Generify.Services.Extensions.DependencyInjection;
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
                string connectionString = Configuration
                    .GetSection("Generify")
                    .GetValue<string>("Connection")
                    .Replace("%APPDATA%", Configuration.GetValue<string>("APPDATA"));

                dbOptions.UseSqlite(connectionString);
            });

            services.AddGenerifyServices();

            services.AddControllers().AddGenerifyControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using IServiceScope scope = app.ApplicationServices.CreateScope();

            GenerifyDataContext context = scope.ServiceProvider.GetRequiredService<GenerifyDataContext>();

            context.Database.EnsureCreated();
        }
    }
}
