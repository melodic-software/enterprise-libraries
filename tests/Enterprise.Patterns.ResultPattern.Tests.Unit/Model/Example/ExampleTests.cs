using Enterprise.Patterns.ResultPattern.Model;
using Xunit.Abstractions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Example;

public class ExampleTests : IAsyncLifetime, IDisposable
{
    /// <summary>
    /// The variable name here stands for "system under test".
    /// </summary>
    private readonly Result _sut = Result.Success();
    private readonly ITestOutputHelper _outputHelper;

    public ExampleTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        // TODO: Add setup code here (this is called first).
        //_outputHelper.WriteLine("Test created.");
    }

    public Task InitializeAsync()
    {
        // TODO: Add setup code here (this is called after constructor).
        //_outputHelper.WriteLine("Test initialized.");
        return Task.CompletedTask;
    }

    [Fact(Skip = "This demonstrates how to skip/ignore a test. This will not be executed.")]
    public void SkipExample()
    {
        // The Skip property is available on most attributes including [Theory] and [InlineData].
    }

    public Task DisposeAsync()
    {
        // TODO: Add tear down code here.
        //_outputHelper.WriteLine("Test disposed.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Technically, this isn't needed since we're using IAsyncLifetime
        // If you weren't using that, you could still use IDisposable.
    }
}
