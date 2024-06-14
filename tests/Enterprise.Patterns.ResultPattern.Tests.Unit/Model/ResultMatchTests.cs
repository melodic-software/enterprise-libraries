using System.Globalization;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultMatchTests
{
    [Fact]
    public void Match_ExecutesOnValue_ForSuccessfulResult()
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
    public void Match_ExecutesOnError_ForFailedResult()
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
    public async Task MatchAsync_ExecutesOnValue_ForSuccessfulResult()
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
    public async Task MatchAsync_ExecutesOnError_ForFailedResult()
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
    public void MatchFirst_ExecutesOnValue_ForSuccessfulResult()
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
    public void MatchFirst_ExecutesOnFirstError_ForFailedResult()
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
    public async Task MatchFirstAsync_ExecutesOnValue_ForSuccessfulResult()
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
    public async Task MatchFirstAsync_ExecutesOnFirstError_ForFailedResult()
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
