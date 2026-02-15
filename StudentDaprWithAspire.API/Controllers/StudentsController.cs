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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Student>>> GetAll()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Student>> Create(Student student)
    {
        var created = await _studentService.CreateStudentAsync(student);
        await _daprClient.PublishEventAsync("pubsub", "student-created", created);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [IgnoreAntiforgeryToken]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> Update(int id, Student student)
    {
        if (id != student.Id) return BadRequest();
        var updated = await _studentService.UpdateStudentAsync(student);
        if (updated == null) return NotFound();
        await _daprClient.PublishEventAsync("pubsub", "student-updated", updated);
        return Ok(updated);
    }

    [HttpPatch("{id}")]
    [IgnoreAntiforgeryToken]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> Patch(int id, [FromBody] Dictionary<string, object> updates)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();

        foreach (var update in updates)
        {
            var property = typeof(Student).GetProperty(update.Key, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                var value = Convert.ChangeType(update.Value, property.PropertyType);
                property.SetValue(student, value);
            }
        }

        var updated = await _studentService.UpdateStudentAsync(student);
        await _daprClient.PublishEventAsync("pubsub", "student-updated", updated);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [IgnoreAntiforgeryToken]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        if (!result) return NotFound();
        await _daprClient.PublishEventAsync("pubsub", "student-deleted", new { Id = id });
        return NoContent();
    }
}