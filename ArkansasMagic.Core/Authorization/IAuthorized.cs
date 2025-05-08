using System.Security.Principal;

namespace ArkansasMagic.Core.Authorization
{
    public interface IAuthorized
    {
        IIdentity Identity { get; }
        string RequiredPolicy { get; }
    }
}

