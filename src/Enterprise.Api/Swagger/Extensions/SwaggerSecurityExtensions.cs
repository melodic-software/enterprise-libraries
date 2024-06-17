using Enterprise.Api.Security.Constants;
using Enterprise.Api.Security.OAuth.Extensions;
using Enterprise.Api.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Extensions;

public static class SwaggerSecurityExtensions
{
    public static void AddSecurity(this SwaggerGenOptions options, SwaggerSecurityOptions securityOptions)
    {
        if (!securityOptions.OAuthScopes.Any())
        {
            securityOptions.OAuthScopes = SecurityConstants.DemoOAuthScopes;
        }

        options.AddSecurityDefinitions(securityOptions);
        options.AddSecurityRequirements(securityOptions);
    }

    private static void AddSecurityDefinitions(this SwaggerGenOptions options, SwaggerSecurityOptions securityOptions)
    {
        //options.AddApiKeySecurityDefinition();
        //options.AddBasicAuthenticationSecurityDefinition();
        //options.AddJwtBearerSecurityDefinition();
        options.AddOAuth2SecurityDefinition(securityOptions);
    }

    private static void AddSecurityRequirements(this SwaggerGenOptions options, SwaggerSecurityOptions securityOptions)
    {
        //options.AddApiKeySecurityRequirement();
        //options.AddBasicAuthenticationSecurityRequirement();
        //options.AddJwtBearerSecurityRequirement();
        options.AddOAuth2SecurityRequirement(securityOptions);
    }
}
