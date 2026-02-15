using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Domain.Interfaces;

namespace StudentDaprWithAspire.Infrastructure.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();
    private int _nextId = 1;

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Student>>(_students.ToList());
    }

    public Task<Student?> GetByIdAsync(int id)
    {
        return Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
    }

    public Task<Student> AddAsync(Student student)
    {
        student.Id = _nextId++;
        _students.Add(student);
        return Task.FromResult(student);
    }

    public Task<Student> UpdateAsync(Student student)
    {
        var existing = _students.FirstOrDefault(s => s.Id == student.Id);
        if (existing != null)
        {
            existing.Name = student.Name;
            existing.Email = student.Email;
            existing.Age = student.Age;
        }
        return Task.FromResult(student);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student != null)
        {
            _students.Remove(student);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
