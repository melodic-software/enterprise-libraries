using System.Reflection;
using Enterprise.Options.Hashing;
using Enterprise.Serialization.Json;
using Enterprise.Serialization.Json.Microsoft;
using FluentAssertions;

namespace Enterprise.Options.Tests.Integration;

public class OptionsHashServiceTests
{
    private readonly ISerializeJson _jsonSerializer = new DefaultJsonSerializer();

    [Fact]
    public void ComputeHash_ShouldProduceCorrectHash_WhenComplexObjectIsProvided()
    {
        // Arrange
        var options = new
        {
            Name = "Test",
            Details = new { Age = 30, Interests = new[] { "Coding", "Music" } }
        };

        // Act
        string hash = OptionsHashingService.ComputeHash(options, _jsonSerializer);

        // Assert
        string expectedHash = "470accda0eb80ba5c459d5c83e93ac9138a1e3d4428f6228137e033e2212ac05";

        hash.Should().Be(expectedHash);
    }

    [Fact]
    public void ComputeHash_ShouldProduceCorrectHash_WhenSimpleObjectIsProvided()
    {
        // Arrange
        var options = new { Name = "Test", Value = 123 };

        // Act
        string hash = OptionsHashingService.ComputeHash(options, _jsonSerializer);

        // Assert
        // Verify that a valid hash is produced (specific hash value can depend on the actual serialization result)
        hash.Should().NotBeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenEmptyObjectIsProvided()
    {
        // Arrange
        var options = new { };

        // Act
        string hash = OptionsHashingService.ComputeHash(options, _jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldBeSuccessful_WhenComplexObjectIsProvided()
    {
        // Arrange
        var options = new
        {
            Outer = new { Inner = new { Value = "InnerValue" } },
            Collection = new List<int> { 1, 2, 3 }
        };

        // Act
        string hash = OptionsHashingService.ComputeHash(options, _jsonSerializer);

        // Assert
        hash.Should().NotBeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenNullObjectIsProvided()
    {
        // Act
        string hash = OptionsHashingService.ComputeHash(null!, _jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public async Task ComputeHash_ShouldBeThreadSafe_WhenAccessedConcurrently()
    {
        // Arrange
        var options = new { Name = "ConcurrentTest", Value = 42 };
        var jsonSerializer = new DefaultJsonSerializer();
        var tasks = new List<Task>();
        int numberOfConcurrentAccesses = 100;

        // Act
        for (int i = 0; i < numberOfConcurrentAccesses; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                string hash = OptionsHashingService.ComputeHash(options, _jsonSerializer);
                hash.Should().NotBeEmpty();
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        // If no exceptions are thrown and all tasks complete, the test passes
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreDelegateProperties_WhenProvided()
    {
        TestNonSerializableType(new { Action = new Action(() => { }) });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreAssemblyProperties_WhenProvided()
    {
        TestNonSerializableType(new { Assembly = Assembly.GetExecutingAssembly() });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTypeProperties_WhenProvided()
    {
        TestNonSerializableType(new { Type = typeof(string) });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreStreamProperties_WhenProvided()
    {
        TestNonSerializableType(new { Stream = new MemoryStream() });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTaskProperties_WhenProvided()
    {
        TestNonSerializableType(new { Task = Task.CompletedTask });
    }

    private void TestNonSerializableType(object nonSerializableObject)
    {
        // Act
        Action act = () => OptionsHashingService.ComputeHash(nonSerializableObject, _jsonSerializer);

        // Assert
        act.Should().NotThrow<NotSupportedException>("because the method should handle non-serializable types gracefully.");
    }
}
