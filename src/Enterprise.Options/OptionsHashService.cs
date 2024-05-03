using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Enterprise.Serialization.Json;

namespace Enterprise.Options;

public static class OptionsHashService
{
    public static string ComputeHash(object options, ISerializeJson jsonSerializer)
    {
        ArgumentNullException.ThrowIfNull(jsonSerializer, nameof(jsonSerializer));

        // Recursively gather all properties that can be serialized based on defined criteria.
        Dictionary<string, object> serializable = GetSerializableProperties(options);

        if (!serializable.Any())
            return string.Empty;

        // Serialize the resulting dictionary which includes only serializable properties.
        string serializedData = jsonSerializer.Serialize(serializable);

        // Compute SHA256 hash of the serialized data.
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedData));
        string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();

        return hash;
    }

    private static Dictionary<string, object> GetSerializableProperties(object? obj)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        if (obj == null)
            return result;

        Type type = obj.GetType();

        // Handle simple and direct serializable types directly by wrapping in a dictionary.
        if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime))
        {
            result.Add("Value", obj);
            return result;
        }

        // Handle collections, enumerate each item and apply serialization recursively.
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            IEnumerable enumerable = (IEnumerable)obj;

            Dictionary<string, object> elements = enumerable.Cast<object>()
                             .Select((item, index) => new { Item = GetSerializableProperties(item), Index = index })
                             .Where(x => x.Item.Any())
                             .ToDictionary(x => $"Item{x.Index}", x => (object)x.Item);

            if (elements.Any())
            {
                result.Add("Collection", elements);
            }

            return result;
        }

        // Process properties of complex object types.
        if (type.IsClass && type != typeof(object))
        {
            Dictionary<string, object> properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(prop => prop.CanRead && prop.GetIndexParameters().Length == 0 && IsValidPropertyType(prop.PropertyType))
                                 .Select(prop => new { Prop = prop, Value = prop.GetValue(obj) })
                                 .Select(x => new { x.Prop.Name, Value = GetSerializableProperties(x.Value) })
                                 .Where(x => x.Value.Any())
                                 .ToDictionary(x => x.Name, x => (object)x.Value);

            if (properties.Any())
            {
                foreach (KeyValuePair<string, object> entry in properties)
                {
                    result.Add(entry.Key, entry.Value);
                }
            }
        }

        return result;
    }

    private static bool IsValidPropertyType(Type type)
    {
        // Check for types that are known to be problematic for serialization or do not represent data to serialize.
        return !typeof(Delegate).IsAssignableFrom(type) &&
               !typeof(Assembly).IsAssignableFrom(type) &&
               !typeof(Type).IsAssignableFrom(type) &&
               !typeof(Stream).IsAssignableFrom(type) &&
               !typeof(Task).IsAssignableFrom(type) &&
               type is { IsInterface: false, IsAbstract: false };
    }
}
