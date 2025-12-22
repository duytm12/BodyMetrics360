using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Domain Services
        services.AddScoped<IGetBMI, GetBMI>();
        services.AddScoped<IGetBFP, GetBFP>();
        services.AddScoped<IGetLBM, GetLBM>();
        services.AddScoped<IGetWtHR, GetWtHR>();
        services.AddScoped<IGetRecommendation, GetRecommendation>();

        // Register Application Use Cases
        services.AddScoped<UseCases.CalculateBMIUseCase>();
        services.AddScoped<UseCases.CalculateBFPUseCase>();
        services.AddScoped<UseCases.CalculateLBMUseCase>();
        services.AddScoped<UseCases.CalculateWtHRUseCase>();

        return services;
    }
}
