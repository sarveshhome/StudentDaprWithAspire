using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Infrastructure.Data;
using StudentDaprWithAspire.Infrastructure.Repositories;

namespace StudentDaprWithAspire.Tests.IntegrationTests;

/// <summary>
/// Integration tests for StudentRepository with actual database
/// </summary>
public class StudentRepositoryIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly StudentRepository _repository;

    public StudentRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new StudentRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStudentToDatabase()
    {
        // Arrange
        var student = new Student
        {
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20
        };

        // Act
        var result = await _repository.AddAsync(student);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("John Doe");

        var savedStudent = await _context.Students.FindAsync(result.Id);
        savedStudent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new[]
        {
            new Student { Name = "Student 1", Email = "s1@test.com", Age = 20 },
            new Student { Name = "Student 2", Email = "s2@test.com", Age = 21 },
            new Student { Name = "Student 3", Email = "s3@test.com", Age = 22 }
        };

        foreach (var student in students)
        {
            await _repository.AddAsync(student);
        }

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var student = new Student
        {
            Name = "Test Student",
            Email = "test@example.com",
            Age = 25
        };
        var added = await _repository.AddAsync(student);

        // Act
        var result = await _repository.GetByIdAsync(added.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(added.Id);
        result.Name.Should().Be("Test Student");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingStudent_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudentInDatabase()
    {
        // Arrange
        var student = new Student
        {
            Name = "Original Name",
            Email = "original@example.com",
            Age = 20
        };
        var added = await _repository.AddAsync(student);

        // Modify student
        added.Name = "Updated Name";
        added.Email = "updated@example.com";
        added.Age = 25;

        // Act
        var result = await _repository.UpdateAsync(added);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Email.Should().Be("updated@example.com");
        result.Age.Should().Be(25);

        // Verify in database
        var dbStudent = await _context.Students.FindAsync(added.Id);
        dbStudent!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task UpdateAsync_NonExistingStudent_ShouldReturnNull()
    {
        // Arrange
        var student = new Student
        {
            Id = 99999,
            Name = "Non Existing",
            Email = "none@example.com",
            Age = 20
        };

        // Act
        var result = await _repository.UpdateAsync(student);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ExistingStudent_ShouldRemoveFromDatabase()
    {
        // Arrange
        var student = new Student
        {
            Name = "To Be Deleted",
            Email = "delete@example.com",
            Age = 20
        };
        var added = await _repository.AddAsync(student);

        // Act
        var result = await _repository.DeleteAsync(added.Id);

        // Assert
        result.Should().BeTrue();

        // Verify deletion
        var deletedStudent = await _context.Students.FindAsync(added.Id);
        deletedStudent.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingStudent_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.DeleteAsync(99999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ConcurrentOperations_ShouldWorkCorrectly()
    {
        // Arrange
        var tasks = new List<Task<Student>>();

        // Act - Add multiple students concurrently
        for (int i = 0; i < 10; i++)
        {
            var student = new Student
            {
                Name = $"Student {i}",
                Email = $"student{i}@test.com",
                Age = 20 + i
            };
            tasks.Add(_repository.AddAsync(student));
        }

        await Task.WhenAll(tasks);

        // Assert
        var allStudents = await _repository.GetAllAsync();
        allStudents.Should().HaveCount(10);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
