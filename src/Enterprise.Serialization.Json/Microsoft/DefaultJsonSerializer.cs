using System.Text.Json;

namespace Enterprise.Serialization.Json.Microsoft;

public class DefaultJsonSerializer : ISerializeJson
{
    private readonly JsonSerializerOptions _serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }
}