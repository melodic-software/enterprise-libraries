namespace Enterprise.Serialization.Json.Abstract;

/// <summary>
/// Deserializes JSON strings into object instances.
/// </summary>
public interface IDeserializeJson
{
    public T Deserialize<T>(string value);

    public object Deserialize(string value, Type type);
}
