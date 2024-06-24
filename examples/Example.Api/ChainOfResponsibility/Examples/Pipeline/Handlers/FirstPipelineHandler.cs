﻿using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Example.Api.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.Api.ChainOfResponsibility.Examples.Pipeline.Handlers;

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
