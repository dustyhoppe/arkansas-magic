using ArkansasMagic.Core.Data;
using ArkansasMagic.Domain.Inf;
using ArkansasMagic.Infrastructure.Configuration;
using ArkansasMagic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ArkansasMagic.Infrastructure
{
    public class InfrastructurePackage : IPackage
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            // Database
            var databaseConfiguration = configuration.BindSection<DatabaseConfiguration>("Database");
            var version = new MySqlServerVersion(new Version(8, 0, 25));

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseMySql(databaseConfiguration.ConnectionString, version)
                    .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                    .EnableDetailedErrors(); // <-- with debugging (remove for production)
            });
        }
    }
}
