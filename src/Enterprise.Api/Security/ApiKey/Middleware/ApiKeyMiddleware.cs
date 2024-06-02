using Enterprise.Api.Security.ApiKey.Validation;
using Microsoft.AspNetCore.Http;
using static Enterprise.Api.Swagger.Services.SwaggerRequestDetectionService;

// Use EITHER the attribute OR this middleware.
// This middleware could be extended to allow for multiple keys (one for each external party).
// All keys could exist in configuration, or in a database.

namespace Enterprise.Api.Security.ApiKey.Middleware;

public class ApiKeyMiddleware : IMiddleware
{
    private readonly IValidateApiKey _apiKeyValidator;

    public ApiKeyMiddleware(IValidateApiKey apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_apiKeyValidator.RequestContainsValidApiKey(context))
        {
            await next(context);
            return;
        }

        if (SwaggerPageRequested(context))
        {
            await next(context);
            return;
        }

        // The pipeline is terminated here.
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Invalid API Key");
    }
}
