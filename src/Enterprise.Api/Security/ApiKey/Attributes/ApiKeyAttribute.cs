using Enterprise.Api.Security.ApiKey.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Enterprise.Api.Swagger.Services.SwaggerRequestDetectionService;

namespace Enterprise.Api.Security.ApiKey.Attributes;

// Use EITHER the middleware OR this attribute.
// This attribute could be extended to allow for multiple keys (one for each external party).
// All keys could exist in configuration, or a database.

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private readonly IValidateApiKey _apiKeyValidator;

    // NOTE: Since we're injecting into the constructor, this attribute can't be used directly.
    // It must be referenced as a type param in the [TypeFilter] attribute (this allows for DI to work).

    public ApiKeyAttribute(IValidateApiKey apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        HttpContext httpContext = context.HttpContext;

        if (_apiKeyValidator.RequestContainsValidApiKey(httpContext))
            return;

        if (SwaggerPageRequested(httpContext))
            return;

        context.Result = new UnauthorizedResult();
    }
}