using System.Text.Json;
using Enterprise.Serialization.Json.Abstract.Composite;

namespace Enterprise.Serialization.Json.Microsoft;

public class DefaultJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _serializerOptions;
    

    public DefaultJsonSerializer() : this(JsonSerializerOptionsService.GetDefaultOptions())
    {
    }

    public DefaultJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _serializerOptions = jsonSerializerOptions;
    }

    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }

    public T Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, _serializerOptions);
    }

    public object Deserialize(string value, Type type)
    {
        return JsonSerializer.Deserialize(value, type, _serializerOptions);
    }
}
