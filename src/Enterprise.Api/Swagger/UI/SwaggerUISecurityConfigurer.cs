using Enterprise.Api.Swagger.Options;
using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Swagger.UI;

public static class SwaggerUISecurityConfigurer
{
    public static void ConfigureSecurity(SwaggerSecurityOptions securityOptions, Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions options)
    {
        if (!securityOptions.CanConfigureOAuth)
        {
            return;
        }

        options.OAuthClientId(securityOptions.OAuthClientId);

        if (!string.IsNullOrWhiteSpace(securityOptions.OAuthClientSecret))
        {
            options.OAuthClientSecret(securityOptions.OAuthClientSecret);
        }

        options.OAuthAppName(securityOptions.OAuthAppName);

        if (securityOptions.UsePkce)
        {
            options.OAuthUsePkce();
        }

        var queryStringParams = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(securityOptions.OAuthAudience))
        {
            queryStringParams.Add("audience", securityOptions.OAuthAudience);
        }

        if (queryStringParams.Any())
        {
            options.OAuthAdditionalQueryStringParams(queryStringParams);
        }
    }
}
