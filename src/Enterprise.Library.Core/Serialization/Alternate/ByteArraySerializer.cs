using System.Buffers;
using System.Text.Json;
using Enterprise.Library.Core.Serialization.Abstract;

namespace Enterprise.Library.Core.Serialization.Alternate;

public class ByteArraySerializer : IByteArraySerializer
{
    public byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }

    public T? Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes);
    }

    public object? Deserialize(byte[] bytes, Type type)
    {
        return JsonSerializer.Deserialize(bytes, type);
    }
}
