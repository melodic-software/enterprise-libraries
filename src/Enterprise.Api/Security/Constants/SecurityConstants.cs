﻿using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Enterprise.Api.Security.Constants;

public static class SecurityConstants
{
    // https://datatracker.ietf.org/doc/html/rfc9068
    public const string AccessTokenInJwtFormatType = "at+jwt";
    public const string JwtFormatType = "JWT";
    public const string BasicAuthenticationScheme = "Basic";
    public const string CustomApiKeyHeader = "X-API-Key";
    public const string DefaultAuthenticationScheme = JwtBearerAuthenticationScheme;
    public const string DefaultAuthPolicyName = "DefaultAuthPolicy";
    public const string DefaultJwtNameClaimType = "email"; // TODO: Shouldn't this be JwtClaimTypes.Name?
    public const string JwtBearerAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"

    public const string DemoAuthority = "https://demo.duendesoftware.com";
    public const string DemoAudience = "api";
    public const string DemoOAuthClientId = "interactive.public.short";
    public const string DemoOAuthAppName = "Web API";

    public static readonly Dictionary<string, string> DemoOAuthScopes = new()
    {
        { "api", "Access to the API" },
        { "openid", "OpenID information" },
        { "profile", "User profile information" },
        { "email", "User email address" }
    };

    public static class Swagger
    {
        public const string ApiKeySecurityDefinitionName = "ApiKey";
        public const string ApiKeySecuritySchemeName = "ApiKeyScheme";
        public const string BasicAuthenticationSecurityDefinitionName = "basicAuth";
        public const string BasicAuthenticationSecuritySchemeName = "basic";
        public const string BearerSecurityDefinitionName = "Bearer";
        public const string BearerSecuritySchemeName = "Bearer";
        public const string OAuth2SecurityDefinitionName = "oauth2";
        
        public static string OAuth2RedirectUrl(string baseUrl) => $"{baseUrl}/swagger/oauth2-redirect.html";
    }
}