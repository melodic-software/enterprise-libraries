namespace Enterprise.DateTimes.Utc;

public interface IEnsureUtcService
{
    DateTime EnsureUtc(DateTime dateTime);
    DateTimeOffset EnsureUtc(DateTimeOffset dateTimeOffset);
}