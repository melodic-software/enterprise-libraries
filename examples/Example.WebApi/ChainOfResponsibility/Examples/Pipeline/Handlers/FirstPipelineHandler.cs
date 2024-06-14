using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Handlers;

public class FirstPipelineHandler : IHandler<MyRequest, MyResponse>
{
    private readonly ILogger<FirstPipelineHandler> _logger;

    public FirstPipelineHandler(ILogger<FirstPipelineHandler> logger)
    {
        _logger = logger;
    }

    public async Task<MyResponse?> HandleAsync(MyRequest request, SuccessorDelegate<MyResponse> next, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting first pipeline link.");
        MyResponse result = await next();
        _logger.LogInformation("Exiting first pipeline link.");
        return result;
    }
}
