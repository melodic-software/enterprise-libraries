### Pipeline

The Pipeline variation of the Chain of Responsibility pattern emphasizes asynchronous handling and flexible chaining of handlers. 
This approach is especially useful in environments that demand non-blocking operations and complex processing chains, 
such as in web applications or services processing high volumes of requests.

**Key Characteristics**:
- **Asynchronous Handling**: Supports asynchronous operations throughout the chain, ideal for performance-critical applications that handle I/O operations or other latency-sensitive tasks.
- **Flexible and Dynamic Composition**: Handlers can be added or removed dynamically, and the chain can be restructured at runtime without significant overhead.
- **Delegate-based Linking**: Uses delegates to link handlers, providing loose coupling and high flexibility, which allows for modifications in the chain's behavior during runtime.

**Implementation Example**:
This example demonstrates setting up a responsibility chain using asynchronous handlers that modify and pass responses along the pipeline. 
It leverages C# asynchronous programming features to ensure that the chain does not block while processing.

```csharp
using Enterprise.DI.Core.Registration;
using Medley.Api.ChainOfResponsibility.Examples.Pipeline.Models;
using Medley.Api.ChainOfResponsibility.Pipeline.Dependencies;
using Medley.Api.ChainOfResponsibility.Pipeline.Handlers.Generic;

namespace Medley.Api.ChainOfResponsibility.Examples.Pipeline.Dependencies;

internal sealed ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterChainOfResponsibility<MyRequest, MyResponse>()
            .WithSuccessor<RequestLoggingHandler<MyRequest, MyResponse>>()
            .WithSuccessor<RequestExceptionHandler<MyRequest, MyResponse>>()
            .WithSuccessor<FirstPipelineHandler>()
            .WithSuccessor<SecondPipelineHandler>();
    }
}
```

**Implementation Example**:
This example demonstrates a modern implementation of the Chain of Responsibility pattern where handlers are set up to handle documents, with logging integrated into the process.

```csharp
using Medley.Api.ChainOfResponsibility.Examples.Pipeline.Models;
using Medley.Api.ChainOfResponsibility.Pipeline.Chains;

namespace Medley.Api.ChainOfResponsibility.Examples.Pipeline.Demo;

public static class PipelineDemo
{
    public static async Task ExecuteAsync(IServiceProvider services)
    {
        try
        {
            await using AsyncServiceScope scope = services.CreateAsyncScope();

            IResponsibilityChain<MyRequest, MyResponse> responsibilityChain = scope.ServiceProvider
                .GetRequiredService<IResponsibilityChain<MyRequest, MyResponse>>();

            var request = new MyRequest("TEST");

            MyResponse? response = await responsibilityChain.HandleAsync(request, CancellationToken.None);

            if (response != null)
            {
                Console.WriteLine(response.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}

```