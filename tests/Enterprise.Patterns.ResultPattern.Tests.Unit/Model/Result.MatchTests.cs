using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultMatchTests
{
    [Fact]
    public void Match_ShouldReturnOnSuccessResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        Func<string> onSuccess = () => "Success";
        static string OnError(IEnumerable<IError> errors) => "Error";

        // Act
        string result = initialResult.Match(onSuccess, OnError);

        // Assert
        result.Should().Be("Success");
    }

    [Fact]
    public void Match_ShouldReturnOnErrorResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        string OnSuccess() => "Success";
        string OnError(IEnumerable<IError> errors) => "Error";

        // Act
        string result = initialResult.Match(OnSuccess, OnError);

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnOnSuccessResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        Task<string> OnSuccessAsync() => Task.FromResult("Success");
        Task<string> OnErrorAsync(IEnumerable<IError> errors) => Task.FromResult("Error");

        // Act
        string result = await initialResult.MatchAsync(OnSuccessAsync, OnErrorAsync);

        // Assert
        result.Should().Be("Success");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnOnErrorResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        var errors = new List<IError> { Error.NullValue() };
        var initialResult = Result.Failure(errors);
        Task<string> OnSuccessAsync() => Task.FromResult("Success");
        Task<string> OnErrorAsync(IEnumerable<IError> errors) => Task.FromResult("Error");

        // Act
        string result = await initialResult.MatchAsync(OnSuccessAsync, OnErrorAsync);

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public void MatchFirst_ShouldReturnOnSuccessResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        string OnSuccess() => "Success";
        string OnFirstError(IError error) => "Error";

        // Act
        string result = initialResult.MatchFirst(OnSuccess, OnFirstError);

        // Assert
        result.Should().Be("Success");
    }

    [Fact]
    public void MatchFirst_ShouldReturnOnFirstErrorResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        NullValueError error = Error.NullValue();
        var errors = new List<IError> { error };
        var initialResult = Result.Failure(errors);
        string OnSuccess() => "Success";
        string OnFirstError(IError error) => "Error";

        // Act
        string result = initialResult.MatchFirst(OnSuccess, OnFirstError);

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public async Task MatchFirstAsync_ShouldReturnOnSuccessResult_WhenIsSuccessIsTrue()
    {
        // Arrange
        var initialResult = Result.Success();
        Task<string> OnSuccessAsync() => Task.FromResult("Success");
        Task<string> OnFirstErrorAsync(IError error) => Task.FromResult("Error");

        // Act
        string result = await initialResult.MatchFirstAsync(OnSuccessAsync, OnFirstErrorAsync);

        // Assert
        result.Should().Be("Success");
    }

    [Fact]
    public async Task MatchFirstAsync_ShouldReturnOnFirstErrorResult_WhenIsSuccessIsFalse()
    {
        // Arrange
        NullValueError error = Error.NullValue();
        var errors = new List<IError> { error };
        var initialResult = Result.Failure(errors);
        Task<string> OnSuccessAsync() => Task.FromResult("Success");
        Task<string> ErrorAsync(IError error) => Task.FromResult("Error");

        // Act
        string result = await initialResult.MatchFirstAsync(OnSuccessAsync, ErrorAsync);

        // Assert
        result.Should().Be("Error");
    }
}
