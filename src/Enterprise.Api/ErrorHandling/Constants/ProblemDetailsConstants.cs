using Enterprise.Api.Constants;

namespace Enterprise.Api.ErrorHandling.Constants;

public static class ProblemDetailsConstants
{
    public const string Type = StatusCodeLinkConstants.InternalServerErrorLink;
    public const string Title = "Server Error";
    public const string Detail = "An unexpected error has occurred.";

    public const string ErrorsExtensionKey = "errors";
    public const string TraceIdExtensionKey = "traceId";
}