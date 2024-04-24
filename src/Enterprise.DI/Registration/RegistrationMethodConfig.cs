using System.Reflection;
using Enterprise.Reflection.Assemblies;
using static Enterprise.Reflection.Types.AssignableConcreteTypeService;

namespace Enterprise.DI.Registration;

public class RegistrationMethodConfig
{
    private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static;

    public Type InterfaceType { get; }
    public string MethodName { get; }
    public Func<ParameterInfo[], bool> ParametersAreValid { get; }
    public BindingFlags BindingFlags { get; }
    public object[] MethodParameters { get; }
    public Func<Assembly[]> GetAssemblies { get; }
    public Func<Assembly, Type, List<TypeInfo>> GetAssemblyTypes { get; }

    public RegistrationMethodConfig(Type interfaceType,
        string methodName,
        Func<ParameterInfo[], bool> parametersAreValid,
        object[] methodParameters)
    {
        InterfaceType = interfaceType;
        MethodName = methodName;
        ParametersAreValid = parametersAreValid;
        BindingFlags = DefaultBindingFlags;
        MethodParameters = methodParameters;
        GetAssemblies = GetDefaultAssemblies;
        GetAssemblyTypes = GetDefaultAssemblyTypes;
    }

    public RegistrationMethodConfig(Type interfaceType,
        string methodName,
        Func<ParameterInfo[], bool> parametersAreValid,
        object[] methodParameters,
        BindingFlags bindingFlags = DefaultBindingFlags,
        Func<Assembly[]>? getAssemblies = null,
        Func<Assembly, Type, List<TypeInfo>>? getAssemblyTypes = null)
    {
        InterfaceType = interfaceType;
        MethodName = methodName;
        ParametersAreValid = parametersAreValid;
        
        BindingFlags = bindingFlags;
        MethodParameters = methodParameters;
        GetAssemblies = getAssemblies ?? GetDefaultAssemblies;
        GetAssemblyTypes = getAssemblyTypes ?? GetDefaultAssemblyTypes;
    }

    public Assembly[] GetDefaultAssemblies()
    {
        return AssemblyLoader.LoadSolutionAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);
    }

    public List<TypeInfo> GetDefaultAssemblyTypes(Assembly assembly, Type interfaceType)
    {
        List<TypeInfo> types = GetAssignableConcreteTypes(assembly, interfaceType)
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsAbstract: false, IsGenericTypeDefinition: false })
            .ToList();

        return types;
    }
}