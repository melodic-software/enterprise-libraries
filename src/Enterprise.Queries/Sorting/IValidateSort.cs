using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.Queries.Sorting;

public interface IValidateSort
{
    Error? Validate(SortOptions sortOptions);
}