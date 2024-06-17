using System.Text.Json;
using Enterprise.Serialization.Json.Abstract;

namespace Enterprise.Serialization.Json.Microsoft;

public class DefaultJsonSerializer : ISerializeJson
{
    private readonly JsonSerializerOptions _serializerOptions;

    public DefaultJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _serializerOptions = jsonSerializerOptions;
    }

    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }
}
