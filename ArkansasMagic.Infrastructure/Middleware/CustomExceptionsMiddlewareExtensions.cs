using Microsoft.AspNetCore.Builder;

namespace ArkansasMagic.Infrastructure.Middleware
{
    public static class CustomExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionsMiddleware>();
        }
    }
}
