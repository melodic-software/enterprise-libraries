using Enterprise.Api.Constants;
using Enterprise.Api.Security.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Enterprise.Api.Security.ApiKey.Validation;

public class DefaultApiKeyValidator : IValidateApiKey
{
    private readonly List<string> _apiKeys;

    public DefaultApiKeyValidator(IConfiguration configuration)
    {
        _apiKeys = [];

        string? apiKey = configuration[ConfigConstants.ApiKeyConfigKeyName];

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            _apiKeys.Add(apiKey);
        }
    }

    public bool RequestContainsValidApiKey(HttpContext httpContext)
    {
        IHeaderDictionary requestHeaders = httpContext.Request.Headers;

        bool keysMatch = ValidateCustomHeader(requestHeaders);

        return keysMatch;
    }

    private bool ValidateCustomHeader(IHeaderDictionary requestHeaders)
    {
        bool apiKeyPresentInHeader = requestHeaders.TryGetValue(SecurityConstants.CustomApiKeyHeader, out StringValues values);
        bool keysMatch = apiKeyPresentInHeader && values.Any(s => _apiKeys.Contains(s, StringComparer.Ordinal));
        return keysMatch;
    }

    public bool ValidateAuthHeader(HttpContext httpContext)
    {
        bool containsAuthHeader = httpContext.Request.Headers.ContainsKey(HeaderNames.Authorization);

        if (!containsAuthHeader)
        {
            return false;
        }

        string authHeader = httpContext.Request.Headers[HeaderNames.Authorization].ToString();

        bool keysMatch = _apiKeys.Contains(authHeader);

        return keysMatch;
    }
}
