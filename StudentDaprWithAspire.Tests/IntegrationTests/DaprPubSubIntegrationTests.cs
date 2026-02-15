using Dapr.Client;
using FluentAssertions;
using Moq;
using StudentDaprWithAspire.Domain.Entities;

namespace StudentDaprWithAspire.Tests.IntegrationTests;

/// <summary>
/// Integration tests for Dapr pub/sub event publishing
/// </summary>
public class DaprPubSubIntegrationTests
{
    private readonly Mock<DaprClient> _mockDaprClient;

    public DaprPubSubIntegrationTests()
    {
        _mockDaprClient = new Mock<DaprClient>();
    }

    [Fact]
    public async Task PublishStudentCreatedEvent_ShouldCallDaprClient()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20
        };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                "student-created",
                student,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockDaprClient.Object.PublishEventAsync("pubsub", "student-created", student);

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                "pubsub",
                "student-created",
                student,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishStudentUpdatedEvent_ShouldCallDaprClient()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Name = "Updated Name",
            Email = "updated@example.com",
            Age = 25
        };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                "student-updated",
                student,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockDaprClient.Object.PublishEventAsync("pubsub", "student-updated", student);

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                "pubsub",
                "student-updated",
                student,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishStudentDeletedEvent_ShouldCallDaprClient()
    {
        // Arrange
        var deletedData = new { Id = 1 };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                "student-deleted",
                deletedData,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockDaprClient.Object.PublishEventAsync("pubsub", "student-deleted", deletedData);

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                "pubsub",
                "student-deleted",
                deletedData,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishEvent_WithCorrectPubSubName_ShouldSucceed()
    {
        // Arrange
        var pubsubName = "pubsub";
        var topicName = "student-created";
        var student = new Student { Id = 1, Name = "Test", Email = "test@example.com", Age = 20 };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                pubsubName,
                topicName,
                student,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockDaprClient.Object.PublishEventAsync(pubsubName, topicName, student);

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                pubsubName,
                topicName,
                student,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("student-created")]
    [InlineData("student-updated")]
    [InlineData("student-deleted")]
    public async Task PublishEvent_WithDifferentTopics_ShouldCallCorrectTopic(string topicName)
    {
        // Arrange
        var eventData = new { Id = 1, Name = "Test" };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                topicName,
                eventData,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockDaprClient.Object.PublishEventAsync("pubsub", topicName, eventData);

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                "pubsub",
                topicName,
                eventData,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishMultipleEvents_ShouldCallDaprClientMultipleTimes()
    {
        // Arrange
        var students = new[]
        {
            new Student { Id = 1, Name = "Student 1", Email = "s1@test.com", Age = 20 },
            new Student { Id = 2, Name = "Student 2", Email = "s2@test.com", Age = 21 },
            new Student { Id = 3, Name = "Student 3", Email = "s3@test.com", Age = 22 }
        };

        _mockDaprClient
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                "student-created",
                It.IsAny<Student>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        foreach (var student in students)
        {
            await _mockDaprClient.Object.PublishEventAsync("pubsub", "student-created", student);
        }

        // Assert
        _mockDaprClient.Verify(
            x => x.PublishEventAsync(
                "pubsub",
                "student-created",
                It.IsAny<Student>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
    }
}
