namespace Enterprise.Api.Swagger.MinimalApis;

/// <summary>
/// Represents metadata for minimal API endpoints that specifies the endpoint should only be included
/// in Swagger documentation when the application is running in specific environments.
/// </summary>
public class EnvironmentSwaggerFilterMetadata
{
    public string Environment { get; }

    public EnvironmentSwaggerFilterMetadata(string environment)
    {
        Environment = environment;
    }
}