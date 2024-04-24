namespace Enterprise.DateTimes.Current.Abstract;

public interface IDateTimeOffsetNowProvider
{
    public DateTimeOffset Now { get; }
}