using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Context;

/// <summary>
/// Provides a context for fluent service registration, allowing for easy addition of services and decorators.
/// </summary>
/// <typeparam name="TService">The type of service to be registered.</typeparam>
public partial class RegistrationContext<TService> where TService : class
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Provides a context for fluent service registration, allowing for easy addition of services and decorators of a specific type.
    /// </summary>
    public RegistrationContext(IServiceCollection services)
    {
        _services = services;
    }
}
