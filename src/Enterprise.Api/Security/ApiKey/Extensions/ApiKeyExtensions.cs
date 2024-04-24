using Enterprise.Api.Security.ApiKey.AuthHandler;
using Enterprise.Api.Security.ApiKey.Middleware;
using Enterprise.Api.Security.ApiKey.Options;
using Enterprise.Api.Security.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.Api.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.Api.Security.ApiKey.Extensions;

public static class ApiKeyExtensions
{
    public static void AddApiKeyAuthentication(this AuthenticationBuilder authBuilder)
    {
        authBuilder.AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeyConstants.SchemeName, null);
    }

    public static void UseApiKeyMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ApiKeyMiddleware>();
    }

    public static void AddApiKeySecurityDefinition(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(ApiKeySecurityDefinitionName, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = SecurityConstants.CustomApiKeyHeader,
            Description = "Please ensure an API key is present in the request headers.",
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiKeySecuritySchemeName
        });
    }

    public static void AddApiKeySecurityRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiKeySecurityDefinitionName
                    }
                },
                new List<string>()
            }
        });
    }
}