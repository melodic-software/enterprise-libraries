using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.Swagger.Attributes;

/// <summary>
/// Specifies that the controller or action should only be included in Swagger documentation
/// when the application is running in the development environment.
/// </summary>
public sealed class DevOnlySwaggerFilterAttribute : EnvironmentSwaggerFilterAttribute
{
    public DevOnlySwaggerFilterAttribute() : base(Environments.Development)
    {
    }
}