using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Application.Mappers;
using Insurance.Domain.Entities;
using Insurance.Domain.Interfaces;
using Insurance.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

/// <summary>
/// Serviço de aplicação que orquestra as operações de seguro
/// </summary>
public class InsuranceApplicationService : IInsuranceApplicationService
{
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IInsuredRepository _insuredRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IExternalInsuredService _externalInsuredService;
    private readonly IInsuranceCalculatorService _calculatorService;
    private readonly ILogger<InsuranceApplicationService> _logger;

    public InsuranceApplicationService(
        IInsuranceRepository insuranceRepository,
        IInsuredRepository insuredRepository,
        IVehicleRepository vehicleRepository,
        IExternalInsuredService externalInsuredService,
        IInsuranceCalculatorService calculatorService,
        ILogger<InsuranceApplicationService> logger)
    {
        _insuranceRepository = insuranceRepository;
        _insuredRepository = insuredRepository;
        _vehicleRepository = vehicleRepository;
        _externalInsuredService = externalInsuredService;
        _calculatorService = calculatorService;
        _logger = logger;
    }

    public async Task<InsuranceResponse> CreateInsuranceAsync(CreateInsuranceRequest request)
    {
        _logger.LogInformation("Criando seguro para CPF: {Cpf}", request.InsuredCpf);

        // 1. Buscar dados do segurado no serviço externo
        var externalInsuredData = await _externalInsuredService.GetInsuredByCpfAsync(request.InsuredCpf);
        if (externalInsuredData == null)
        {
            throw new InvalidOperationException($"Segurado com CPF {request.InsuredCpf} não encontrado no serviço externo.");
        }

        // 2. Verificar se o segurado já existe no banco, senão criar
        var normalizedCpf = Cpf.Normalize(request.InsuredCpf);
        var insured = await _insuredRepository.GetByCpfAsync(normalizedCpf);
        if (insured == null)
        {
            insured = new Insured
            {
                Name = externalInsuredData.Name,
                Cpf = externalInsuredData.Cpf,
                Age = externalInsuredData.Age
            };
            insured = await _insuredRepository.AddAsync(insured);
            _logger.LogInformation("Segurado criado: {Name} (CPF: {Cpf})", insured.Name, insured.Cpf);
        }

        // 3. Criar o veículo
        var vehicle = new Vehicle
        {
            Value = request.VehicleValue,
            Model = request.VehicleModel
        };
        vehicle = await _vehicleRepository.AddAsync(vehicle);
        _logger.LogInformation("Veículo criado: {Model} (Valor: {Value})", vehicle.Model, vehicle.Value);

        // 4. Calcular valores do seguro
        var calculation = _calculatorService.Calculate(request.VehicleValue);

        // 5. Criar a apólice
        var insurancePolicy = new InsurancePolicy
        {
            VehicleId = vehicle.Id,
            InsuredId = insured.Id,
            RiskRate = calculation.RiskRate,
            RiskPremium = calculation.RiskPremium,
            PurePremium = calculation.PurePremium,
            CommercialPremium = calculation.CommercialPremium,
            InsuranceValue = calculation.InsuranceValue,
            CreatedAt = DateTime.UtcNow
        };
        insurancePolicy = await _insuranceRepository.AddAsync(insurancePolicy);
        _logger.LogInformation("Apólice criada: ID {Id}, Valor do Seguro: {InsuranceValue}", 
            insurancePolicy.Id, insurancePolicy.InsuranceValue);

        // 6. Retornar response usando mapper
        return insurancePolicy.ToResponse();
    }

    public async Task<InsuranceResponse?> GetInsuranceByIdAsync(int id)
    {
        _logger.LogInformation("Buscando seguro com ID: {Id}", id);

        var insurance = await _insuranceRepository.GetByIdAsync(id);
        
        return insurance?.ToResponse();
    }

    public async Task<IEnumerable<InsuranceResponse>> GetAllInsurancesAsync()
    {
        _logger.LogInformation("Listando todas as apólices de seguro");

        var insurances = await _insuranceRepository.GetAllAsync();
        
        return insurances.Select(i => i.ToResponse());
    }

    public async Task<InsuranceReportResponse> GetReportAsync()
    {
        _logger.LogInformation("Gerando relatório de seguros");

        var totalPolicies = await _insuranceRepository.CountAsync();
        var averageInsuranceValue = totalPolicies > 0 
            ? await _insuranceRepository.GetAverageInsuranceValueAsync() 
            : 0;
        var averageVehicleValue = totalPolicies > 0 
            ? await _insuranceRepository.GetAverageVehicleValueAsync() 
            : 0;

        return new InsuranceReportResponse
        {
            AverageInsuranceValue = Math.Round(averageInsuranceValue, 2),
            AverageVehicleValue = Math.Round(averageVehicleValue, 2),
            TotalPolicies = totalPolicies
        };
    }
}
