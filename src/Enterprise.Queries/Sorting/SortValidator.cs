using Enterprise.Mapping.Properties.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Queries.Sorting;

public class SortValidator<TSource, TTarget> : IValidateSort
{
    private readonly IPropertyMappingService _propertyMappingService;

    public SortValidator(IPropertyMappingService propertyMappingService)
    {
        _propertyMappingService = propertyMappingService;
    }

    public Error? Validate(SortOptions sortOptions)
    {
        if (string.IsNullOrWhiteSpace(sortOptions.OrderBy))
        {
            return null;
        }

        // TODO: Should this sort property be whatever the name of the input param is?
        // For example, the name of the sort parameter in a query string that is bound in an API controller method?

        return !_propertyMappingService.MappingExistsFor<TSource, TTarget>(sortOptions.OrderBy) ?
            Error.Validation("Sort.Invalid", "Invalid sort specified.")
            : null;
    }
}
