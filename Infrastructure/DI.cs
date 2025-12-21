using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Services;
using Infrastructure.SQLServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register EF Core Repositories (Scoped lifetime for DbContext)
        services.AddScoped<IInputRepository, SqlServerInputRepository>();
        services.AddScoped<IOutputRepository, SqlServerOutputRepository>();

        // Register Session User Service
        services.AddScoped<ISessionUserService, SessionUserService>();

        // Keep InMemory repositories for testing if needed
        // services.AddSingleton<IInputRepository, InMemoryInputRepository>();
        // services.AddSingleton<IOutputRepository, InMemoryOutputRepository>();

        return services;

    }
}
