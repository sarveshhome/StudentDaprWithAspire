using StudentDaprWithAspire.Domain.Entities;

namespace StudentDaprWithAspire.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> CreateStudentAsync(Student student);
    Task<Student> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
}