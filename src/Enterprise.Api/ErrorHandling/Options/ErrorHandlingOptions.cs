using Enterprise.Api.ErrorHandling.Constants;

namespace Enterprise.Api.ErrorHandling.Options;

public class ErrorHandlingOptions
{
    public const string ConfigSectionKey = "ErrorHandling";

    /// <summary>
    /// Determines if the Hellang middleware will be used or if standard exception handling middleware will be used.
    /// </summary>
    public bool UseHellangMiddleware { get; set; } = true;

    /// <summary>
    /// This is the friendly message that will be returned when internal server errors occur (500 status code).
    /// </summary>
    public string InternalServerErrorResponseDetailMessage { get; set; } = ErrorConstants.InternalServerErrorMessage;
}
