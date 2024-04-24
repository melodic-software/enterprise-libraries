namespace Enterprise.Api.Constants;

public static class StatusCodeLinkConstants
{
    public const string InternalServerErrorLink = "https://httpstatuses.com/500";
    public const string BadRequestLink = "https://httpstatuses.io/400";
    public const string UnprocessableEntityLink = "https://httpstatuses.io/422";
    public const string UnauthorizedLink = "https://httpstatuses.com/401";

    public static string LinkFor(int statusCode) => $"https://httpstatuses.io/{statusCode}";
}