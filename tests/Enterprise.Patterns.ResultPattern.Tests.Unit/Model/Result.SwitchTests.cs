using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultSwitchTests
{
    [Fact]
    public void Switch_ShouldExecuteOnSuccess_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessCalled = false;
        bool onErrorCalled = false;
        void OnSuccess() => onSuccessCalled = true;
        void OnError(IEnumerable<IError> errors) => onErrorCalled = true;

        // Act
        initialResult.Switch(OnSuccess, OnError);

        // Assert
        onSuccessCalled.Should().BeTrue();
        onErrorCalled.Should().BeFalse();
    }

    [Fact]
    public void Switch_ShouldExecuteOnError_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool onSuccessCalled = false;
        bool onErrorCalled = false;
        void OnSuccess() => onSuccessCalled = true;
        void OnError(IEnumerable<IError> errors) => onErrorCalled = true;

        // Act
        initialResult.Switch(OnSuccess, OnError);

        // Assert
        onSuccessCalled.Should().BeFalse();
        onErrorCalled.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchAsync_ShouldExecuteOnSuccess_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessCalled = false;
        bool onErrorCalled = false;

        Task OnSuccessAsync()
        {
            onSuccessCalled = true;
            return Task.CompletedTask;
        }

        Task OnErrorAsync(IEnumerable<IError> errors)
        {
            onErrorCalled = true;
            return Task.CompletedTask;
        }

        // Act
        await initialResult.SwitchAsync(OnSuccessAsync, OnErrorAsync);

        // Assert
        onSuccessCalled.Should().BeTrue();
        onErrorCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_ShouldExecuteOnError_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool onSuccessCalled = false;
        bool onErrorCalled = false;

        Task OnSuccessAsync()
        {
            onSuccessCalled = true;
            return Task.CompletedTask;
        }

        Task OnErrorAsync(IEnumerable<IError> errors)
        {
            onErrorCalled = true;
            return Task.CompletedTask;
        }

        // Act
        await initialResult.SwitchAsync(OnSuccessAsync, OnErrorAsync);

        // Assert
        onSuccessCalled.Should().BeFalse();
        onErrorCalled.Should().BeTrue();
    }

    [Fact]
    public void SwitchFirst_ShouldExecuteOnSuccess_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessCalled = false;
        bool onFirstErrorCalled = false;
        void OnSuccess() => onSuccessCalled = true;
        void OnFirstError(IError error) => onFirstErrorCalled = true;

        // Act
        initialResult.SwitchFirst(OnSuccess, OnFirstError);

        // Assert
        onSuccessCalled.Should().BeTrue();
        onFirstErrorCalled.Should().BeFalse();
    }

    [Fact]
    public void SwitchFirst_ShouldExecuteOnFirstError_WhenIsSuccessIsFalse()
    {
        // Arrange
        NullValueError error = Error.NullValue();
        var errors = new List<IError> { error };
        var initialResult = Result.Failure(errors);
        bool onSuccessCalled = false;
        bool onFirstErrorCalled = false;
        void OnSuccess() => onSuccessCalled = true;
        void OnFirstError(IError error) => onFirstErrorCalled = true;

        // Act
        initialResult.SwitchFirst(OnSuccess, OnFirstError);

        // Assert
        onSuccessCalled.Should().BeFalse();
        onFirstErrorCalled.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchFirstAsync_ShouldExecuteOnSuccess_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        bool onSuccessCalled = false;
        bool onFirstErrorCalled = false;

        Task OnSuccessAsync()
        {
            onSuccessCalled = true;
            return Task.CompletedTask;
        }

        Task OnFirstErrorAsync(IError error)
        {
            onFirstErrorCalled = true;
            return Task.CompletedTask;
        }

        // Act
        await initialResult.SwitchFirstAsync(OnSuccessAsync, OnFirstErrorAsync);

        // Assert
        onSuccessCalled.Should().BeTrue();
        onFirstErrorCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchFirstAsync_ShouldExecuteOnFirstError_WhenIsSuccessIsFalse()
    {
        // Arrange
        NullValueError error = Error.NullValue();
        var errors = new List<IError> { error };
        var initialResult = Result.Failure(errors);
        bool onSuccessCalled = false;
        bool onFirstErrorCalled = false;

        Task OnSuccessAsync()
        {
            onSuccessCalled = true;
            return Task.CompletedTask;
        }

        Task OnFirstErrorAsync(IError error)
        {
            onFirstErrorCalled = true;
            return Task.CompletedTask;
        }

        // Act
        await initialResult.SwitchFirstAsync(OnSuccessAsync, OnFirstErrorAsync);

        // Assert
        onSuccessCalled.Should().BeFalse();
        onFirstErrorCalled.Should().BeTrue();
    }
}
