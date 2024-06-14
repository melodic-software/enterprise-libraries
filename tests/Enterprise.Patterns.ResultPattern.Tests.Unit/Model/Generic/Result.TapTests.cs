using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultTapTests
{
    [Fact]
    public void Tap_ShouldInvokeAction_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");
        bool actionCalled = false;

        // Act
        Result<string> tapResult = result.Tap(value => actionCalled = true);

        // Assert
        actionCalled.Should().BeTrue();
        tapResult.Should().Be(result);
    }

    [Fact]
    public void Tap_ShouldNotInvokeAction_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);
        bool actionCalled = false;

        // Act
        Result<string> tapResult = result.Tap(value => actionCalled = true);

        // Assert
        actionCalled.Should().BeFalse();
        tapResult.Should().Be(result);
    }

    [Fact]
    public async Task TapAsync_ShouldInvokeAction_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");
        bool actionCalled = false;

        // Act
        Result<string> tapResult = await result.TapAsync(async value =>
        {
            actionCalled = true;
            await Task.Delay(1);
        });

        // Assert
        actionCalled.Should().BeTrue();
        tapResult.Should().Be(result);
    }

    [Fact]
    public async Task TapAsync_ShouldNotInvokeAction_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);
        bool actionCalled = false;

        // Act
        Result<string> tapResult = await result.TapAsync(async value =>
        {
            actionCalled = true;
            await Task.Delay(1);
        });

        // Assert
        actionCalled.Should().BeFalse();
        tapResult.Should().Be(result);
    }
}
