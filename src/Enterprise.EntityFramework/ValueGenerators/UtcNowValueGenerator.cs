using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Enterprise.EntityFramework.ValueGenerators;

public class UtcNowValueGenerator : ValueGenerator<DateTime>
{
    private readonly TimeProvider _timeProvider;
    public UtcNowValueGenerator()
    {
        _timeProvider = TimeProvider.System;
    }

    public UtcNowValueGenerator(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override DateTime Next(EntityEntry entry)
    {
        return TimeProvider.System.GetUtcNow().UtcDateTime;
    }

    public override bool GeneratesTemporaryValues => false;
}
