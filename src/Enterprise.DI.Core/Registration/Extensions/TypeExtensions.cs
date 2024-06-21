using System.Reflection;
using Enterprise.DI.Core.Registration.Attributes;

namespace Enterprise.DI.Core.Registration.Extensions;

public static class TypeExtensions
{
    public static bool ExcludeRegistrations(this TypeInfo typeInfo)
    {
        return typeInfo.GetCustomAttribute<ExcludeRegistrationsAttribute>() != null;
    }
}
