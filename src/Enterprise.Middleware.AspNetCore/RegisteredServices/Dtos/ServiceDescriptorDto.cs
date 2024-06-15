namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;

// TODO: Move this to API client library.

public class ServiceDescriptionDto
{

    public string? ServiceTypeNamespace { get; set; }
    public string? ServiceTypeFullName { get; set; }
    public string ServiceTypeName { get; set; }
    public string ServiceLifetime { get; set; } = null!;
    public string? ImplementationTypeNamespace { get; set; }
    public string? ImplementationTypeFullName { get; set; }
    public string? ImplementationTypeName { get; set; }
    public string? ServiceKey { get; set; }
}
