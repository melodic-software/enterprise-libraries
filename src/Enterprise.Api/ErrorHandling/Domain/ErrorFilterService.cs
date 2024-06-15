using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.ErrorHandling.Domain;

public static class ErrorFilterService
{
    public static IReadOnlyCollection<IError> FilterBy(int statusCode, IEnumerable<IError> errors)
    {
        var errorList = errors.ToList();

        IReadOnlyCollection<IError> result = statusCode switch
        {
            StatusCodes.Status403Forbidden => errorList
                .Where(x => x.Descriptors.Contains(ErrorDescriptor.Permission))
                .ToList(),
            StatusCodes.Status404NotFound => errorList
                .Where(x => x.Descriptors.Contains(ErrorDescriptor.NotFound))
                .ToList(),
            StatusCodes.Status409Conflict => errorList
                .Where(x => x.Descriptors.Contains(ErrorDescriptor.Conflict))
                .ToList(),
            StatusCodes.Status422UnprocessableEntity => errorList
                .Where(x =>
                    x.Descriptors.Contains(ErrorDescriptor.Validation) ||
                    x.Descriptors.Contains(ErrorDescriptor.BusinessRule)
                )
                .ToList(),
            _ => errorList
        };

        if (!result.Any())
        {
            result = errorList;
        }

        return result;
    }
}
