using Domain.Interfaces;
using Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register Repository Implementations
        services.AddSingleton<IInputRepository, InMemoryInputRepository>();
        services.AddSingleton<IOutputRepository, InMemoryOutputRepository>();

        return services;

    }
}
