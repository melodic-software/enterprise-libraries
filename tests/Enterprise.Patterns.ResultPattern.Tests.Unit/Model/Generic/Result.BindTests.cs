using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultBindTests
{
    [Fact]
    public void Bind_ShouldInvokeBind_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> bindResult = result.Bind(value => Result<int>.Success(value.Length));

        // Assert
        bindResult.IsSuccess.Should().BeTrue();
        bindResult.Value.Should().Be(5);
    }

    [Fact]
    public void Bind_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<int> bindResult = result.Bind(value => Result<int>.Success(value.Length));

        // Assert
        bindResult.IsFailure.Should().BeTrue();
        bindResult.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public async Task BindAsync_ShouldInvokeBind_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<int> bindResult = await result.BindAsync(async value =>
        {
            await Task.Delay(1);
            return Result<int>.Success(value.Length);
        });

        // Assert
        bindResult.IsSuccess.Should().BeTrue();
        bindResult.Value.Should().Be(5);
    }

    [Fact]
    public async Task BindAsync_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<int> bindResult = await result.BindAsync(async value =>
        {
            await Task.Delay(1);
            return Result<int>.Success(value.Length);
        });

        // Assert
        bindResult.IsFailure.Should().BeTrue();
        bindResult.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
