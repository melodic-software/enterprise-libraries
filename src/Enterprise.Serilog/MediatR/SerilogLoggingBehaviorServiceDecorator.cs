using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Errors;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Enterprise.Serilog.MediatR;

public class SerilogLoggingBehaviorServiceDecorator : ILoggingBehaviorService
{
    private readonly ILoggingBehaviorService _decorated;

    private const string Errors = "Errors";

    public SerilogLoggingBehaviorServiceDecorator(ILoggingBehaviorService decorated)
    {
        _decorated = decorated;
    }

    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string typeName)
    {
        errors = errors.ToList();

        var propertyValue = errors.Select(x => new { x.Code, x.Message }).ToList();

        // https://github.com/serilog/serilog/wiki/Enrichment
        // This is a form of log enrichment that acts similarly to logging scopes.
        // It requires the log context enricher to be enabled in the logger configuration.
        using (LogContext.PushProperty(Errors, propertyValue, true))
        {
            _decorated.LogApplicationServiceError(logger, errors, typeName);
        }
    }
}
