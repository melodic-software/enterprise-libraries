using Enterprise.Api.Security.Constants;
using Enterprise.Api.Security.OAuth.Extensions;
using Enterprise.Api.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Extensions;

public static class SwaggerSecurityExtensions
{
    public static void AddSecurity(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        if (!swaggerOptions.OAuthScopes.Any())
        {
            swaggerOptions.OAuthScopes = SecurityConstants.DemoOAuthScopes;
        }

        options.AddSecurityDefinitions(swaggerOptions);
        options.AddSecurityRequirements(swaggerOptions);
    }

    private static void AddSecurityDefinitions(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        //options.AddApiKeySecurityDefinition();
        //options.AddBasicAuthenticationSecurityDefinition(swaggerOptions);
        //options.AddBearerSecurityDefinition();
        options.AddOAuth2SecurityDefinition(swaggerOptions);
    }

    private static void AddSecurityRequirements(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        //options.AddApiKeySecurityRequirement();
        //options.AddBasicAuthenticationSecurityRequirement(swaggerOptions);
        //options.AddBearerSecurityRequirement();
        options.AddOAuth2SecurityRequirement(swaggerOptions);
    }
}
