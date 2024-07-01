using System.Text.Json;
using Enterprise.Library.Core.Serialization.Abstract;

namespace Enterprise.Library.Core.Serialization;

public class ByteArraySerializer : IByteArraySerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ByteArraySerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public byte[] Serialize<T>(T value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value, _jsonSerializerOptions);
    }

    public T? Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes, _jsonSerializerOptions);
    }

    public object? Deserialize(byte[] bytes, Type type)
    {
        return JsonSerializer.Deserialize(bytes, type, _jsonSerializerOptions);
    }
}
