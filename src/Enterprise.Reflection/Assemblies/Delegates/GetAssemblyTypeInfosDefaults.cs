using System.Reflection;
using static Enterprise.Reflection.Types.AssignableConcreteTypeService;

namespace Enterprise.Reflection.Assemblies.Delegates;

public static class GetAssemblyTypeInfosDefaults
{
    public static List<TypeInfo> GetDefaultAssemblyTypes(Assembly assembly, Type interfaceType)
    {
        var types = GetAssignableConcreteTypes(assembly, interfaceType)
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsAbstract: false, IsGenericTypeDefinition: false })
            .ToList();

        return types;
    }
}
