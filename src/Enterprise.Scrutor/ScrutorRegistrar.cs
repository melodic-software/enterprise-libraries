using System.Reflection;
using Enterprise.DI.Core.Lifetime.Attributes;
using Enterprise.DI.Core.Lifetime.Attributes.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Enterprise.Scrutor;

public static class ScrutorRegistrar
{
    public static void AutoRegisterServices(this IServiceCollection services,
        IReadOnlyCollection<Assembly>? assemblies = null, RegistrationStrategy? registrationStrategy = null)
    {
        // It is generally suggested to use the "throw" strategy to help identify and mitigate any weird bugs with duplicate DI registrations.
        registrationStrategy ??= RegistrationStrategy.Throw;

        services.Scan(selector =>
        {
            IImplementationTypeSelector implementationTypeSelector = assemblies != null && assemblies.Any()
                ? selector.FromAssemblies(assemblies)
                : selector.FromApplicationDependencies();

            RegisterUsingScrutorAttributes(implementationTypeSelector, registrationStrategy);
            RegisterUsingCustomAttributes(implementationTypeSelector, registrationStrategy);
        });
    }

    /// <summary>
    /// This auto wires anything that has the [ServiceDescriptor] attribute.
    /// There is a bit of a code smell to this because the classes have awareness of how they should be injected.
    /// It also requires projects to have a reference to the Scrutor library because the attribute is defined there.
    /// </summary>
    /// <param name="implementationTypeSelector"></param>
    /// <param name="registrationStrategy"></param>
    private static void RegisterUsingScrutorAttributes(IImplementationTypeSelector implementationTypeSelector, RegistrationStrategy registrationStrategy)
    {
        implementationTypeSelector
            .AddClasses()
            .UsingRegistrationStrategy(registrationStrategy)
            .UsingAttributes();
    }

    /// <summary>
    /// These still have a bit of a code smell, but do NOT require a dependency on Scrutor.
    /// </summary>
    /// <param name="implementationTypeSelector"></param>
    /// <param name="registrationStrategy"></param>
    private static void RegisterUsingCustomAttributes(IImplementationTypeSelector implementationTypeSelector, RegistrationStrategy registrationStrategy)
    {
        // TODO: We may need to adjust the properties on the attributes to support some of the things you can do with the Scrutor attribute.
        AddWithAttribute<SingletonServiceAttribute>(implementationTypeSelector, registrationStrategy);
        AddWithAttribute<ScopedServiceAttribute>(implementationTypeSelector, registrationStrategy);
        AddWithAttribute<TransientServiceAttribute>(implementationTypeSelector, registrationStrategy);
    }

    private static void AddWithAttribute<TAttribute>(IImplementationTypeSelector implementationTypeSelector, RegistrationStrategy registrationStrategy) where TAttribute : ServiceRegistrationAttribute
    {
        ILifetimeSelector lifetimeSelector = implementationTypeSelector
            .AddClasses(classes => classes.WithAttribute<TAttribute>())
            .UsingRegistrationStrategy(registrationStrategy)
            .As(SelectWith<TAttribute>());

        Type attributeType = typeof(TAttribute);

        if (attributeType == typeof(SingletonServiceAttribute))
            lifetimeSelector.WithSingletonLifetime();
        else if (attributeType == typeof(ScopedServiceAttribute))
            lifetimeSelector.WithScopedLifetime();
        else if (attributeType == typeof(TransientServiceAttribute))
            lifetimeSelector.WithTransientLifetime();
    }

    private static Func<Type, IEnumerable<Type>> SelectWith<TAttribute>() where TAttribute : ServiceRegistrationAttribute
    {
        return serviceType =>
        {
            TAttribute? attribute = serviceType.GetCustomAttribute<TAttribute>();

            if (attribute == null)
                return Enumerable.Empty<Type>();

            List<Type> serviceTypes = new List<Type>();

            if (attribute.AsMatchingInterface)
            {
                // The concept of a "matching interface" is often application-specific.
                // The current logic (finding an interface that matches the class name prefixed with 'I') is a common convention, but not universal.
                // TODO: Consider providing a way to customize this logic, such as allowing the user to pass in a function that defines how to find the matching interface.

                Type? matchingInterface = serviceType.GetInterface("I" + serviceType.Name);

                if (matchingInterface != null)
                    serviceTypes.Add(matchingInterface);
            }

            if (attribute.AsImplementedInterfaces)
                serviceTypes.AddRange(serviceType.GetInterfaces());

            if (attribute.AsSelf)
                serviceTypes.Add(serviceType);

            return serviceTypes;
        };
    }
}