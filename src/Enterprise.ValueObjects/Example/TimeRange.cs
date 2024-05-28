using Enterprise.ValueObjects.Model;

namespace Enterprise.ValueObjects.Example;

internal sealed class TimeRange : ValueObject
{
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
        {
            throw new ArgumentException("End time must be greater than the start time.");
        }

        Start = start;
        End = end;
    }

    public static TimeRange? FromDateTimes(DateTime start, DateTime end)
    {
        if (start.Date != end.Date)
        {
            //return Error.Validation("Start and end date times must be on the same day.");
            return null;
        }

        if (start >= end)
        {
            //return Error.Validation("End time must be greater than the start time");
            return null;
        }

        var startTime = TimeOnly.FromDateTime(start);
        var endTime = TimeOnly.FromDateTime(end);

        return new TimeRange(startTime, endTime);
    }

    public bool OverlapsWith(TimeRange other)
    {
        if (Start >= other.End)
        {
            return false;
        }

        if (other.Start >= End)
        {
            return false;
        }

        return true;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}

//internal sealed record TimeRange : IValueObject
//{
//    public TimeOnly Start { get; init; }
//    public TimeOnly End { get; init; }

//    public TimeRange(TimeOnly start, TimeOnly end)
//    {
//        if (start >= end)
//        {
//            throw new ArgumentException("End time must be greater than the start time.");
//        }

//        Start = start;
//        End = end;
//    }

//    public static TimeRange? FromDateTimes(DateTime start, DateTime end)
//    {
//        if (start.Date != end.Date)
//        {
//            //return Error.Validation("Start and end date times must be on the same day.");
//            return null;
//        }

//        if (start >= end)
//        {
//            //return Error.Validation("End time must be greater than the start time");
//            return null;
//        }

//        var startTime = TimeOnly.FromDateTime(start);
//        var endTime = TimeOnly.FromDateTime(end);

//        return new TimeRange(startTime, endTime);
//    }

//    public bool OverlapsWith(TimeRange other)
//    {
//        if (Start >= other.End)
//        {
//            return false;
//        }

//        if (other.Start >= End)
//        {
//            return false;
//        }

//        return true;
//    }
//}
