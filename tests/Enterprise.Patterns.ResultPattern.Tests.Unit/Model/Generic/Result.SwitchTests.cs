using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultSwitchTests
{
    [Fact]
    public void Switch_ShouldInvokeOnSuccess_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");
        bool successCalled = false;
        bool errorCalled = false;

        // Act
        result.Switch(
            onSuccess: value => successCalled = true,
            onError: errors => errorCalled = true);

        // Assert
        successCalled.Should().BeTrue();
        errorCalled.Should().BeFalse();
    }

    [Fact]
    public void Switch_ShouldInvokeOnError_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);
        bool successCalled = false;
        bool errorCalled = false;

        // Act
        result.Switch(
            onSuccess: value => successCalled = true,
            onError: errors => errorCalled = true);

        // Assert
        successCalled.Should().BeFalse();
        errorCalled.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchAsync_ShouldInvokeOnSuccess_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");
        bool successCalled = false;
        bool errorCalled = false;

        // Act
        await result.SwitchAsync(
            onSuccess: async value =>
            {
                successCalled = true;
                await Task.Delay(1);
            },
            onErrorAsync: async errors =>
            {
                errorCalled = true;
                await Task.Delay(1);
            });

        // Assert
        successCalled.Should().BeTrue();
        errorCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_ShouldInvokeOnError_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);
        bool successCalled = false;
        bool errorCalled = false;

        // Act
        await result.SwitchAsync(
            onSuccess: async value =>
            {
                successCalled = true;
                await Task.Delay(1);
            },
            onErrorAsync: async errors =>
            {
                errorCalled = true;
                await Task.Delay(1);
            });

        // Assert
        successCalled.Should().BeFalse();
        errorCalled.Should().BeTrue();
    }

    [Fact]
    public void Switch_ExecutesOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;

        // Act
        successResult.Switch(
            value => valueActionInvoked = true,
            errors => errorActionInvoked = true);

        // Assert
        valueActionInvoked.Should().BeTrue();
        errorActionInvoked.Should().BeFalse();
    }

    [Fact]
    public void Switch_ExecutesOnError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;

        // Act
        failedResult.Switch(
            value => valueActionInvoked = true,
            errors => errorActionInvoked = true);

        // Assert
        valueActionInvoked.Should().BeFalse();
        errorActionInvoked.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchAsync_ExecutesOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;

        // Act
        await successResult.SwitchAsync(
            async value =>
            {
                valueActionInvoked = true;
                await Task.CompletedTask;
            },
            async errors =>
            {
                errorActionInvoked = true;
                await Task.CompletedTask;
            });

        // Assert
        valueActionInvoked.Should().BeTrue();
        errorActionInvoked.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_ExecutesOnError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;
        IEnumerable<IError> onErrorErrors = new List<IError>();

        // Act
        await failedResult.SwitchAsync(
            async value =>
            {
                valueActionInvoked = true;
                await Task.CompletedTask;
            },
            async errors =>
            {
                errorActionInvoked = true;
                onErrorErrors = errors;
                await Task.CompletedTask;
            });

        // Assert
        valueActionInvoked.Should().BeFalse();
        errorActionInvoked.Should().BeTrue();
        onErrorErrors.Should().Contain(error);
    }

    [Fact]
    public void SwitchFirst_ExecutesOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;

        // Act
        successResult.SwitchFirst(
            value => valueActionInvoked = true,
            error => errorActionInvoked = true);

        // Assert
        valueActionInvoked.Should().BeTrue();
        errorActionInvoked.Should().BeFalse();
    }

    [Fact]
    public void SwitchFirst_ExecutesOnFirstError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;
        string firstErrorMessage = string.Empty;

        // Act
        failedResult.SwitchFirst(
            value => valueActionInvoked = true,
            firstError =>
            {
                errorActionInvoked = true;
                firstErrorMessage = firstError.Message;
            });

        // Assert
        valueActionInvoked.Should().BeFalse();
        errorActionInvoked.Should().BeTrue();
        firstErrorMessage.Should().Be("Error");
    }

    [Fact]
    public async Task SwitchFirstAsync_ExecutesOnValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;

        // Act
        await successResult.SwitchFirstAsync(
            async value =>
            {
                valueActionInvoked = true;
                await Task.CompletedTask;
            },
            async error =>
            {
                errorActionInvoked = true;
                await Task.CompletedTask;
            });

        // Assert
        valueActionInvoked.Should().BeTrue();
        errorActionInvoked.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchFirstAsync_ExecutesOnFirstError_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);
        bool valueActionInvoked = false;
        bool errorActionInvoked = false;
        string firstErrorMessage = string.Empty;

        // Act
        await failedResult.SwitchFirstAsync(
            async value =>
            {
                valueActionInvoked = true;
                await Task.CompletedTask;
            },
            async firstError =>
            {
                errorActionInvoked = true;
                firstErrorMessage = firstError.Message;
                await Task.CompletedTask;
            });

        // Assert
        valueActionInvoked.Should().BeFalse();
        errorActionInvoked.Should().BeTrue();
        firstErrorMessage.Should().Be("Error");
    }
}
