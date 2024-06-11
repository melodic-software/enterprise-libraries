using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.NodaTime.Model;

namespace Enterprise.NodaTime.Current;

public class CurrentDateTimeProvider : ICurrentDateTimeProvider
{
    public DateTime Now => TimeProvider.System.GetLocalNow().Date;
    public DateTime UtcNow => new NodaUniversalDateTime();
}
