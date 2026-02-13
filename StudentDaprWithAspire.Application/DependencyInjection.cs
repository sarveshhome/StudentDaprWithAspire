using Microsoft.Extensions.DependencyInjection;
using StudentDaprWithAspire.Application.Interfaces;
using StudentDaprWithAspire.Application.Services;

namespace StudentDaprWithAspire.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}