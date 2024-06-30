using Enterprise.ModularMonoliths.ModuleNaming;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Enterprise.MediatR.Behaviors.Logging;

public class ModularMonolithRequestLoggingBehavior<TRequest, TResult> :
    IPipelineBehavior<TRequest, TResult>
    where TRequest : class
{
    private const string ModulePropertyName = "Module";
    private readonly ILogger<ModularMonolithRequestLoggingBehavior<TRequest, TResult>> _logger;
    private readonly IModuleNameService _moduleNameService;

    public ModularMonolithRequestLoggingBehavior(ILogger<ModularMonolithRequestLoggingBehavior<TRequest, TResult>> logger, IModuleNameService moduleNameService)
    {
        _logger = logger;
        _moduleNameService = moduleNameService;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        Type requestType = request.GetType();

        // If the application is configured as a modular monolith, 
        // the name will be returned based on a specific convention.
        string moduleName = _moduleNameService.GetModuleName(requestType);

        // TODO: Can we just use the native .NET ILogger.BeginScope here instead?
        // What are the differences?
        using IDisposable? moduleLogContext = !string.IsNullOrWhiteSpace(moduleName) ?
            LogContext.PushProperty(ModulePropertyName, moduleName) :
            null;

        TResult result = await next();

        return result;
    }
}
