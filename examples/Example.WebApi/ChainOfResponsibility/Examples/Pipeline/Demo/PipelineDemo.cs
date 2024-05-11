using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Models;

namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Demo;

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
