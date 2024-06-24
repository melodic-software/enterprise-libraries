using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.Api.ChainOfResponsibility.Examples.Pipeline.Handlers;

public class SecondPipelineHandler : IHandler<MyRequest, MyResponse>
{
    private readonly ILogger<SecondPipelineHandler> _logger;

    public SecondPipelineHandler(ILogger<SecondPipelineHandler> logger)
    {
        _logger = logger;
    }

    public async Task<MyResponse?> HandleAsync(MyRequest request, SuccessorDelegate<MyResponse> next, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting second pipeline link.");
        MyResponse result = await next();
        _logger.LogInformation("Exiting second pipeline link.");
        return result;
    }
}
