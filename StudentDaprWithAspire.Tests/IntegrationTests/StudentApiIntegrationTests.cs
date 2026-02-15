using FluentAssertions;
using StudentDaprWithAspire.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace StudentDaprWithAspire.Tests.IntegrationTests;

/// <summary>
/// Integration tests for Student API endpoints
/// </summary>
public class StudentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public StudentApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllStudents_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/students");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateStudent_ReturnsCreatedStudent()
    {
        // Arrange
        var newStudent = new Student
        {
            Name = "Integration Test Student",
            Email = "integration@test.com",
            Age = 22
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", newStudent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdStudent = await response.Content.ReadFromJsonAsync<Student>();
        createdStudent.Should().NotBeNull();
        createdStudent!.Id.Should().BeGreaterThan(0);
        createdStudent.Name.Should().Be(newStudent.Name);
        createdStudent.Email.Should().Be(newStudent.Email);
        createdStudent.Age.Should().Be(newStudent.Age);
    }

    [Fact]
    public async Task GetStudentById_ExistingStudent_ReturnsStudent()
    {
        // Arrange - Create a student first
        var newStudent = new Student
        {
            Name = "Test Student",
            Email = "test@example.com",
            Age = 20
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", newStudent);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<Student>();

        // Act
        var response = await _client.GetAsync($"/api/students/{createdStudent!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var student = await response.Content.ReadFromJsonAsync<Student>();
        student.Should().NotBeNull();
        student!.Id.Should().Be(createdStudent.Id);
    }

    [Fact]
    public async Task GetStudentById_NonExistingStudent_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/students/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateStudent_ExistingStudent_ReturnsUpdatedStudent()
    {
        // Arrange - Create a student first
        var newStudent = new Student
        {
            Name = "Original Name",
            Email = "original@example.com",
            Age = 20
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", newStudent);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<Student>();

        // Update the student
        createdStudent!.Name = "Updated Name";
        createdStudent.Email = "updated@example.com";
        createdStudent.Age = 25;

        // Act
        var response = await _client.PutAsJsonAsync($"/api/students/{createdStudent.Id}", createdStudent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedStudent = await response.Content.ReadFromJsonAsync<Student>();
        updatedStudent.Should().NotBeNull();
        updatedStudent!.Name.Should().Be("Updated Name");
        updatedStudent.Email.Should().Be("updated@example.com");
        updatedStudent.Age.Should().Be(25);
    }

    [Fact]
    public async Task UpdateStudent_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Name = "Test",
            Email = "test@example.com",
            Age = 20
        };

        // Act - Use different ID in URL
        var response = await _client.PutAsJsonAsync("/api/students/999", student);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteStudent_ExistingStudent_ReturnsNoContent()
    {
        // Arrange - Create a student first
        var newStudent = new Student
        {
            Name = "To Be Deleted",
            Email = "delete@example.com",
            Age = 20
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", newStudent);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<Student>();

        // Act
        var response = await _client.DeleteAsync($"/api/students/{createdStudent!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/students/{createdStudent.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteStudent_NonExistingStudent_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/students/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAndRetrieveMultipleStudents_WorksCorrectly()
    {
        // Arrange
        var students = new[]
        {
            new Student { Name = "Student 1", Email = "s1@test.com", Age = 20 },
            new Student { Name = "Student 2", Email = "s2@test.com", Age = 21 },
            new Student { Name = "Student 3", Email = "s3@test.com", Age = 22 }
        };

        // Act - Create students
        foreach (var student in students)
        {
            await _client.PostAsJsonAsync("/api/students", student);
        }

        // Get all students
        var response = await _client.GetAsync("/api/students");
        var allStudents = await response.Content.ReadFromJsonAsync<List<Student>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        allStudents.Should().NotBeNull();
        allStudents!.Count.Should().BeGreaterOrEqualTo(3);
    }
}
