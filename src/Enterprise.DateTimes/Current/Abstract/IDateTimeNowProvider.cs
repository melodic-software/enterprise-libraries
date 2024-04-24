namespace Enterprise.DateTimes.Current.Abstract;

public interface IDateTimeNowProvider
{
    public DateTime Now { get; }
}