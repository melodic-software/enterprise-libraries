using System.Reflection;
using Enterprise.Applications.DI.Startup;
using Enterprise.Reflection.Assemblies.Delegates;
using static Enterprise.Reflection.Assemblies.Delegates.AssemblyNameFilters;

namespace Enterprise.Applications.DI.Registration.Methods;

public class RegistrationMethodConfig
{
    private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static;

    private GetAssemblies DefaultGetAssemblies { get; } = () => AssemblyAutoLoader.LoadAssemblies(ThatAreNotMicrosoft);

    public Type InterfaceType { get; }
    public string MethodName { get; }
    public ParameterInfosAreValid ParameterInfosAreValid { get; }
    public BindingFlags BindingFlags { get; }
    public object[] MethodParameters { get; }
    public GetAssemblies GetAssemblies { get; }
    public GetAssemblyTypeInfos GetAssemblyTypeInfos { get; }

    public RegistrationMethodConfig(Type interfaceType,
        string methodName,
        ParameterInfosAreValid parametersAreValid,
        object[] methodParameters,
        BindingFlags bindingFlags = DefaultBindingFlags,
        GetAssemblies? getAssemblies = null,
        GetAssemblyTypeInfos? getAssemblyTypes = null)
    {
        InterfaceType = interfaceType;
        MethodName = methodName;
        ParameterInfosAreValid = parametersAreValid;

        BindingFlags = bindingFlags;
        MethodParameters = methodParameters;
        GetAssemblies = getAssemblies ?? DefaultGetAssemblies;
        GetAssemblyTypeInfos = getAssemblyTypes ?? GetAssemblyTypeInfosDefaults.GetDefaultAssemblyTypes;
    }
}
