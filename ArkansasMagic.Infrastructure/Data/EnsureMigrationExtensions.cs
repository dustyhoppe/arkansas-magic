using ArkansasMagic.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ArkansasMagic.Infrastructure.Data
{
    public static class EnsureMigrationExtensions
    {
        public static void EnsureMigration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<IApplicationDbContext>();
            context.Migrate();
        }
    }
}
