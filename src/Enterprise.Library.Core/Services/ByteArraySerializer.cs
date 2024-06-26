﻿using System.Buffers;
using System.Text.Json;
using Enterprise.Library.Core.Services.Abstract;

namespace Enterprise.Library.Core.Services;

public class ByteArraySerializer : IByteArraySerializer
{
    public byte[] Serialize<T>(T value)
    {
        ArrayBufferWriter<byte> buffer = new ArrayBufferWriter<byte>();

        using Utf8JsonWriter writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        byte[] result = buffer.WrittenSpan.ToArray();

        return result;
    }

    public T? Deserialize<T>(byte[]? bytes)
    {
        if (bytes == null)
            return default;

        return JsonSerializer.Deserialize<T>(bytes);
    }
}