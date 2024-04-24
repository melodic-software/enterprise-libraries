using System.Reflection;

namespace Enterprise.DI.Core.Registration;

public static class TypeExtensions
{
    public static bool ExcludeRegistrations(this TypeInfo typeInfo)
    {
        return typeInfo.GetCustomAttribute<ExcludeRegistrationsAttribute>() != null;
    }
}