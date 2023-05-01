using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ArkansasMagic.Contracts;
using ArkansasMagic.Core.Authentication;
using ArkansasMagic.Core.Authorization;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArkansasMagic.Infrastructure.Middleware
{
    public class CustomExceptionsMiddleware
    {
        private readonly ILogger<CustomExceptionsMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CustomExceptionsMiddleware(ILogger<CustomExceptionsMiddleware> logger
            , RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An An unhandled exception has occurred.");

                if (ex is ValidationException)
                {
                    var errors = new List<string>();
                    foreach (var validationFailure in (ex as ValidationException).Errors)
                    {
                        errors.Add(validationFailure.ErrorMessage);
                    }

                    var body = JsonSerializer.Serialize(new ErrorResponse { Errors = errors });

                    context.Response.StatusCode = 422;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(body);

                    return;
                }
                else if (ex is AuthenticationException)
                {
                    var body = JsonSerializer.Serialize(new ErrorResponse { Errors = new List<string> { ex.Message } });

                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(body);
                }
                else if (ex is AuthorizationException)
                {
                    var body = JsonSerializer.Serialize(new ErrorResponse { Errors = new List<string> { ex.Message } });

                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(body);
                }

                throw;
            }
        }
    }
}
