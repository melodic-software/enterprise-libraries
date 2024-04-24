using Enterprise.ValueObjects.Model;

namespace Enterprise.ValueObjects.Example;

public class Location : ValueObject
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}