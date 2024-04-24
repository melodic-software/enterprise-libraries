using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.DateTimes.Model;

namespace Enterprise.DateTimes.Current;

public class CurrentDateTimeOffsetProvider : ICurrentDateTimeOffsetProvider
{
    public DateTimeOffset Now => TimeProvider.System.GetUtcNow();
    public DateTimeOffset UtcNow => new UniversalDateTime();
}
