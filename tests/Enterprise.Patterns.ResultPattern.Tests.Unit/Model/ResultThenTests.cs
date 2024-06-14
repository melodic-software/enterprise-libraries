using System.Globalization;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultThenTests
{
    [Fact]
    public void Then_ExecutesFunctionOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("hello");

        // Act
        Result<string> result = successResult.Then(value => Result.Success(value.ToUpper(CultureInfo.InvariantCulture)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("HELLO");
    }

    [Fact]
    public void Then_ReturnsError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        Result<string> result = failedResult.Then(value => Result.Success(value.ToUpper(CultureInfo.InvariantCulture)));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(error);
    }

    [Fact]
    public void Then_ExecutesActionOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("hello");
        bool actionExecuted = false;

        // Act
        Result<string> result = successResult.Then(value =>
        {
            actionExecuted = true;
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void Then_ReturnsSameResult_WhenActionExecutes()
    {
        // Arrange
        var successResult = Result.Success("hello");

        // Act
        Result<string> result = successResult.Then(value => { });

        // Assert
        result.Should().BeSameAs(successResult);
    }

    [Fact]
    public async Task ThenAsync_ExecutesFunctionAsyncOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("hello");

        // Act
        Result<string> result = await successResult.ThenAsync(value => Task.FromResult(Result.Success(value.ToUpper(CultureInfo.InvariantCulture))));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("HELLO");
    }

    [Fact]
    public async Task ThenAsync_ExecutesActionAsyncOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("hello");
        bool actionExecuted = false;

        // Act
        Result<string> result = await successResult.ThenAsync(async value =>
        {
            actionExecuted = true;
            await Task.CompletedTask;
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task ThenAsync_ExecutesFunctionReturningValueAsync_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("hello");

        // Act
        Result<string> result = await successResult.ThenAsync(value => Task.FromResult(value.ToUpper(CultureInfo.InvariantCulture)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("HELLO");
    }

    [Fact]
    public async Task ThenAsync_ReturnsError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        Result<string> result = await failedResult.ThenAsync(value => Task.FromResult(Result.Success(value.ToUpper(CultureInfo.InvariantCulture))));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(error);
    }
}
