using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using Enterprise.ValueObjects.Model;

namespace Enterprise.ValueObjects.Example;

internal sealed class LicensePlate : ValueObject
{
    public string Value { get; }

    private LicensePlate(string value)
    {
        Value = value;
    }

    public static Result<LicensePlate> Create(string? value)
    {
        value = value?.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("License plate cannot be null or white space.");
        }

        if (value.Length is < 5 or > 8)
        {
            return Error.Validation("License plate value is out of range.");
        }

        return new LicensePlate(value);
    }

    public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
    public static implicit operator LicensePlate(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

//internal sealed record LicensePlate : IValueObject
//{
//    public string Value { get; }

//    private LicensePlate(string value)
//    {
//        Value = value;
//    }

//    public static Result<LicensePlate> Create(string? value)
//    {
//        value = value?.Trim();

//        if (string.IsNullOrWhiteSpace(value))
//        {
//            return Error.Validation("License plate cannot be null or white space.");
//        }

//        if (value.Length is < 5 or > 8)
//        {
//            return Error.Validation("License plate value is out of range.");
//        }

//        return new LicensePlate(value);
//    }

//    public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
//    public static implicit operator LicensePlate(string value) => new(value);
//}
