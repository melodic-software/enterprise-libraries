using System.Reflection;

namespace Enterprise.Reflection.Types;

public static class AssignableConcreteTypeService
{
    public static List<TypeInfo> GetAssignableConcreteTypes<T>(Assembly assembly)
    {
        Type type = typeof(T);
        List<TypeInfo> types = GetAssignableConcreteTypes(assembly, type);
        return types;
    }

    public static List<TypeInfo> GetAssignableConcreteTypes(Assembly assembly, Type type)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(type);

        IEnumerable<TypeInfo> definedTypes = assembly.DefinedTypes;

        List<TypeInfo> types = definedTypes
            .Where(x =>
                x is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableFrom(x)
            )
            .ToList();

        return types;
    }
}