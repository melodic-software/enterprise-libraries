using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultThenTests
{
    [Fact]
    public void Then_ShouldExecuteOnSuccess_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessExecuted = false;
        void OnSuccess() => onSuccessExecuted = true;

        // Act
        Result result = initialResult.Then(OnSuccess);

        // Assert
        onSuccessExecuted.Should().BeTrue();
        result.Should().Be(initialResult);
    }

    [Fact]
    public void Then_ShouldNotExecuteOnSuccess_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool onSuccessExecuted = false;
        void OnSuccess() => onSuccessExecuted = true;

        // Act
        Result result = initialResult.Then(OnSuccess);

        // Assert
        onSuccessExecuted.Should().BeFalse();
        result.Should().BeOfType<Result>();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task ThenAsync_ShouldExecuteOnSuccessAsync_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessExecuted = false;

        Task OnSuccessAsync()
        {
            onSuccessExecuted = true;
            return Task.CompletedTask;
        }

        // Act
        Result result = await initialResult.ThenAsync(OnSuccessAsync);

        // Assert
        onSuccessExecuted.Should().BeTrue();
        result.Should().Be(initialResult);
    }

    [Fact]
    public async Task ThenAsync_ShouldNotExecuteOnSuccessAsync_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool onSuccessExecuted = false;

        Task OnSuccessAsync()
        {
            onSuccessExecuted = true;
            return Task.CompletedTask;
        }

        // Act
        Result result = await initialResult.ThenAsync(OnSuccessAsync);

        // Assert
        onSuccessExecuted.Should().BeFalse();
        result.Should().BeOfType<Result>();
        result.Errors.Should().BeEquivalentTo(errors);
    }
}
