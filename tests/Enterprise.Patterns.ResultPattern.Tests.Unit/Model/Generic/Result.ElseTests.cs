using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultElseTests
{
    [Fact]
    public void Else_ShouldReturnValue_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<string> elseResult = result.Else(errors => new Error("Code", "Message", errors.Select(e => ErrorDescriptor.Validation)));

        // Assert
        elseResult.IsSuccess.Should().BeTrue();
        elseResult.Value.Should().Be("value");
    }

    [Fact]
    public void Else_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<string> elseResult = result.Else(errors => new Error("NewCode", "NewMessage", errors.Select(e => ErrorDescriptor.Validation)));

        // Assert
        elseResult.IsFailure.Should().BeTrue();
        elseResult.Errors.Should().ContainSingle().Which.Code.Should().Be("NewCode");
    }

    [Fact]
    public void Else_WithFuncReturningIError_ShouldReturnErrorResult_WhenCalledWithFailure()
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
    public void Else_WithFuncReturningIError_ShouldNotChangeResult_WhenCalledWithSuccess()
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
    public void Else_WithFuncReturningIEnumerableIError_ShouldReturnProcessedErrorResult_WhenCalledWithFailure()
    {
        // Arrange
        var errors = new List<IError>
        {
            Error.Validation("Original error"),
            Error.Validation("New error")
        };

        var failedResult = Result.Failure<string>(errors);

        // Act
        Result<string> result = failedResult.Else(es => new List<IError> { Error.Validation("Processed error") });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Message.Should().Be("Processed error");
    }

    [Fact]
    public void Else_WithSingleIError_ShouldReturnOverrideError_WhenCalledWithFailure()
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
    public void Else_WithFuncReturningT_ShouldReturnRecoveredValue_WhenCalledWithFailure()
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
    public void Else_WithValue_ShouldReturnDefaultValue_WhenCalledWithFailure()
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
    public async Task ElseAsync_ShouldReturnValue_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        Result<string> elseResult = await result.ElseAsync(async errors =>
        {
            await Task.Delay(1);
            return "fallback value";
        });

        // Assert
        elseResult.IsSuccess.Should().BeTrue();
        elseResult.Value.Should().Be("value");
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Result<string> elseResult = await result.ElseAsync(async errors =>
        {
            await Task.Delay(1);
            return new Error("NewCode", "NewMessage", errors.Select(e => ErrorDescriptor.Validation));
        });

        // Assert
        elseResult.IsFailure.Should().BeTrue();
        elseResult.Errors.Should().ContainSingle().Which.Code.Should().Be("NewCode");
    }

    [Fact]
    public async Task ElseAsync_WithFuncReturningTaskT_ShouldReturnAsyncRecoveredValue_WhenCalledWithFailure()
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
    public async Task ElseAsync_WithFuncReturningTaskIError_ShouldReturnAsyncErrorResult_WhenCalledWithFailure()
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
    public async Task ElseAsync_WithFuncReturningTaskIEnumerableIError_ShouldReturnProcessedAsyncErrorResult_WhenCalledWithFailure()
    {
        // Arrange
        var errors = new List<IError> { Error.Validation("Original error") };
        var failedResult = Result.Failure<string>(errors);

        // Act
        Result<string> result = await failedResult.ElseAsync(errors => Task.FromResult<IEnumerable<IError>>(new IError[] { Error.Validation("Processed async error") }));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.First().Message.Should().Be("Processed async error");
    }

    [Fact]
    public async Task ElseAsync_WithTaskIError_ShouldReturnAsyncTaskErrorResult_WhenCalledWithFailure()
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
    public async Task ElseAsync_WithTaskT_ShouldReturnAsyncTaskValue_WhenCalledWithFailure()
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
