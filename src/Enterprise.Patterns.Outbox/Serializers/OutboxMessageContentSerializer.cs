using Enterprise.Serialization.Json.Abstract;
using Newtonsoft.Json;

namespace Enterprise.Patterns.Outbox.Serializers;

public class OutboxMessageContentSerializer : ISerializeJson
{
    private readonly JsonSerializerSettings _serializerSettings = new()
    {
        // This will serialize the type of the object which allows for polymorphic deserialization.
        TypeNameHandling = TypeNameHandling.All
    };

    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, _serializerSettings);
    }
}
