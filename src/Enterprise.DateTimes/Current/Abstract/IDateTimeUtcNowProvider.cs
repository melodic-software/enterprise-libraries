namespace Enterprise.DateTimes.Current.Abstract;

public interface IDateTimeUtcNowProvider
{
    public DateTime UtcNow { get; }
}