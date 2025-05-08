using System;

namespace ArkansasMagic.Core.Authorization
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string requiredPolicy)
            : base($"The policy '{requiredPolicy}' is required to perform this action.")
        { }
    }
}
