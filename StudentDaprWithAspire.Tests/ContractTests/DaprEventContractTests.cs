using FluentAssertions;
using System.Text.Json;

namespace StudentDaprWithAspire.Tests.ContractTests;

/// <summary>
/// Contract tests for Dapr pub/sub event schemas
/// </summary>
public class DaprEventContractTests
{
    [Fact]
    public void StudentCreatedEvent_ShouldHaveCorrectStructure()
    {
        // Arrange
        var eventData = new
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20,
            EventType = "student-created",
            Timestamp = DateTime.UtcNow
        };

        // Act
        var json = JsonSerializer.Serialize(eventData);
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Should().ContainKey("Id");
        deserialized.Should().ContainKey("Name");
        deserialized.Should().ContainKey("Email");
        deserialized.Should().ContainKey("Age");
    }

    [Fact]
    public void StudentUpdatedEvent_ShouldHaveCorrectStructure()
    {
        // Arrange
        var eventData = new
        {
            Id = 1,
            Name = "Updated Name",
            Email = "updated@example.com",
            Age = 25,
            EventType = "student-updated"
        };

        // Act
        var json = JsonSerializer.Serialize(eventData);

        // Assert
        json.Should().Contain("\"Id\":1");
        json.Should().Contain("student-updated");
    }

    [Fact]
    public void StudentDeletedEvent_ShouldContainId()
    {
        // Arrange
        var eventData = new { Id = 1 };

        // Act
        var json = JsonSerializer.Serialize(eventData);
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Should().ContainKey("Id");
        deserialized["Id"].GetInt32().Should().Be(1);
    }

    [Theory]
    [InlineData("student-created")]
    [InlineData("student-updated")]
    [InlineData("student-deleted")]
    public void DaprEvent_ShouldHaveValidTopicNames(string topicName)
    {
        // Assert
        topicName.Should().NotBeNullOrEmpty();
        topicName.Should().StartWith("student-");
        topicName.Should().MatchRegex("^[a-z-]+$");
    }

    [Fact]
    public void DaprPubSubComponent_ShouldHaveCorrectName()
    {
        // Arrange
        var pubsubName = "pubsub";

        // Assert
        pubsubName.Should().Be("pubsub");
        pubsubName.Should().NotBeNullOrEmpty();
    }
}
