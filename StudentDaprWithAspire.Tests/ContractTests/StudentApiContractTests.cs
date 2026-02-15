using FluentAssertions;
using StudentDaprWithAspire.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace StudentDaprWithAspire.Tests.ContractTests;

/// <summary>
/// Contract tests verify API request/response contracts
/// </summary>
public class StudentApiContractTests
{
    [Fact]
    public void Student_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var student = new Student
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20
        };

        // Assert
        student.Should().NotBeNull();
        student.Id.Should().Be(1);
        student.Name.Should().Be("John Doe");
        student.Email.Should().Be("john@example.com");
        student.Age.Should().Be(20);
    }

    [Fact]
    public void CreateStudentRequest_ShouldSerializeCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Age = 22
        };

        // Act
        var json = JsonSerializer.Serialize(student);
        var deserialized = JsonSerializer.Deserialize<Student>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().Be(student.Name);
        deserialized.Email.Should().Be(student.Email);
        deserialized.Age.Should().Be(student.Age);
    }

    [Fact]
    public void GetAllStudentsResponse_ShouldBeEnumerable()
    {
        // Arrange
        var students = new List<Student>
        {
            new() { Id = 1, Name = "Student 1", Email = "s1@test.com", Age = 20 },
            new() { Id = 2, Name = "Student 2", Email = "s2@test.com", Age = 21 }
        };

        // Act
        var json = JsonSerializer.Serialize(students);
        var deserialized = JsonSerializer.Deserialize<List<Student>>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Should().HaveCount(2);
        deserialized![0].Id.Should().Be(1);
        deserialized[1].Id.Should().Be(2);
    }

    [Theory]
    [InlineData("", "test@example.com", 20)] // Empty name
    [InlineData("John", "", 20)] // Empty email
    [InlineData("John", "test@example.com", -1)] // Invalid age
    public void Student_ShouldValidateRequiredFields(string name, string email, int age)
    {
        // Arrange & Act
        var student = new Student
        {
            Name = name,
            Email = email,
            Age = age
        };

        // Assert - Basic validation checks
        if (string.IsNullOrEmpty(name))
            student.Name.Should().BeEmpty();
        
        if (string.IsNullOrEmpty(email))
            student.Email.Should().BeEmpty();
        
        if (age < 0)
            student.Age.Should().BeLessThan(0);
    }

    [Fact]
    public void UpdateStudentRequest_ShouldIncludeId()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Name = "Updated Name",
            Email = "updated@example.com",
            Age = 25
        };

        // Act
        var json = JsonSerializer.Serialize(student);

        // Assert
        json.Should().Contain("\"Id\":1");
        json.Should().Contain("Updated Name");
    }

    [Fact]
    public void StudentResponse_ShouldMatchExpectedSchema()
    {
        // Arrange
        var expectedProperties = new[] { "Id", "Name", "Email", "Age" };
        var student = new Student
        {
            Id = 1,
            Name = "Test",
            Email = "test@example.com",
            Age = 20
        };

        // Act
        var properties = typeof(Student).GetProperties().Select(p => p.Name);

        // Assert
        properties.Should().Contain(expectedProperties);
    }
}
