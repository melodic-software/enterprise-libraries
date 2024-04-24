using Enterprise.Api.ErrorHandling.Constants;

namespace Enterprise.Api.ErrorHandling.Options;

public class ErrorHandlingConfigOptions
{
    public const string ConfigSectionKey = "ErrorHandling";

    /// <summary>
    /// This is the friendly message that will be returned when internal server errors occur (500 status code).
    /// </summary>
    public string InternalServerErrorResponseDetailMessage { get; set; } = ErrorConstants.InternalServerErrorMessage;
}