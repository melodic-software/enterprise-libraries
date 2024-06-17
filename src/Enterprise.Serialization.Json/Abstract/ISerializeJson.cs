namespace Enterprise.Serialization.Json.Abstract;

public interface ISerializeJson
{
    string Serialize<T>(T value);
}

public interface ISerializeJson<in T>
{
    string Serialize(T value);
}
