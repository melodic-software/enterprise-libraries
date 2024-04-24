using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.DateTimes.Model;

namespace Enterprise.DateTimes.Current;

public class CurrentDateTimeProvider : ICurrentDateTimeProvider
{
    public DateTime Now => TimeProvider.System.GetUtcNow().UtcDateTime;
    public DateTime UtcNow => new UniversalDateTime();
}
