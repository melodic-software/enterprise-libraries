using System.Globalization;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultMatchTests
{
    [Fact]
    public void Match_ShouldReturnSuccessResult_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        string matchResult = result.Match(
            onSuccess: value => value,
            onError: errors => "error");

        // Assert
        matchResult.Should().Be("value");
    }

    [Fact]
    public void Match_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        string matchResult = result.Match(
            onSuccess: value => value,
            onError: errors => errors.First().Code);

        // Assert
        matchResult.Should().Be("Code");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessResult_WhenCalledWithSuccess()
    {
        // Arrange
        var result = Result<string>.Success("value");

        // Act
        string matchResult = await result.MatchAsync(
            onSuccessAsync: async value =>
            {
                await Task.Delay(1);
                return value;
            },
            onErrorAsync: async errors =>
            {
                await Task.Delay(1);
                return "error";
            });

        // Assert
        matchResult.Should().Be("value");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureResult_WhenCalledWithFailure()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        string matchResult = await result.MatchAsync(
            onSuccessAsync: async value =>
            {
                await Task.Delay(1);
                return value;
            },
            onErrorAsync: async errors =>
            {
                await Task.Delay(1);
                return errors.First().Code;
            });

        // Assert
        matchResult.Should().Be("Code");
    }

    [Fact]
    public void Match_ShouldReturnUpperCaseValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");

        // Act
        string result = successResult.Match(
            value => value.ToUpper(CultureInfo.InvariantCulture),
            errors => "error");

        // Assert
        result.Should().Be("SUCCESS VALUE");
    }

    [Fact]
    public void Match_ShouldReturnErrorMessage_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        string result = failedResult.Match(
            value => value.ToUpper(CultureInfo.InvariantCulture),
            errors => errors.First().Message);

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnUpperCaseValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");

        // Act
        string result = await successResult.MatchAsync(
            value => Task.FromResult(value.ToUpper(CultureInfo.InvariantCulture)),
            errors => Task.FromResult("error"));

        // Assert
        result.Should().Be("SUCCESS VALUE");
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnErrorMessage_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        string result = await failedResult.MatchAsync(
            value => Task.FromResult(value.ToUpper(CultureInfo.InvariantCulture)),
            errors => Task.FromResult(errors.First().Message));

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public void MatchFirst_ShouldReturnUpperCaseValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");

        // Act
        string result = successResult.MatchFirst(
            value => value.ToUpper(CultureInfo.InvariantCulture),
            error => "error");

        // Assert
        result.Should().Be("SUCCESS VALUE");
    }

    [Fact]
    public void MatchFirst_ShouldReturnErrorMessage_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        string result = failedResult.MatchFirst(
            value => value.ToUpper(CultureInfo.InvariantCulture),
            error => error.Message);

        // Assert
        result.Should().Be("Error");
    }

    [Fact]
    public async Task MatchFirstAsync_ShouldReturnUpperCaseValue_ForSuccessfulResult()
    {
        // Arrange
        var successResult = Result.Success("success value");

        // Act
        string result = await successResult.MatchFirstAsync(
            value => Task.FromResult(value.ToUpper(CultureInfo.InvariantCulture)),
            error => Task.FromResult("error"));

        // Assert
        result.Should().Be("SUCCESS VALUE");
    }

    [Fact]
    public async Task MatchFirstAsync_ShouldReturnErrorMessage_ForFailedResult()
    {
        // Arrange
        ValidationError error = Error.Validation("Error");
        var failedResult = Result.Failure<string>(error);

        // Act
        string result = await failedResult.MatchFirstAsync(
            value => Task.FromResult(value.ToUpper(CultureInfo.InvariantCulture)),
            error => Task.FromResult(error.Message));

        // Assert
        result.Should().Be("Error");
    }
}
