using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using StudentDaprWithAspire.Application.Interfaces;
using StudentDaprWithAspire.Domain.Entities;

namespace StudentDaprWithAspire.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly DaprClient _daprClient;

    public StudentsController(IStudentService studentService, DaprClient daprClient)
    {
        _studentService = studentService;
        _daprClient = daprClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAll()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Create(Student student)
    {
        var created = await _studentService.CreateStudentAsync(student);
        await _daprClient.PublishEventAsync("pubsub", "student-created", created);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Student>> Update(int id, Student student)
    {
        if (id != student.Id) return BadRequest();
        var updated = await _studentService.UpdateStudentAsync(student);
        await _daprClient.PublishEventAsync("pubsub", "student-updated", updated);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        if (!result) return NotFound();
        await _daprClient.PublishEventAsync("pubsub", "student-deleted", new { Id = id });
        return NoContent();
    }
}