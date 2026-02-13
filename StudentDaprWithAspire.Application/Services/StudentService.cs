using StudentDaprWithAspire.Application.Interfaces;
using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Domain.Interfaces;

namespace StudentDaprWithAspire.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync() => await _repository.GetAllAsync();

    public async Task<Student?> GetStudentByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Student> CreateStudentAsync(Student student) => await _repository.AddAsync(student);

    public async Task<Student> UpdateStudentAsync(Student student) => await _repository.UpdateAsync(student);

    public async Task<bool> DeleteStudentAsync(int id) => await _repository.DeleteAsync(id);
}