namespace Enterprise.Middleware.AspNetCore;

// TODO: Move this to API client library.

public class ServiceDescriptionDto
{
    public string? ServiceTypeFullName { get; set; }
    public string ServiceLifetime { get; set; } = null!;
    public string? InstanceTypeName { get; set; }
    public string? ServiceKey { get; set; }
}