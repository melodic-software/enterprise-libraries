using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Enterprise.EntityFramework.ValueConverters;

public class DateTimeToChar10Converter : ValueConverter<DateTime, string>
{
    public const string Format = "yyyy-MM-dd";

    public DateTimeToChar10Converter() : base(
        dateTime => dateTime.ToString(Format, CultureInfo.InvariantCulture),
        stringValue => DateTime.ParseExact(stringValue, Format, CultureInfo.InvariantCulture))
    {

    }
}