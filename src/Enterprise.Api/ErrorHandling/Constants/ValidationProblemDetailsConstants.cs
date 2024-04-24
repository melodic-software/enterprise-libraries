using Enterprise.Api.Constants;

namespace Enterprise.Api.ErrorHandling.Constants;

public static class ValidationProblemDetailsConstants
{
    public const string Detail = "See the errors field for details.";
    public const string Link = StatusCodeLinkConstants.UnprocessableEntityLink;
    public const string Title = "Unprocessable Entity";
}