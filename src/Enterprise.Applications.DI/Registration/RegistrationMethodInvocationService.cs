using Enterprise.DI.Core.Registration;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Enterprise.Applications.DI.Registration;

public static class RegistrationMethodInvocationService
{
    public static void InvokeUsing(RegistrationMethodConfig config)
    {
        Validate(config);

        PreStartupLogger.Instance.LogInformation(
            "Invoking \"{MethodName}\" on \"{InterfaceTypeName}\".",
            config.MethodName, config.InterfaceType.FullName
        );

        HashSet<Assembly> processedAssemblies = [];
        HashSet<Type> processedTypes = [];

        Assembly[] assemblies = config.GetAssemblies()
            .OrderBy(x => x.GetName().FullName)
            .ToArray();

        foreach (Assembly assembly in assemblies)
        {
            if (processedAssemblies.Contains(assembly))
            {
                continue;
            }

            ProcessAssembly(assembly, processedTypes, config);

            processedAssemblies.Add(assembly);
        }
    }

    private static void Validate(RegistrationMethodConfig config)
    {
        MethodInfo? methodInfo = config.InterfaceType.GetMethod(config.MethodName, config.BindingFlags);

        if (methodInfo == null)
        {
            throw new InvalidOperationException($"{config.MethodName} method not found.");
        }

        ParameterInfo[] methodParameters = methodInfo.GetParameters();

        if (!config.ParameterInfosAreValid(methodParameters))
        {
            throw new InvalidOperationException($"{config.MethodName} parameters are invalid.");
        }
    }

    private static void ProcessAssembly(Assembly assembly, HashSet<Type> processedTypes, RegistrationMethodConfig config)
    {
        List<TypeInfo> typeInfos = config.GetAssemblyTypeInfos(assembly, config.InterfaceType);

        foreach (TypeInfo typeInfo in typeInfos)
        {
            ProcessType(processedTypes, config, typeInfo);
        }
    }

    private static void ProcessType(HashSet<Type> processedTypes, RegistrationMethodConfig config, TypeInfo typeInfo)
    {
        if (processedTypes.Contains(typeInfo))
        {
            return;
        }

        if (typeInfo.ExcludeRegistrations())
        {
            PreStartupLogger.Instance.LogWarning("Registrations have been excluded for: {TypeName}.", typeInfo.FullName);
        }
        else
        {
            Invoke(typeInfo, config);
        }   

        processedTypes.Add(typeInfo);
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
