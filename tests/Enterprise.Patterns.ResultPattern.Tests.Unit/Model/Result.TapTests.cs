using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultTapTests
{
    [Fact]
    public void Tap_ShouldExecuteAction_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool actionExecuted = false;
        void Action() => actionExecuted = true;

        // Act
        Result result = initialResult.Tap(Action);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(initialResult);
    }

    [Fact]
    public void Tap_ShouldNotExecuteAction_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool actionExecuted = false;
        void Action() => actionExecuted = true;

        // Act
        Result result = initialResult.Tap(Action);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(initialResult);
    }

    [Fact]
    public async Task TapAsync_ShouldExecuteActionAsync_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool actionExecuted = false;

        Task ActionAsync()
        {
            actionExecuted = true;
            return Task.CompletedTask;
        }

        // Act
        Result result = await initialResult.TapAsync(ActionAsync);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(initialResult);
    }

    [Fact]
    public async Task TapAsync_ShouldNotExecuteActionAsync_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool actionExecuted = false;

        Task ActionAsync()
        {
            actionExecuted = true;
            return Task.CompletedTask;
        }

        // Act
        Result result = await initialResult.TapAsync(ActionAsync);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(initialResult);
    }
}
