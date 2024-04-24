using Enterprise.Api.Constants;
using Enterprise.Api.Controllers.Behavior;

namespace Enterprise.Api.ErrorHandling.Constants;

public static class ClientErrorDataConstants
{
    public const string BadRequestLink = StatusCodeLinkConstants.BadRequestLink;
    public const string BadRequestTitle = "Bad Request";
    public const string UnprocessableEntityLink = StatusCodeLinkConstants.UnprocessableEntityLink;
    public const string UnprocessableEntityTitle = ValidationProblemDetailsConstants.Title;
    public const string InternalServerErrorLink = StatusCodeLinkConstants.InternalServerErrorLink;
    public const string InternalServerErrorTitle = ApiBehaviorConstants.InternalServerErrorMessage;
    public const string UnauthorizedLink = StatusCodeLinkConstants.UnauthorizedLink;
}