using Enterprise.Validation.Model;

namespace Enterprise.Validation.Extensions;

public static class ValidationExtensions
{
    public static Dictionary<string, string[]> ToDictionary(this IEnumerable<ValidationError> validationErrors)
    {
        var group = validationErrors
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                });

        var dictionary = group.ToDictionary(x => x.Key, x => x.Values);

        return dictionary;
    }
}
