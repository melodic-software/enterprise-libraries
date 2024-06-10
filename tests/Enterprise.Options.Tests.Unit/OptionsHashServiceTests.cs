using System.Reflection;
using Enterprise.Options.Hashing;
using Enterprise.Serialization.Json;
using FluentAssertions;
using NSubstitute;

namespace Enterprise.Options.Tests.Unit;

public class OptionsHashServiceTests
{
    [Fact]
    public void ComputeHash_ShouldReturnExpectedHash_WhenSerializableObjectIsProvided()
    {
        // Arrange
        var options = new { Name = "Test", Value = 123 };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        string expectedSerializedData = "{\"Name\":\"Test\",\"Value\":123}";
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns(expectedSerializedData);
        string expectedHash = "b01f9a4a87d97098d8fc7a4d001272d7e61cece4cec7b21e2798993598b025f4";

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().Be(expectedHash);
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenObjectHasNoSerializableProperties()
    {
        // Arrange
        var options = new { };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldHandleComplexObjects_WhenNestedObjectsAreProvided()
    {
        // Arrange
        var options = new
        {
            Outer = new { Inner = new { Value = "InnerValue" } },
            Collection = new List<int> { 1, 2, 3 }
        };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        string expectedSerializedData = "{\"Outer\":{\"Inner\":{\"Value\":\"InnerValue\"}},\"Collection\":{\"Item0\":{\"Value\":1},\"Item1\":{\"Value\":2},\"Item2\":{\"Value\":3}}}";
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns(expectedSerializedData);
        string expectedHash = "fa4dad13fa20995362022813092cf95f1c0b62429504763e7d1dbd8e40c38c6d";

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().Be(expectedHash);
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenNullObjectIsProvided()
    {
        // Arrange
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(null!, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldThrowArgumentNullException_WhenSerializerIsNull()
    {
        // Arrange
        var options = new { Name = "Test" };

        // Act
        Func<string> act = () => OptionsHashingService.ComputeHash(options, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'jsonSerializer')");
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreDelegateProperties_WhenProvided()
    {
        // Arrange
        var options = new { Action = new Action(() => { }) };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreAssemblyProperties_WhenProvided()
    {
        // Arrange
        var options = new { Assembly = Assembly.GetExecutingAssembly() };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTypeProperties_WhenProvided()
    {
        // Arrange
        var options = new { Type = typeof(string) };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreStreamProperties_WhenProvided()
    {
        // Arrange
        var options = new { Stream = new MemoryStream() };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTaskProperties_WhenProvided()
    {
        // Arrange
        var options = new { Task = Task.CompletedTask };
        ISerializeJson? jsonSerializer = Substitute.For<ISerializeJson>();
        jsonSerializer.Serialize(Arg.Any<Dictionary<string, object>>()).Returns("{}");

        // Act
        string hash = OptionsHashingService.ComputeHash(options, jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }
}
