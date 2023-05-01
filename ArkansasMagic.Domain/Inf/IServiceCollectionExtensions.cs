using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ArkansasMagic.Domain.Inf
{
    public static class IServiceCollectionExtensions
    {
        public static void RegisterPackage<TPackage>(this IServiceCollection services, IConfiguration configuration)
            where TPackage : IPackage
        {
            IPackage package = Activator.CreateInstance<TPackage>();
            package.Register(services, configuration);
        }
    }
}
