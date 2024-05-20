using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Reflection.Collections;

public static class CollectionClearer
{
    private const string MethodName = "Clear";

    /// <summary>
    /// This checks if the options has any IEnumerable properties.
    /// It first looks for a "Clear" method, and alternatively attempts to re-instantiate the collection using its default constructor.
    /// This assumes collections have a parameterless constructor, which night b
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="logger"></param>
    public static void ClearCollections(object instance, ILogger? logger = null)
    {
        Type optionsType = instance.GetType();

        IEnumerable<PropertyInfo> properties = instance.GetType()
            .GetProperties()
            .Where(p =>
                typeof(IEnumerable).IsAssignableFrom(p.PropertyType) &&
                p.PropertyType != typeof(string)
            ).ToList();

        foreach (PropertyInfo property in properties)
        {
            object? propertyValue = property.GetValue(instance);

            if (propertyValue == null)
            {
                continue;
            }

            Type propertyValueType = propertyValue.GetType();

            MethodInfo? clearMethod = propertyValueType.GetMethod(MethodName);

            if (clearMethod != null)
            {
                // If there is a Clear method, invoke it to clear the collection.
                clearMethod.Invoke(propertyValue, null);
            }
            else
            {
                // Check if there is a parameterless constructor before attempting to create a new instance.
                ConstructorInfo? constructor = propertyValueType.GetConstructor(Type.EmptyTypes);

                if (constructor != null)
                {
                    object newCollection = constructor.Invoke(null);
                    property.SetValue(instance, newCollection);
                }
                else
                {
                    logger?.LogWarning(
                        "Property \"{PropertyName}\" on type \"{OptionsTypeName}\" is a collection without a \"{MethodName}\" method " +
                        "or parameterless constructor and cannot be automatically cleared.",
                        property.Name, optionsType.Name, MethodName
                    );
                }
            }
        }
    }
}
