using System.Reflection;
using Enterprise.DI.Registration.Attributes;

namespace Enterprise.DI.Registration.Extensions;

public static class TypeExtensions
{
    public static bool ExcludeRegistrations(this TypeInfo typeInfo)
    {
        return typeInfo.GetCustomAttribute<ExcludeRegistrationsAttribute>() != null;
    }
}
