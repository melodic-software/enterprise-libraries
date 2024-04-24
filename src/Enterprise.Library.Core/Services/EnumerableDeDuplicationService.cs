using Enterprise.Library.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Reflection;

namespace Enterprise.Library.Core.Services;

public static class EnumerableDeDuplicationService
{
    public static void DeduplicateIEnumerables(object value, ILogger logger)
    {
        List<PropertyInfo> properties = value.GetType()
            .GetProperties()
            .Where(prop =>
                typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                prop.PropertyType.IsGenericType &&
                !Attribute.IsDefined(prop, typeof(AllowDuplicatesAttribute))
            ).ToList();

        foreach (PropertyInfo prop in properties)
        {
            if (!prop.CanWrite)
            {
                logger.LogInformation($"Property '{prop.Name}' is read-only and cannot be deduplicated.");
                continue;
            }

            object? currentValue = prop.GetValue(value);
            if (currentValue == null) continue;

            Type itemType = prop.PropertyType.GetGenericArguments()[0];
            var distinctItems = new HashSet<object>(((IEnumerable)currentValue).Cast<object>());

            if (prop.PropertyType.IsAssignableFrom(typeof(List<>).MakeGenericType(itemType)))
            {
                Type listType = typeof(List<>).MakeGenericType(itemType);
                object? newList = Activator.CreateInstance(listType);

                if (newList is IList list)
                {
                    foreach (object item in distinctItems)
                        list.Add(item);
                }

                prop.SetValue(value, newList);
            }

            // Further extension for other collection types can be added here.
        }
    }
}