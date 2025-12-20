using Domain.Interfaces;
using Infrastructure.InMemory;
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

        // Keep InMemory repositories for testing if needed
        // services.AddSingleton<IInputRepository, InMemoryInputRepository>();
        // services.AddSingleton<IOutputRepository, InMemoryOutputRepository>();

        return services;

    }
}
