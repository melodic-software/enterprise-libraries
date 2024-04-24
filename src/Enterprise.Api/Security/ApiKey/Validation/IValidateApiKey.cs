using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Security.ApiKey.Validation;

public interface IValidateApiKey
{
    public bool RequestContainsValidApiKey(HttpContext httpContext);
}