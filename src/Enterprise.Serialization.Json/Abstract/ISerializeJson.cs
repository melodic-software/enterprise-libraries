namespace Enterprise.Serialization.Json.Abstract;

public interface ISerializeJson
{
    string Serialize<T>(T value);
}
