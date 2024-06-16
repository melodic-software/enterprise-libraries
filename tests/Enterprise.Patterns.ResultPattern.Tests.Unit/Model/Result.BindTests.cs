using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultBindTests
{
    [Fact]
    public void Bind_ShouldReturnResultOfBindFunction_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        Func<Result> bindFunction = Result.Success;

        // Act
        Result result = initialResult.Bind(bindFunction);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Bind_ShouldReturnFailureResultWithSameErrors_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        Func<Result> bindFunction = Result.Success;

        // Act
        Result result = initialResult.Bind(bindFunction);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task BindAsync_ShouldReturnResultOfBindAsyncFunction_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        static Task<Result> BindAsyncFunction() => Task.FromResult(Result.Success());

        // Act
        Result result = await initialResult.BindAsync(BindAsyncFunction);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task BindAsync_ShouldReturnFailureResultWithSameErrors_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        static Task<Result> BindAsyncFunction() => Task.FromResult(Result.Success());

        // Act
        Result result = await initialResult.BindAsync(BindAsyncFunction);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }
}
