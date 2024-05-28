namespace Enterprise.Reflection.Types;

public static class TypeExtensions
{
    public static bool IsNotExactSameTypeAs(this object? a, object? b)
    {
        return a?.GetType().IsNotExactSameAs(b?.GetType()) ?? false;
    }

    public static bool Implements(this Type type, Type interfaceType)
    {
        return interfaceType.IsInterface && interfaceType.IsAssignableFrom(type);
    }

    public static bool IsDerivedFrom(this Type type, Type baseClass)
    {
        return baseClass.IsClass && type.IsSubclassOf(baseClass);
    }

    public static bool IsExactSameAs(this Type? type, Type? otherType)
    {
        return type == otherType;
    }

    public static bool IsNotExactSameAs(this Type? type, Type? otherType) => !type.IsExactSameAs(otherType);

    public static bool IsSameOrSubclassOf(this Type type, Type otherType)
    {
        return type.IsExactSameAs(otherType) || type.IsSubclassOf(otherType);
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
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateOnly) ||
               type == typeof(TimeOnly) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid) ||
               type == typeof(byte[]) ||
               type == typeof(Uri) ||
               type == typeof(Enum) ||
               type.IsEnum; // Handles enum types dynamically.
    }
}
