namespace Enterprise.Library.Services.Abstract;

public interface IByteArraySerializer
{
    byte[] Serialize<T>(T value);
    T? Deserialize<T>(byte[]? bytes);
}