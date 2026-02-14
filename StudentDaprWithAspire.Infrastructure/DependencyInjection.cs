using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentDaprWithAspire.Domain.Interfaces;
using StudentDaprWithAspire.Infrastructure.Data;
using StudentDaprWithAspire.Infrastructure.Repositories;

namespace StudentDaprWithAspire.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Database=StudentDb;Integrated Security=true;TrustServerCertificate=true;";
        
        services.AddSingleton(new SqlConnectionFactory(connectionString));
        services.AddScoped<IStudentRepository, StudentRepository>();
        
        return services;
    }
}