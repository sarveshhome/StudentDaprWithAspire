using Microsoft.EntityFrameworkCore;
using StudentDaprWithAspire.Domain.Entities;
using StudentDaprWithAspire.Domain.Interfaces;
using StudentDaprWithAspire.Infrastructure.Data;

namespace StudentDaprWithAspire.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync() => await _context.Students.ToListAsync();

    public async Task<Student?> GetByIdAsync(int id) => await _context.Students.FindAsync(id);

    public async Task<Student> AddAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;
        
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}