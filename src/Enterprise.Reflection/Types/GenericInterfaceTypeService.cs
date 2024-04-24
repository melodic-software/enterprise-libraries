namespace Enterprise.Reflection.Types;

public static class GenericInterfaceTypeService
{
    public static bool ImplementsGenericInterface(Type type, Type genericInterfaceType)
    {
        Type[] interfaceTypes = type.GetInterfaces();
        bool result = interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericInterfaceType);
        return result;
    }
}