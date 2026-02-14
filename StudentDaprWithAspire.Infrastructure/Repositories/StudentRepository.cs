using Dapper;
using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Domain.Interfaces;
using StudentDaprWithAspire.Infrastructure.Data;

namespace StudentDaprWithAspire.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public StudentRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Student>("SELECT Id, Name, Email FROM Students");
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Student>(
            "SELECT Id, Name, Email FROM Students WHERE Id = @Id", new { Id = id });
    }

    public async Task<Student> AddAsync(Student student)
    {
        using var connection = _connectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Students (Name, Email) OUTPUT INSERTED.Id VALUES (@Name, @Email)", student);
        student.Id = id;
        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE Students SET Name = @Name, Email = @Email WHERE Id = @Id", student);
        return student;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync("DELETE FROM Students WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }
}