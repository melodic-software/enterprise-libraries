namespace Enterprise.Serialization.Json.Abstract.Composite;

/// <summary>
/// This composite interface provides both serialization and deserialization functionality.
/// If you only need one or the other, consider using <see cref="ISerializeJson"/> or <see cref="IDeserializeJson"/>.
/// </summary>
public interface IJsonSerializer : ISerializeJson, IDeserializeJson;
