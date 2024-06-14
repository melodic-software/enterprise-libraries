using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultTryCatchTests
{
    [Fact]
    public void TryCatch_ShouldInvokeFunc_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> tryCatchResult = result.TryCatch(value => Result<int>.Success(value.Length));

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
        tryCatchResult.Value.Should().Be(5);
    }

    [Fact]
    public void TryCatch_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<int> tryCatchResult = result.TryCatch(value => Result<int>.Success(value.Length));

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void TryCatch_ShouldCatchException_WhenThrown()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> tryCatchResult = result.TryCatch<int>(value => throw new InvalidOperationException("Test exception"));

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Errors.Should().ContainSingle().Which.Message.Should().Be("Test exception");
    }

    [Fact]
    public async Task TryCatchAsync_ShouldInvokeFunc_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> tryCatchResult = await result.TryCatchAsync(async value =>
        {
            await Task.Delay(1);
            return Result<int>.Success(value.Length);
        });

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
        tryCatchResult.Value.Should().Be(5);
    }

    [Fact]
    public async Task TryCatchAsync_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<int> tryCatchResult = await result.TryCatchAsync(async value =>
        {
            await Task.Delay(1);
            return Result<int>.Success(value.Length);
        });

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public async Task TryCatchAsync_ShouldCatchException_WhenThrown()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> tryCatchResult = await result.TryCatchAsync<int>(async value =>
        {
            await Task.Delay(1);
            throw new InvalidOperationException("Test exception");
        });

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Errors.Should().ContainSingle().Which.Message.Should().Be("Test exception");
    }
}
