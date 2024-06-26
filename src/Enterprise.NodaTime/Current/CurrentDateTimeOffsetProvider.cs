﻿using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.NodaTime.Model;

namespace Enterprise.NodaTime.Current;

internal class CurrentDateTimeOffsetProvider : ICurrentDateTimeOffsetProvider
{
    public DateTimeOffset Now => TimeProvider.System.GetUtcNow();
    public DateTimeOffset UtcNow => new NodaUniversalDateTime();
}
