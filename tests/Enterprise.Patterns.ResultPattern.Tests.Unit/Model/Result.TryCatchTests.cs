using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultTryCatchTests
{
    [Fact]
    public void TryCatch_ShouldExecuteFunc_WhenIsSuccessIsTrue_AndNoException()
    {
        // Arrange
        var initialResult = Result.Success();
        bool funcExecuted = false;

        Result Func()
        {
            funcExecuted = true;
            return Result.Success();
        }

        // Act
        Result result = initialResult.TryCatch(Func);

        // Assert
        funcExecuted.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void TryCatch_ShouldNotExecuteFunc_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool funcExecuted = false;
        Func<Result> func = () => { funcExecuted = true; return Result.Success(); };

        // Act
        Result result = initialResult.TryCatch(func);

        // Assert
        funcExecuted.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void TryCatch_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var initialResult = Result.Success();
        Func<Result> func = () => throw new Exception("Test exception");

        // Act
        Result result = initialResult.TryCatch(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Test exception");
    }

    [Fact]
    public async Task TryCatchAsync_ShouldExecuteFunc_WhenIsSuccessIsTrue_AndNoException()
    {
        // Arrange
        var initialResult = Result.Success();
        bool funcExecuted = false;
        Func<Task<Result>> func = async () => { funcExecuted = true; return await Task.FromResult(Result.Success()); };

        // Act
        Result result = await initialResult.TryCatchAsync(func);

        // Assert
        funcExecuted.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TryCatchAsync_ShouldNotExecuteFunc_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        bool funcExecuted = false;
        Func<Task<Result>> func = async () => { funcExecuted = true; return await Task.FromResult(Result.Success()); };

        // Act
        Result result = await initialResult.TryCatchAsync(func);

        // Assert
        funcExecuted.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task TryCatchAsync_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var initialResult = Result.Success();
        static Task<Result> Func() => throw new Exception("Test exception");

        // Act
        Result result = await initialResult.TryCatchAsync(Func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Test exception");
    }
}
