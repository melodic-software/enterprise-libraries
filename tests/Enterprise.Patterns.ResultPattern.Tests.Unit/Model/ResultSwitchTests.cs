using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultSwitchTests
{
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
            errors =>
            {
                errorActionInvoked = true;
            });

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
            }, errors =>
            {
                errorActionInvoked = true;
                return Task.CompletedTask;
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
        await failedResult.SwitchAsync(value =>
            {
                valueActionInvoked = true;
                return Task.CompletedTask;
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
            }, error =>
            {
                errorActionInvoked = true;
                return Task.CompletedTask;
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
        await failedResult.SwitchFirstAsync(value =>
            {
                valueActionInvoked = true;
                return Task.CompletedTask;
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
