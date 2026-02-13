using Moq;
using FluentAssertions;
using StudentDaprWithAspire.Application.Services;
using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Domain.Interfaces;

namespace StudentDaprWithAspire.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepository = new Mock<IStudentRepository>();
        _service = new StudentService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
    {
        var students = new List<Student>
        {
            new() { Id = 1, Name = "John", Email = "john@test.com" },
            new() { Id = 2, Name = "Jane", Email = "jane@test.com" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

        var result = await _service.GetAllStudentsAsync();

        result.Should().HaveCount(2);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ShouldReturnStudent_WhenExists()
    {
        var student = new Student { Id = 1, Name = "John", Email = "john@test.com" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

        var result = await _service.GetStudentByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateStudentAsync_ShouldReturnCreatedStudent()
    {
        var student = new Student { Name = "John", Email = "john@test.com" };
        _mockRepository.Setup(r => r.AddAsync(student)).ReturnsAsync(student);

        var result = await _service.CreateStudentAsync(student);

        result.Should().Be(student);
        _mockRepository.Verify(r => r.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task UpdateStudentAsync_ShouldReturnUpdatedStudent()
    {
        var student = new Student { Id = 1, Name = "John Updated", Email = "john@test.com" };
        _mockRepository.Setup(r => r.UpdateAsync(student)).ReturnsAsync(student);

        var result = await _service.UpdateStudentAsync(student);

        result.Should().Be(student);
        _mockRepository.Verify(r => r.UpdateAsync(student), Times.Once);
    }

    [Fact]
    public async Task DeleteStudentAsync_ShouldReturnTrue_WhenDeleted()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteStudentAsync(1);

        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
