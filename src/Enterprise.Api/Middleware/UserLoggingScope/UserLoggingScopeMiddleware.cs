using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Middleware.UserLoggingScope;

/// <summary>
/// If a user is authenticated, a logging scope is created to capture user information (username, subject, etc.).
/// </summary>
public class UserLoggingScopeMiddleware : IMiddleware
{
    private readonly ILogger<UserLoggingScopeMiddleware> _logger;

    /// <summary>
    /// If a user is authenticated, a logging scope is created to capture user information (username, subject, etc.).
    /// </summary>
    public UserLoggingScopeMiddleware(ILogger<UserLoggingScopeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        bool userIsAuthenticated = context.User.Identity is { IsAuthenticated: true };

        if (userIsAuthenticated)
        {
            ClaimsPrincipal user = context.User;

            string identityName = user.Identity?.Name ?? "N/A";
            string? subject = user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;

            using (_logger.BeginScope("User: {User}, SubjectId: {Subject}", identityName, subject))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
