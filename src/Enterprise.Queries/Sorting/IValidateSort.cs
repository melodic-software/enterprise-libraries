using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Queries.Sorting;

public interface IValidateSort
{
    Error? Validate(SortOptions sortOptions);
}
