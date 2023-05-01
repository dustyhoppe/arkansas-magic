using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArkansasMagic.Core.Authorization;
using ArkansasMagic.Core.Validation;
using ArkansasMagic.Domain.Inf;
using ArkansasMagic.Core.Wizards;

namespace ArkansasMagic.Core
{
    public class CorePackage : IPackage
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(GetType().Assembly);
            services.AddMediatR(GetType().Assembly);
            services.AddValidatorsFromAssembly(GetType().Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IWizardsApiClient, WizardsApiClient>();
        }
    }
}
