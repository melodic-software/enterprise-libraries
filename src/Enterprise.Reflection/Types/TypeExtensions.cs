namespace Enterprise.Reflection.Types;

public static class TypeExtensions
{
    public static bool Implements(this Type type, Type interfaceType)
    {
        return interfaceType.IsInterface && interfaceType.IsAssignableFrom(type);
    }

    public static bool IsDerivedFrom(this Type type, Type baseClass)
    {
        return baseClass.IsClass && type.IsSubclassOf(baseClass);
    }

    public static bool IsSameOrSubclassOf(this Type type, Type otherType)
    {
        return type == otherType || type.IsSubclassOf(otherType);
    }

    public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType)
    {
        Type[] interfaces = type.GetInterfaces();
        bool result = interfaces.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericInterfaceType);
        return result;
    }

    public static bool IsConcreteClass(this Type type)
    {
        return type is { IsClass: true, IsAbstract: false };
    }

    public static bool HasDefaultConstructor(this Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) != null;
    }

    public static bool IsPrimitiveOrSimpleType(this Type type)
    {
        // TODO: If new types like "DateOnly" get added, this will need to be updated.
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(DateOnly);
    }
}