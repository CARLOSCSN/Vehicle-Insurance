using Insurance.Domain.Interfaces;
using Insurance.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.ExternalServices;

/// <summary>
/// Mock do serviço externo de consulta de segurados
/// Simula uma API REST externa
/// </summary>
public class MockExternalInsuredService : IExternalInsuredService
{
    private readonly ILogger<MockExternalInsuredService> _logger;

    // Dados mockados de segurados
    private readonly Dictionary<string, ExternalInsuredDto> _mockData = new()
    {
        ["12345678900"] = new("Carlos Santana", "12345678900", 34),
        ["98765432100"] = new("Maria Silva", "98765432100", 28),
        ["11122233344"] = new("João Santos", "11122233344", 45),
        ["55566677788"] = new("Ana Costa", "55566677788", 32)
    };

    public MockExternalInsuredService(ILogger<MockExternalInsuredService> logger)
    {
        _logger = logger;
    }

    public async Task<ExternalInsuredDto?> GetInsuredByCpfAsync(string cpf)
    {
        _logger.LogInformation("Consultando serviço externo para CPF: {Cpf}", cpf);

        // Simular delay de rede
        await Task.Delay(100);

        // Normalizar CPF
        var normalizedCpf = Cpf.Normalize(cpf);

        if (_mockData.TryGetValue(normalizedCpf, out var insuredData))
        {
            _logger.LogInformation("Segurado encontrado no serviço externo: {Name}", insuredData.Name);
            return insuredData;
        }

        _logger.LogWarning("Segurado com CPF {Cpf} não encontrado no serviço externo", cpf);
        return null;
    }
}
