using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using StudentDaprWithAspire.API.Controllers;
using StudentDaprWithAspire.Application.Interfaces;
using StudentDaprWithAspire.Domain.Entities;

namespace StudentDaprWithAspire.Tests.Controllers;

public class StudentsControllerTests
{
    private readonly Mock<IStudentService> _mockService;
    private readonly Mock<DaprClient> _mockDaprClient;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _mockService = new Mock<IStudentService>();
        _mockDaprClient = new Mock<DaprClient>();
        _controller = new StudentsController(_mockService.Object, _mockDaprClient.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithStudents()
    {
        var students = new List<Student>
        {
            new() { Id = 1, Name = "John", Email = "john@test.com" }
        };
        _mockService.Setup(s => s.GetAllStudentsAsync()).ReturnsAsync(students);

        var result = await _controller.GetAll();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(students);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenStudentExists()
    {
        var student = new Student { Id = 1, Name = "John", Email = "john@test.com" };
        _mockService.Setup(s => s.GetStudentByIdAsync(1)).ReturnsAsync(student);

        var result = await _controller.GetById(1);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(student);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        _mockService.Setup(s => s.GetStudentByIdAsync(1)).ReturnsAsync((Student?)null);

        var result = await _controller.GetById(1);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var student = new Student { Id = 1, Name = "John", Email = "john@test.com" };
        _mockService.Setup(s => s.CreateStudentAsync(It.IsAny<Student>())).ReturnsAsync(student);

        var result = await _controller.Create(student);

        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().BeEquivalentTo(student);
        _mockDaprClient.Verify(d => d.PublishEventAsync("pubsub", "student-created", student, default), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenIdMatches()
    {
        var student = new Student { Id = 1, Name = "John", Email = "john@test.com" };
        _mockService.Setup(s => s.UpdateStudentAsync(student)).ReturnsAsync(student);

        var result = await _controller.Update(1, student);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(student);
        _mockDaprClient.Verify(d => d.PublishEventAsync("pubsub", "student-updated", student, default), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        var student = new Student { Id = 2, Name = "John", Email = "john@test.com" };

        var result = await _controller.Update(1, student);

        result.Result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenDeleted()
    {
        _mockService.Setup(s => s.DeleteStudentAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
        _mockDaprClient.Verify(d => d.PublishEventAsync("pubsub", "student-deleted", It.IsAny<object>(), default), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        _mockService.Setup(s => s.DeleteStudentAsync(1)).ReturnsAsync(false);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NotFoundResult>();
    }
}
