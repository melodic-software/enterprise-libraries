using Enterprise.EntityFramework.ValueComparers;
using Enterprise.EntityFramework.ValueConverters;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;
using System.Linq.Expressions;
using System.Text.Json;

namespace Enterprise.EntityFramework.Extensions;

public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Use a value converter for the Color struct.
    /// The string value is persisted in the provider, and is converted back to a struct using the persisted name from the provider.
    /// </summary>
    /// <param name="propertyBuilder"></param>
    /// <returns></returns>
    public static PropertyBuilder<Color> ConvertColor(this PropertyBuilder<Color> propertyBuilder)
    {
        Expression<Func<Color, string>> convertToProviderExpression = c => c.ToString();
        Expression<Func<string, Color>> convertFromProviderExpression = s => Color.FromName(s);
        propertyBuilder = propertyBuilder.HasConversion(convertToProviderExpression, convertFromProviderExpression);
        return propertyBuilder;
    }

    // NOTE: Use this for SQLite since it doesn't support JSON columns yet.
    // Otherwise, call .ToJson() on the owned entity instead.

    public static PropertyBuilder<T> ConvertJson<T>(this PropertyBuilder<T> propertyBuilder)
    {
        JsonSerializerOptions serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

        JsonValueConverter<T> converter = new(serializerOptions);
        JsonValueComparer<T> valueComparer = new(serializerOptions);

        return propertyBuilder.HasConversion(converter, valueComparer);
    }

    public static PropertyBuilder<T> HasListOfIdsConverter<T>(this PropertyBuilder<T> propertyBuilder)
    {
        GuidListConverter converter = new GuidListConverter();
        GuidListComparer comparer = new GuidListComparer();

        return propertyBuilder.HasConversion(converter, comparer);
    }
}