using Enterprise.DateTimes.Utc;
using NodaTime;

namespace Enterprise.NodaTime.Utc;

/// <summary>
/// Service to ensure that DateTime and DateTimeOffset values are converted to UTC.
/// This service uses Noda Time for reliable date and time conversions.
/// </summary>
public class EnsureUtcService : IEnsureUtcService
{
    /// <summary>
    /// Converts a DateTime to UTC.
    /// If the DateTime is of kind 'Unspecified', it is treated as local time.
    /// If the DateTime is of kind 'Utc', it is returned without modification.
    /// </summary>
    /// <param name="dateTime">The DateTime to convert.</param>
    /// <returns>The DateTime converted to UTC.</returns>
    public DateTime EnsureUtc(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
            return dateTime;

        LocalDateTime localDateTime = LocalDateTime.FromDateTime(dateTime);

        // Assuming the system's local time zone, but this can be more specific
        DateTimeZone zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
        ZonedDateTime zonedDateTime = localDateTime.InZoneLeniently(zone);

        return zonedDateTime.ToDateTimeUtc();
    }

    /// <summary>
    /// Ensures that a DateTimeOffset represents the same instant in UTC.
    /// </summary>
    /// <param name="dateTimeOffset">The DateTimeOffset to convert.</param>
    /// <returns>A DateTimeOffset adjusted to UTC.</returns>
    public DateTimeOffset EnsureUtc(DateTimeOffset dateTimeOffset)
    {
        // Noda Time's Instant handles DateTimeOffset seamlessly.
        Instant instant = Instant.FromDateTimeOffset(dateTimeOffset);

        return instant.ToDateTimeOffset();
    }
}