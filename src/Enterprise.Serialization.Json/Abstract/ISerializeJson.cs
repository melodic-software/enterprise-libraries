namespace Enterprise.Serialization.Json.Abstract;

/// <summary>
/// Serializes objects into JSON strings.
/// </summary>
public interface ISerializeJson
{
    string Serialize<T>(T value);
}
