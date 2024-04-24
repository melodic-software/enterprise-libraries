using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Enterprise.EntityFramework.ValueConverters;
// NOTE: Use this for SQLite since it doesn't support JSON columns yet.
// Otherwise, call .ToJson() on the owned entity instead.

public class JsonValueConverter<T> : ValueConverter<T, string>
{
    public JsonValueConverter(JsonSerializerOptions serializerOptions, ConverterMappingHints? mappingHints = null)
        : base(
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions)!,
            mappingHints)
    {
    }
}