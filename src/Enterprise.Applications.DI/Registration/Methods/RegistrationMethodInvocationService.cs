using System.Reflection;
using Enterprise.Applications.DI.Registration.Sorting;
using Enterprise.DI.Core.Registration.Extensions;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;
using static Enterprise.Applications.DI.Registration.Methods.Validation.RegistrationMethodValidationService;

namespace Enterprise.Applications.DI.Registration.Methods;

public static class RegistrationMethodInvocationService
{
    public static void InvokeUsing(RegistrationMethodConfig config)
    {
        Validate(config);

        PreStartupLogger.Instance.LogInformation(
            "Invoking \"{MethodName}\" on \"{InterfaceTypeName}\".",
            config.MethodName, config.InterfaceType.FullName
        );

        Assembly[] assemblies = config.GetAssemblies()
            .OrderBy(x => x.GetName().FullName, new SegmentComparer())
            .ToArray();

        var typeInfos = assemblies
            .SelectMany(a => config.GetAssemblyTypeInfos(a, config.InterfaceType))
            .OrderBy(ti => ti.AssemblyQualifiedName ?? ti.FullName ?? string.Empty, new SegmentComparer())
            .ToHashSet();

        HashSet<Type> processedTypes = [];

        foreach (TypeInfo typeInfo in typeInfos)
        {
            if (processedTypes.Contains(typeInfo))
            {
                continue;
            }

            ProcessTypeInfo(typeInfo, config);

            processedTypes.Add(typeInfo);
        }
    }

    private static void ProcessTypeInfo(TypeInfo typeInfo, RegistrationMethodConfig config)
    {
        if (typeInfo.ExcludeRegistrations())
        {
            PreStartupLogger.Instance.LogWarning(
                "Registrations have been excluded for: {TypeName}.",
                typeInfo.FullName
            );
        }
        else
        {
            Invoke(typeInfo, config);
        }
    }

    private static void Invoke(Type type, RegistrationMethodConfig config)
    {
        try
        {
            PreStartupLogger.Instance.LogInformation("Invoking method on: {TypeName}.", type.FullName);

            MethodInfo? methodInfo = type.GetMethod(config.MethodName, config.BindingFlags);

            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Method '{config.MethodName}' not found in type '{type.Name}'.");
            }

            methodInfo.Invoke(null, config.MethodParameters);
        }
        catch (TargetInvocationException ex)
        {
            throw new InvalidOperationException($"Error invoking method '{config.MethodName}' on type '{type.Name}'.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An unexpected error occurred while processing type '{type.Name}'.", ex);
        }
    }
}
