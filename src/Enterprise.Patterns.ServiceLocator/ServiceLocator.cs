using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Patterns.ServiceLocator;

/// <summary>
/// Implements the Service Locator design pattern as a means to decouple application components.
/// However, using the Service Locator pattern is generally considered an anti-pattern because it hides dependencies,
/// making code harder to test and maintain. Prefer dependency injection where practical.
/// For more information on this pattern, see https://en.wikipedia.org/wiki/Service_locator_pattern.
/// </summary>
public class ServiceLocator
{
    private static readonly Lazy<ServiceLocator> _instance = new(() => new ServiceLocator());

    private readonly Dictionary<Type, Func<object?>> _services = new();
    private readonly object _lock = new();

    private IServiceProvider? _serviceProvider;

    /// <summary>
    /// Provides access to the singleton instance of the ServiceLocator.
    /// </summary>
    public static ServiceLocator Instance => _instance.Value;

    /// <summary>
    /// Private constructor to prevent instantiation outside the class. Initializes internal structures.
    /// </summary>
    private ServiceLocator()
    {

    }

    /// <summary>
    /// Registers a service factory for a given type. The service will be lazily created by the provided function.
    /// </summary>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="resolver">A function that returns an instance of the service when called.</param>
    public void Register<T>(Func<T?> resolver)
    {
        lock (_lock)
        {
            _services[typeof(T)] = () => resolver();
        }
    }

    /// <summary>
    /// Sets an external service provider to be used by the Service Locator.
    /// If set, the locator will use this provider to resolve services.
    /// </summary>
    /// <param name="externalProvider">The external IServiceProvider to use.</param>
    public void SetExternalProvider(IServiceProvider externalProvider)
    {
        lock (_lock)
        {
            _serviceProvider = externalProvider;
        }
    }

    /// <summary>
    /// Retrieves a service of type T from the Service Locator.
    /// Uses the external provider if available, otherwise uses the registered services.
    /// Throws InvalidOperationException if the service type is not registered and no external provider is set.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>An instance of type T or null if the service cannot be instantiated.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T? GetService<T>()
    {
        lock (_lock)
        {
            if (_serviceProvider != null)
            {
                return _serviceProvider.GetService<T>();
            }

            if (_services.TryGetValue(typeof(T), out Func<object?>? resolver))
            {
                object? service = resolver();

                if (service != null)
                {
                    return (T)service;
                }
            }
            else
            {
                throw new InvalidOperationException($"No service registered for type {typeof(T).FullName}.");
            }
        }

        return default;
    }

    /// <summary>
    /// Clears all registered services from the Service Locator.
    /// Does not affect an external service provider if one is set.
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _services.Clear();
        }
    }
}
