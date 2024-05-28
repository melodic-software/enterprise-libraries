using Enterprise.ValueObjects.Model;

namespace Enterprise.ValueObjects.Example;

//internal sealed record Location(double Latitude, double Longitude) : IValueObject;

internal sealed class Location : ValueObject
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
