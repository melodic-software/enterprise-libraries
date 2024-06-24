using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Enterprise.Serilog.MediatR;

public class SerilogLoggingBehaviorServiceDecorator : IRequestLoggingBehaviorService
{
    private readonly IRequestLoggingBehaviorService _decorated;

    private const string Errors = "Errors";

    public SerilogLoggingBehaviorServiceDecorator(IRequestLoggingBehaviorService decorated)
    {
        _decorated = decorated;
    }

    public void LogApplicationServiceError(ILogger logger, IEnumerable<IError> errors, string requestTypeName)
    {
        errors = errors.ToList();

        var propertyValue = errors.Select(x => new { x.Code, x.Message }).ToList();

        // https://github.com/serilog/serilog/wiki/Enrichment
        // This is a form of log enrichment that acts similarly to logging scopes.
        // It requires the log context enricher to be enabled in the logger configuration.
        using (LogContext.PushProperty(Errors, propertyValue, true))
        {
            _decorated.LogApplicationServiceError(logger, errors, requestTypeName);
        }
    }
}
