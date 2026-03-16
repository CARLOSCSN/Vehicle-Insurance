using Insurance.Application.Interfaces;
using Insurance.Application.Services;
using Insurance.Domain.Interfaces;
using Insurance.Infrastructure.Data;
using Insurance.Infrastructure.ExternalServices;
using Insurance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Infrastructure;

/// <summary>
/// Configuração de Dependency Injection para Infrastructure Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database - SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));

        // Repositories
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IInsuredRepository, InsuredRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        // External Services
        services.AddScoped<IExternalInsuredService, MockExternalInsuredService>();

        // Application Services
        services.AddScoped<IInsuranceCalculatorService, InsuranceCalculatorService>();
        services.AddScoped<IInsuranceApplicationService, InsuranceApplicationService>();

        return services;
    }
}
