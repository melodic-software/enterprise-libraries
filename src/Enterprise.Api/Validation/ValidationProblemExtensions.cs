using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.Validation;

public static class ValidationProblemExtensions
{
    /// <summary>
    /// Translates the validation problem details instance to a <see cref="ValidationProblem"/> result.
    /// </summary>
    /// <param name="problemDetails"></param>
    /// <returns></returns>
    public static ValidationProblem ToValidationProblem(this ValidationProblemDetails problemDetails)
    {
        return TypedResults.ValidationProblem
        (
            problemDetails.Errors,
            problemDetails.Detail,
            problemDetails.Instance,
            problemDetails.Title,
            problemDetails.Type,
            problemDetails.Extensions
        );
    }
}