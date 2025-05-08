using ArkansasMagic.Core.Authorization;
using System.Security.Principal;

namespace ArkansasMagic.Core.Authentication
{
    public interface IIdentityProvider
    {
        int GetUserId(IIdentity identity);
        bool HasPolicy(IIdentity identity, string policy);
        bool HasRole(IIdentity identity, RoleType role);
    }
}
