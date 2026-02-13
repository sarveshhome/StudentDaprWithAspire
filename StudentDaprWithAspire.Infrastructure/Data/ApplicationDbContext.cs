using Microsoft.EntityFrameworkCore;
using StudentDaprWithAspire.Domain.Entities;

namespace StudentDaprWithAspire.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
}