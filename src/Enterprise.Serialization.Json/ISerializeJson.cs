namespace Enterprise.Serialization.Json;

public interface ISerializeJson
{
    string Serialize<T>(T value);
}

public interface ISerializeJson<in T>
{
    string Serialize(T value);
}