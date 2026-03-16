namespace Insurance.Domain.Interfaces;

/// <summary>
/// Interface para serviço externo de consulta de segurados
/// </summary>
public interface IExternalInsuredService
{
    Task<ExternalInsuredDto?> GetInsuredByCpfAsync(string cpf);
}

/// <summary>
/// DTO simples para dados retornados pelo serviço externo
/// </summary>
public record ExternalInsuredDto(string Name, string Cpf, int Age);
