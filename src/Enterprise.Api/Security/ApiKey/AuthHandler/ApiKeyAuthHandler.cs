using System.Security.Claims;
using System.Text.Encodings.Web;
using Enterprise.Api.Security.ApiKey.Options;
using Enterprise.Api.Security.ApiKey.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.Security.ApiKey.AuthHandler;

public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
    private readonly IValidateApiKey _apiKeyValidator;

    public ApiKeyAuthHandler(
        IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IValidateApiKey apiKeyValidator)
        : base(options, logger, encoder)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!_apiKeyValidator.RequestContainsValidApiKey(Context))
        {
            return Task.FromResult(AuthenticateResult.Fail("API Key is not valid."));
        }

        // TODO: Implement claims transformation.
        // There may not be context of a user here since it's just an API key.

        Claim[] claims =
        [
            new(ClaimTypes.Email, ""),
            new(ClaimTypes.Name, "")
        ];

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, ApiKeyConstants.AuthType);

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        AuthenticationTicket ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

        AuthenticateResult result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
