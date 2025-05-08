using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArkansasMagic.Domain.Clock;
using ArkansasMagic.Domain.Inf;

namespace ArkansasMagic.Domain
{
    public class DomainPackage : IPackage
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISystemClock, SystemClock>();
        }
    }
}
