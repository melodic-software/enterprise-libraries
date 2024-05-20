using Enterprise.Applications.DI.Startup;
using Enterprise.Reflection.Assemblies;
using System.Reflection;
using static Enterprise.Reflection.Types.AssignableConcreteTypeService;

namespace Enterprise.Applications.DI.Registration;

public class RegistrationMethodConfig
{
    private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static;

    private Func<Assembly[]> DefaultGetAssemblies { get; } = () => AssemblyAutoLoader.LoadAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);

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
        GetAssemblies = getAssemblies ?? DefaultGetAssemblies;
        GetAssemblyTypes = getAssemblyTypes ?? GetDefaultAssemblyTypes;
    }

    public List<TypeInfo> GetDefaultAssemblyTypes(Assembly assembly, Type interfaceType)
    {
        var types = GetAssignableConcreteTypes(assembly, interfaceType)
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsAbstract: false, IsGenericTypeDefinition: false })
            .ToList();

        return types;
    }
}