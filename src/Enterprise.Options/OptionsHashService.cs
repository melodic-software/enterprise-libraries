using System.Security.Cryptography;
using System.Text;
using Enterprise.Serialization.Json;

namespace Enterprise.Options;

public static class OptionsHashService
{
    public static string ComputeHash(object options, ISerializeJson jsonSerializer)
    {
        Dictionary<string, object?> properties = options.GetType()
            .GetProperties()
            .Where(prop => !typeof(Delegate).IsAssignableFrom(prop.PropertyType))
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(options));

        string serializedData = jsonSerializer.Serialize(properties);
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedData));
        string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();

        return hash;
    }
}
