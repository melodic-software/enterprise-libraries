using System.Diagnostics;
using Enterprise.ModularMonoliths.ModuleNaming;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Enterprise.MediatR.Behaviors.Logging;

public class RequestLoggingBehavior<TRequest, TResult> :
    IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
{
    private const string ErrorsPropertyName = "Errors";
    private const string ModulePropertyName = "Module";
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResult>> _logger;
    private readonly IModuleNameService _moduleNameService;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResult>> logger, IModuleNameService moduleNameService)
    {
        _logger = logger;
        _moduleNameService = moduleNameService;
    }

    public async Task<TResult> Handle(TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Type requestType = request.GetType();
        string requestTypeName = requestType.Name;

        // If the application is configured as a modular monolith, 
        // the name will be returned based on a specific convention.
        string moduleName = _moduleNameService.GetModuleName(requestType);

        using IDisposable? moduleLogContext = !string.IsNullOrWhiteSpace(moduleName) ? 
            LogContext.PushProperty(ModulePropertyName, moduleName) : 
            null;
        
        using IDisposable loggerScope = _logger.BeginScope("{@Request}", request);

        var stopWatch = Stopwatch.StartNew();

        _logger.LogInformation("Processing request.");
        TResult result = await next();
        HandleResult(result);

        stopWatch.Stop();

        _logger.LogInformation("Request completed in {Milliseconds} ms.", stopWatch.ElapsedMilliseconds);

        return result;
    }

    private void HandleResult(TResult genericResult)
    {
        if (genericResult is not Result result)
        {
            _logger.LogInformation("Request completed.");
            return;
        }

        if (result.IsSuccess)
        {
            _logger.LogInformation("Request completed successfully.");
        }
        else
        {
            var propertyValue = result.Errors
                .Select(x => new { x.Code, x.Message })
                .ToList();

            // https://github.com/serilog/serilog/wiki/Enrichment
            // This is a form of log enrichment that acts similarly to logging scopes.
            // It requires the log context enricher to be enabled in the logger configuration.
            using (LogContext.PushProperty(ErrorsPropertyName, propertyValue, true))
            {
                _logger.LogInformation("Request failed with error(s). {@Errors}", result.Errors);
            }
        }
    }
}
