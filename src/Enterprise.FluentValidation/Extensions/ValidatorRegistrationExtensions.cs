using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.FluentValidation.Extensions;

public static class ValidatorRegistrationExtensions
{
    public static IServiceCollection AddValidatorsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        // Get the assembly containing the specified type.
        Assembly assembly = typeof(T).GetTypeInfo().Assembly;

        Type genericValidatorInterface = typeof(IValidator<>);

        // Find all validator types in the assembly.
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericValidatorInterface))
            .ToList();

        // Register each validator with its implemented interfaces.
        RegisterValidator(services, validatorTypes, genericValidatorInterface);

        return services;
    }

    private static void RegisterValidator(IServiceCollection services, List<Type> validatorTypes, Type genericValidatorInterface)
    {
        foreach (Type validatorType in validatorTypes)
        {
            var implementedInterfaces = validatorType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericValidatorInterface)
                .ToList();

            RegisterImplementedInterfaces(services, implementedInterfaces, validatorType);
        }
    }

    private static void RegisterImplementedInterfaces(IServiceCollection services, IEnumerable<Type> implementedInterfaces, Type validatorType)
    {
        foreach (Type implementedInterface in implementedInterfaces)
        {
            services.TryAddTransient(implementedInterface, validatorType);
        }
    }
}
