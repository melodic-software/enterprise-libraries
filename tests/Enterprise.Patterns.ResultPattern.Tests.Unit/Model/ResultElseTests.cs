using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultElseTests
{
    [Fact]
    public void Else_WithFuncReturningIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        IError error = Error.Validation("Sample error.");
        var failedResult = Result.Failure<string>(error);

        // Act
        Result<string> result = failedResult.Else(errors => errors.First());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Should().Be(error);
    }

    [Fact]
    public void Else_WithFuncReturningIError_DoesNotChangeWhenSuccessful()
    {
        // Arrange
        string expectedValue = "Success";
        var successResult = Result.Success(expectedValue);

        // Act
        Result<string> result = successResult.Else(errors => errors.First());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Else_WithFuncReturningIEnumerableIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        List<IError> errors =
        [
            Error.Validation("Original error"),
            Error.Validation("New error")
        ];

        var failedResult = Result.Failure<string>(errors);

        // Act
        Result<string> result = failedResult.Else(es => new List<IError> { Error.Validation("Processed error") });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Message.Should().Be("Processed error");
    }

    [Fact]
    public void Else_WithSingleIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        IError newError = Error.Validation("Override error");
        var failedResult = Result.Failure<string>(Error.Validation("Original error"));

        // Act
        Result<string> result = failedResult.Else(newError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Override error");
    }

    [Fact]
    public void Else_WithFuncReturningT_ReturnsNewValueWhenFailed()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        Result<string> result = failedResult.Else(errors => "Recovered value");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Recovered value");
    }

    [Fact]
    public void Else_WithValue_ReturnsValueWhenFailed()
    {
        // Arrange
        var failedResult = Result.Failure<string>(Error.Validation("Error"));

        // Act
        Result<string> result = failedResult.Else("Default value");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Default value");
    }

    [Fact]
    public async Task ElseAsync_WithFuncReturningTaskT_ReturnsNewValueWhenFailed()
    {
        // Arrange
        var failedResult = Result.Failure<string>(Error.Validation("Error"));

        // Act
        Result<string> result = await failedResult.ElseAsync(errors => Task.FromResult("Async recovered value"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Async recovered value");
    }

    [Fact]
    public async Task ElseAsync_WithFuncReturningTaskIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        IError newError = Error.Validation("Async error");
        var failedResult = Result.Failure<string>(Error.Validation("Original error"));

        // Act
        Result<string> result = await failedResult.ElseAsync(errors => Task.FromResult(newError));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Async error");
    }

    [Fact]
    public async Task ElseAsync_WithFuncReturningTaskIEnumerableIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        List<IError> errors = [Error.Validation("Original error")];
        var failedResult = Result.Failure<string>(errors);

        // Act
        Result<string> result = await failedResult.ElseAsync(errors => Task.FromResult<IEnumerable<IError>>(new IError[] { Error.Validation("Processed async error") }));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Processed async error");
    }

    [Fact]
    public async Task ElseAsync_WithTaskIError_ReturnsErrorResultWhenFailed()
    {
        // Arrange
        var newErrorTask = Task.FromResult<IError>(Error.Validation("Async task error"));
        var failedResult = Result.Failure<string>(Error.Validation("Original error"));

        // Act
        Result<string> result = await failedResult.ElseAsync(newErrorTask);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Async task error");
    }

    [Fact]
    public async Task ElseAsync_WithTaskT_ReturnsValueWhenFailed()
    {
        // Arrange
        var recoveredValueTask = Task.FromResult("Async task value");
        var failedResult = Result.Failure<string>(Error.Validation("Error"));

        // Act
        Result<string> result = await failedResult.ElseAsync(recoveredValueTask);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Async task value");
    }
}
