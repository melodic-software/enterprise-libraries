using System.Reflection;

namespace Enterprise.Reflection.Properties;

public static class PropertyInfoExtensions
{
    public static bool IsIndexer(this PropertyInfo propertyInfo)
    {
        return propertyInfo.GetIndexParameters().Length != 0;
    }

    public static bool IsDelegate(this PropertyInfo propertyInfo)
    {
        return typeof(Delegate).IsAssignableFrom(propertyInfo.PropertyType);
    }
}
