namespace Enterprise.Library.Core.Serialization.Abstract;

public interface IByteArraySerializer
{
    byte[] Serialize<T>(T value);
    T? Deserialize<T>(byte[] bytes);
    object? Deserialize(byte[] bytes, Type type);
}
