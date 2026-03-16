using Insurance.Domain.Entities;

namespace Insurance.Domain.Interfaces;

/// <summary>
/// Repositório para operações com segurados
/// </summary>
public interface IInsuredRepository
{
    Task<Insured?> GetByCpfAsync(string cpf);
    Task<Insured> AddAsync(Insured insured);
}
