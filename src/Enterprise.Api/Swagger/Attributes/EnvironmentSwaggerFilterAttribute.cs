namespace Enterprise.Api.Swagger.Attributes;

/// <summary>
/// Specifies that the controller or action should only be included in Swagger documentation
/// when the application is running in specific environments.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class EnvironmentSwaggerFilterAttribute : Attribute
{
    public string Environment { get; }

    public EnvironmentSwaggerFilterAttribute(string environment)
    {
        Environment = environment;
    }
}