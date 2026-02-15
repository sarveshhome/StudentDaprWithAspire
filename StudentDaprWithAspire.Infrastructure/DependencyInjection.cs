using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentDaprWithAspire.Domain.Interfaces;
using StudentDaprWithAspire.Infrastructure.Repositories;

namespace StudentDaprWithAspire.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        return services;
    }
}