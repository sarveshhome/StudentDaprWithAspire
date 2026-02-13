using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentDaprWithAspire.Domain.Interfaces;
using StudentDaprWithAspire.Infrastructure.Data;
using StudentDaprWithAspire.Infrastructure.Repositories;

namespace StudentDaprWithAspire.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("StudentDb"));
        
        services.AddScoped<IStudentRepository, StudentRepository>();
        
        return services;
    }
}