using Enterprise.Api.ErrorHandling.Domain;
using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Queries.Paging;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Enterprise.Api.DataShaping;

public class DataShapedQueryResult
{
    public IActionResult? FailureActionResult { get; }
    public IResult? FailureResult { get; }
    public IEnumerable<ExpandoObject> DataShapedResult { get; }
    public PaginationMetadata PaginationMetadata { get; }

    private DataShapedQueryResult(IActionResult? failureActionResult, IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        FailureActionResult = failureActionResult;
        DataShapedResult = dataShapedResult;
        PaginationMetadata = paginationMetadata;
    }

    private DataShapedQueryResult(IResult? failureActionResult, IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        FailureResult = failureActionResult;
        DataShapedResult = dataShapedResult;
        PaginationMetadata = paginationMetadata;
    }

    private DataShapedQueryResult(IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        DataShapedResult = dataShapedResult;
        PaginationMetadata = paginationMetadata;
    }

    public static DataShapedQueryResult Failure(IActionResult actionResult)
    {
        IEnumerable<ExpandoObject> dataShaped = new List<ExpandoObject>();
        PaginationMetadata paginationMetadata = PaginationMetadata.Empty();
        DataShapedQueryResult dataShapedQueryResult = new DataShapedQueryResult(actionResult, dataShaped, paginationMetadata);
        return dataShapedQueryResult;
    }

    public static DataShapedQueryResult Failure(IResult result)
    {
        IEnumerable<ExpandoObject> dataShaped = new List<ExpandoObject>();
        PaginationMetadata paginationMetadata = PaginationMetadata.Empty();
        DataShapedQueryResult dataShapedQueryResult = new DataShapedQueryResult(result, dataShaped, paginationMetadata);
        return dataShapedQueryResult;
    }

    public static DataShapedQueryResult Failure(IReadOnlyCollection<IError> errors, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        // TODO: If ErrorResultFactory becomes a non-static class, just replace these params with the factory instance.
        IResult result = ErrorResultFactory.ToResult(errors, httpContext, problemDetailsFactory);
        DataShapedQueryResult dataShapedQueryResult = Failure(result);
        return dataShapedQueryResult;
    }

    public static DataShapedQueryResult Success(IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        return new DataShapedQueryResult(dataShapedResult, paginationMetadata);
    }
}