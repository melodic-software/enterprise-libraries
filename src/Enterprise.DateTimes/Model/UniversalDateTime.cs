using Enterprise.DateTimes.Extensions;
using Enterprise.DateTimes.Utc;
using static Enterprise.DateTimes.Formatting.DateTimeFormatStrings;

namespace Enterprise.DateTimes.Model;

/// <summary>
/// Represents a date and time in Universal Coordinated Time (UTC).
/// NOTE: UTC is a standard, and GMT is a timezone.
/// </summary>
public class UniversalDateTime
{
    /// <summary>
    /// Gets the DateTimeOffset in UTC.
    /// </summary>
    public DateTimeOffset DateTimeOffset { get; protected init; }

    /// <summary>
    /// Gets the DateTime representation of the DateTimeOffset in UTC.
    /// </summary>
    public DateTime DateTime
    {
        get
        {
            DateTime dateTime = DateTimeOffset.UtcDateTime;
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return dateTime;
        }
    }

    /// <summary>
    /// Gets the DateOnly representation of the DateTimeOffset.
    /// </summary>
    public DateOnly DateOnly => DateTime.DateOnly();

    /// <summary>
    /// Initializes a new instance of the UniversalDateTime class set to the current UTC date and time.
    /// </summary>
    public UniversalDateTime() : this(DateTimeOffset.UtcNow)
    {
    }

    /// <summary>
    /// Initializes a new instance of the UniversalDateTime class set to the current UTC date and time.
    /// </summary>
    public UniversalDateTime(TimeProvider timeProvider) : this(timeProvider.GetUtcNow())
    {

    }

    /// <summary>
    /// Initializes a new instance of the UniversalDateTime class using the specified DateTimeOffset.
    /// </summary>
    /// <param name="dateTimeOffset">The DateTimeOffset to use. Must be in UTC.</param>
    /// <exception cref="ArgumentException">Thrown if the provided DateTimeOffset is not in UTC.</exception>
    public UniversalDateTime(DateTimeOffset dateTimeOffset)
    {
        if (dateTimeOffset.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("DateTimeOffset must be in UTC.", nameof(dateTimeOffset));
        }

        DateTimeOffset = dateTimeOffset;
    }

    public UniversalDateTime(DateTime dateTime, IEnsureUtcService ensureUtcService)
    {
        ArgumentNullException.ThrowIfNull(ensureUtcService);

        // Ensure the date/time is set to UTC.
        DateTime utcDateTime = ensureUtcService.EnsureUtc(dateTime);

        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException($"Expected UTC DateTime, but received DateTime with kind: {utcDateTime.Kind}", nameof(dateTime));
        }

        // Set the DateTimeOffset property.
        DateTimeOffset = new DateTimeOffset(utcDateTime, TimeSpan.Zero);
    }

    public static implicit operator DateTime(UniversalDateTime udt) => udt.DateTime;
    public static implicit operator DateTimeOffset(UniversalDateTime udt) => udt.DateTimeOffset;
    public static implicit operator DateOnly(UniversalDateTime udt) => udt.DateOnly;

    /// <summary>
    /// Returns a string that represents the current object in ISO 8601 format.
    /// </summary>
    /// <returns>A string in ISO 8601 format representing the current UTC date and time.</returns>
    public override string ToString()
    {
        return DateTimeOffset.ToString(Iso8601);
    }
}
