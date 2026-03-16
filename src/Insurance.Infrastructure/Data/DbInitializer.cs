using Insurance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.Data;

/// <summary>
/// Inicializador do banco de dados com seed data
/// </summary>
public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Garantir que o banco existe e está atualizado
            context.Database.EnsureCreated();
            logger.LogInformation("Banco de dados inicializado com sucesso");

            // Verificar se já existem dados
            if (context.InsurancePolicies.Any())
            {
                logger.LogInformation("Banco de dados já contém dados");
                return;
            }

            // Seed data de exemplo
            SeedData(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inicializar o banco de dados");
            throw;
        }
    }

    private static void SeedData(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Populando banco de dados com dados de exemplo");

        // Criar segurados de exemplo
        var insured1 = new Insured
        {
            Name = "Carlos Santana",
            Cpf = "12345678900",
            Age = 34
        };

        var insured2 = new Insured
        {
            Name = "Maria Silva",
            Cpf = "98765432100",
            Age = 28
        };

        context.Insureds.AddRange(insured1, insured2);
        context.SaveChanges();

        // Criar veículos de exemplo
        var vehicle1 = new Vehicle
        {
            Model = "Honda Civic",
            Value = 10000m
        };

        var vehicle2 = new Vehicle
        {
            Model = "Toyota Corolla",
            Value = 50000m
        };

        context.Vehicles.AddRange(vehicle1, vehicle2);
        context.SaveChanges();

        // Criar apólices de exemplo
        var policy1 = new InsurancePolicy
        {
            VehicleId = vehicle1.Id,
            InsuredId = insured1.Id,
            RiskRate = 0.025m,
            RiskPremium = 250.00m,
            PurePremium = 257.50m,
            CommercialPremium = 270.37m,
            InsuranceValue = 270.37m,
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        var policy2 = new InsurancePolicy
        {
            VehicleId = vehicle2.Id,
            InsuredId = insured2.Id,
            RiskRate = 0.025m,
            RiskPremium = 1250.00m,
            PurePremium = 1287.50m,
            CommercialPremium = 1351.88m,
            InsuranceValue = 1351.88m,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };

        context.InsurancePolicies.AddRange(policy1, policy2);
        context.SaveChanges();

        logger.LogInformation("Dados de exemplo inseridos: {Count} apólices", context.InsurancePolicies.Count());
    }
}
