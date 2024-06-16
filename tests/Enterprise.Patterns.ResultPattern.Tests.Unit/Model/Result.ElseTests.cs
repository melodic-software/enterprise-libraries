using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultElseTests
{
    [Fact]
    public void Else_ShouldReturnOriginalResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        static IError OnErrorFunc(IEnumerable<IError> errors) => Substitute.For<IError>();

        // Act
        Result result = initialResult.Else(OnErrorFunc);

        // Assert
        result.Should().Be(initialResult);
    }

    [Fact]
    public void Else_ShouldReturnNewResultFromError_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        IError newError = Error.NotFound();
        IError OnErrorFunc(IEnumerable<IError> _) => newError;

        // Act
        Result result = initialResult.Else(OnErrorFunc);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(newError);
    }

    [Fact]
    public void Else_ShouldReturnNewResultFromErrors_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        var newErrors = new List<IError> { Error.NotFound() };
        IEnumerable<IError> OnErrorFunc(IEnumerable<IError> _) => newErrors;

        // Act
        Result result = initialResult.Else(OnErrorFunc);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(newErrors);
    }

    [Fact]
    public void Else_ShouldReturnSpecifiedError_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        IError? newError = Error.NotFound();

        // Act
        Result result = initialResult.Else(newError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(newError);
    }

    [Fact]
    public void Else_ShouldReturnSpecifiedResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        var newResult = Result.Success();

        // Act
        Result result = initialResult.Else(newResult);

        // Assert
        result.Should().Be(newResult);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnOriginalResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        static Task<Result> OnErrorFuncAsync(IEnumerable<IError> errors) => Task.FromResult(Result.Failure(Substitute.For<IError>()));

        // Act
        Result result = await initialResult.ElseAsync(OnErrorFuncAsync);

        // Assert
        result.Should().Be(initialResult);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnNewResultFromAsyncFunc_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        var newResult = Result.Failure(Substitute.For<IError>());
        Task<Result> OnErrorFuncAsync(IEnumerable<IError> _) => Task.FromResult(newResult);

        // Act
        Result result = await initialResult.ElseAsync(OnErrorFuncAsync);

        // Assert
        result.Should().Be(newResult);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnNewResultFromErrorAsyncFunc_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        IError? newError = Error.NotFound();
        Task<IError> OnErrorFuncAsync(IEnumerable<IError> _) => Task.FromResult(newError);

        // Act
        Result result = await initialResult.ElseAsync(OnErrorFuncAsync);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(newError);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnNewResultFromErrorsAsyncFunc_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        var newErrors = new List<IError> { Error.NotFound() };
        Task<IEnumerable<IError>> OnErrorFuncAsync(IEnumerable<IError> _) => Task.FromResult(newErrors.AsEnumerable());

        // Act
        Result result = await initialResult.ElseAsync(OnErrorFuncAsync);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(newErrors);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnNewResultFromTaskError_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        IError? newError = Error.NotFound();
        var onErrorTask = Task.FromResult(newError);

        // Act
        Result result = await initialResult.ElseAsync(onErrorTask);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(newError);
    }

    [Fact]
    public async Task ElseAsync_ShouldReturnNewResultFromTaskResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        var newResult = Result.Failure(Substitute.For<IError>());
        var onErrorTask = Task.FromResult(newResult);

        // Act
        Result result = await initialResult.ElseAsync(onErrorTask);

        // Assert
        result.Should().Be(newResult);
    }
}
