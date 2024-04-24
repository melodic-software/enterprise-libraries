namespace Enterprise.Api.Swagger.Constants;

public static class SwaggerConstants
{
    public const string FallbackPath = "/swagger";
    public const string RoutePrefix = "swagger";
    public const string RouteTemplate = RoutePrefix + "/{documentname}/swagger.json";
    public const string DefaultAppName = "Web API";
    public const string DefaultAppDescription = "This is a .NET Web API.";
}