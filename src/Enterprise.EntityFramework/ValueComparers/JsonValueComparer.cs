using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Enterprise.EntityFramework.ValueComparers;

/// <summary>
/// Use this for SQLite since it doesn't support JSON columns yet.
/// Otherwise, just call .ToJson() on the owned entity instead.
/// </summary>
/// <typeparam name="T"></typeparam>
public class JsonValueComparer<T> : ValueComparer<T>
{
    public JsonValueComparer(JsonSerializerOptions serializerOptions) : base(
        (l, r) => JsonSerializer.Serialize(l, serializerOptions) == JsonSerializer.Serialize(r, serializerOptions),
        v => object.Equals(v, default(T)) ? 0 : JsonSerializer.Serialize(v, serializerOptions).GetHashCode(),
        v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, serializerOptions), serializerOptions)!)
    {
    }
}