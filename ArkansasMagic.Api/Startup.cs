using ArkansasMagic.Api.Configuration;
using ArkansasMagic.Api.Services;
using ArkansasMagic.Core;
using ArkansasMagic.Domain;
using ArkansasMagic.Domain.Inf;
using ArkansasMagic.Infrastructure;
using ArkansasMagic.Infrastructure.Data;
using ArkansasMagic.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArkansasMagic.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocument(config =>
            {
                config.Title = "Arkansas Magic API";
                config.Version = "v1";
            });

            services.RegisterPackage<DomainPackage>(Configuration);
            services.RegisterPackage<CorePackage>(Configuration);
            services.RegisterPackage<InfrastructurePackage>(Configuration);

            services.AddHostedService<EventFeedService>();
            services.AddHostedService<OrganizationService>();

            services.Configure<EventFiltersOptions>(Configuration.GetSection(EventFiltersOptions.Key));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureMigration();

            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
            });

            app.UseStaticFiles();
            app.UseCustomExceptions();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseReDoc();
        }
    }
}
