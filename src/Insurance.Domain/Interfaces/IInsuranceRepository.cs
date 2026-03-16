using Insurance.Domain.Entities;

namespace Insurance.Domain.Interfaces;

/// <summary>
/// Repositório para operações com apólices de seguro
/// </summary>
public interface IInsuranceRepository
{
    Task<InsurancePolicy?> GetByIdAsync(int id);
    Task<IEnumerable<InsurancePolicy>> GetAllAsync();
    Task<InsurancePolicy> AddAsync(InsurancePolicy insurance);
    Task<int> CountAsync();
    Task<decimal> GetAverageInsuranceValueAsync();
    Task<decimal> GetAverageVehicleValueAsync();
}
