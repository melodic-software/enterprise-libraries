using Enterprise.Api.Swagger.Options;
using Enterprise.Logging.Core.Loggers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.Api.Security.Constants.SecurityConstants;
using static Enterprise.Api.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.Api.Security.OAuth.Extensions;

public static class OAuth2Extensions
{
    /// <summary>
    /// NOTE: use this OR "AddJwtBearer" - not both
    /// https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection
    /// This involves reference tokens, and increases load on the identity server AND the API
    /// because the API has to make a call for every request
    /// </summary>
    /// <param name="authBuilder"></param>
    /// <param name="authority"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="cacheDurationInMinutes"></param>
    public static void AddOAuth2Introspection(this AuthenticationBuilder authBuilder, string authority,
        string? clientId = null, string? clientSecret = null, int cacheDurationInMinutes = 5)
    {
        authBuilder.AddOAuth2Introspection(authenticationScheme: JwtBearerAuthenticationScheme, options =>
        {
            options.Authority = authority;
            options.ClientId = clientId ?? "api-REPLACE-ME";
            options.ClientSecret = clientSecret ?? "259439594-238128-REPLACE-ME";
            // there is built in caching, with a default of 5 minutes, but we can override that
            options.CacheDuration = TimeSpan.FromMinutes(cacheDurationInMinutes);
        });
    }

    public static void AddOAuth2SecurityDefinition(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        if (!swaggerOptions.CanConfigureOAuth)
        {
            PreStartupLogger.Instance.LogInformation(
                "OAuth has not been configured. " +
                "The Swagger OAuth2 security definition will not be defined."
            );

            return;
        }

        string authority = swaggerOptions.Authority;

        DiscoveryDocumentResponse discoveryDocResponse;

        try
        {
            discoveryDocResponse = DiscoveryDocumentService
                .GetDiscoveryDocumentAsync(authority)
                .GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            PreStartupLogger.Instance.LogError(ex, "An error has occurred getting the discovery document.");
            throw;
        }

        DiscoveryDocumentService.ValidateDiscoveryDocumentResponse(discoveryDocResponse);

        options.AddSecurityDefinition(OAuth2SecurityDefinitionName, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = CreateFlows(swaggerOptions, discoveryDocResponse)
        });
    }

    public static void AddOAuth2SecurityRequirement(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        if (!swaggerOptions.CanConfigureOAuth)
        {
            PreStartupLogger.Instance.LogInformation("OAuth has not been configured. The Swagger OAuth2 security requirement will not be added.");

            return;
        }

        Dictionary<string, string> oAuthScopes = swaggerOptions.OAuthScopes;
        string[] scopeNames = [.. oAuthScopes.Keys];

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = OAuth2SecurityDefinitionName
                    }
                },
                scopeNames
            }
        });
    }

    private static OpenApiOAuthFlows CreateFlows(SwaggerOptions swaggerOptions, DiscoveryDocumentResponse discoveryDocResponse)
    {
        Dictionary<string, string> oAuthScopes = swaggerOptions.OAuthScopes;

        string authorizeEndpoint = discoveryDocResponse.AuthorizeEndpoint ?? string.Empty;
        string tokenEndpoint = discoveryDocResponse.TokenEndpoint ?? string.Empty;

        var oAuthFlows = new OpenApiOAuthFlows();

        if (swaggerOptions.EnableAuthorizationCodeFlow)
        {
            oAuthFlows.AuthorizationCode = CreateDefaultFlow(authorizeEndpoint, tokenEndpoint, oAuthScopes);
        }

        if (swaggerOptions.EnableImplicitFlow)
        {
            oAuthFlows.Implicit = CreateDefaultFlow(authorizeEndpoint, tokenEndpoint, oAuthScopes);
        }

        if (swaggerOptions.EnableClientCredentialsFlow)
        {
            oAuthFlows.ClientCredentials = CreateDefaultFlow(authorizeEndpoint, tokenEndpoint, oAuthScopes);
        }

        if (swaggerOptions.EnablePasswordFlow)
        {
            oAuthFlows.Password = CreateDefaultFlow(authorizeEndpoint, tokenEndpoint, oAuthScopes);
        }

        return oAuthFlows;
    }

    private static OpenApiOAuthFlow CreateDefaultFlow(string authorizeEndpoint, string tokenEndpoint, Dictionary<string, string> oAuthScopes)
    {
        return new OpenApiOAuthFlow
        {
            AuthorizationUrl = new Uri(authorizeEndpoint),
            TokenUrl = new Uri(tokenEndpoint),
            Scopes = oAuthScopes,
            // This is typically the same endpoint.
            // TODO: use another property on the discovery document (if applicable), and/or make it configurable.
            RefreshUrl = new Uri(tokenEndpoint),
            Extensions = new Dictionary<string, IOpenApiExtension>()
        };
    }
}
