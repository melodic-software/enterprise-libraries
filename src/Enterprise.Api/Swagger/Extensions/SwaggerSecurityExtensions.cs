using Enterprise.Api.Security.Constants;
using Enterprise.Api.Security.OAuth.Extensions;
using Enterprise.Api.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Extensions;

public static class SwaggerSecurityExtensions
{
    public static void AddSecurity(this SwaggerGenOptions options, SwaggerConfigOptions swaggerConfigOptions)
    {
        if (!swaggerConfigOptions.OAuthScopes.Any())
            swaggerConfigOptions.OAuthScopes = SecurityConstants.DemoOAuthScopes;

        options.AddSecurityDefinitions(swaggerConfigOptions);
        options.AddSecurityRequirements(swaggerConfigOptions);
    }

    private static void AddSecurityDefinitions(this SwaggerGenOptions options, SwaggerConfigOptions swaggerConfigOptions)
    {
        //options.AddApiKeySecurityDefinition();
        //options.AddBasicAuthenticationSecurityDefinition(swaggerConfigOptions);
        //options.AddBearerSecurityDefinition();
        options.AddOAuth2SecurityDefinition(swaggerConfigOptions);
    }

    private static void AddSecurityRequirements(this SwaggerGenOptions options, SwaggerConfigOptions swaggerConfigOptions)
    {
        //options.AddApiKeySecurityRequirement();
        //options.AddBasicAuthenticationSecurityRequirement(swaggerConfigOptions);
        //options.AddBearerSecurityRequirement();
        options.AddOAuth2SecurityRequirement(swaggerConfigOptions);
    }
}