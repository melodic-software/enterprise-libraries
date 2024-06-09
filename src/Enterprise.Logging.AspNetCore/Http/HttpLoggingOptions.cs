namespace Enterprise.Logging.AspNetCore.Http;

public class HttpLoggingOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Middleware:HttpLogging";

    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging
    /// HTTP Logging is a middleware that logs information about incoming HTTP requests and HTTP responses.
    /// It can reduce the performance of an app, especially when logging the request and response bodies.
    /// It can potentially log personally identifiable information (PII). Consider the risk and avoid logging sensitive information.
    /// </summary>
    public bool EnableHttpLogging { get; set; }
    public int RequestBodyLogLimit { get; set; } = HttpLoggingConstants.DefaultRequestBodyLogLimit;
    public int ResponseBodyLogLimit { get; set; } = HttpLoggingConstants.DefaultResponseBodyLogLimit;
}
