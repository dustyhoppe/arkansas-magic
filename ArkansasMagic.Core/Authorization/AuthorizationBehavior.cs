using MediatR;
using ArkansasMagic.Core.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IIdentityProvider _identityProvider;

        public AuthorizationBehavior(IIdentityProvider identityProvider)
        {
            _identityProvider = identityProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IAuthorized authorized)
            {
                if (authorized.Identity == null || !authorized.Identity.IsAuthenticated)
                {
                    throw new AuthenticationException();
                }
                if (!_identityProvider.HasPolicy(authorized.Identity, authorized.RequiredPolicy))
                {
                    throw new AuthorizationException(authorized.RequiredPolicy);
                }
            }

            return await next();
        }
    }
}
