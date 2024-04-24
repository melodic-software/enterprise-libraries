namespace Enterprise.DateTimes.Current.Abstract;

public interface IDateTimeOffsetUtcNowProvider
{
    public DateTimeOffset UtcNow { get; }
}