using Insurance.Application.DTOs;

namespace Insurance.Application.Interfaces;

/// <summary>
/// Serviço de aplicação para operações de seguro
/// </summary>
public interface IInsuranceApplicationService
{
    Task<InsuranceResponse> CreateInsuranceAsync(CreateInsuranceRequest request);
    Task<InsuranceResponse?> GetInsuranceByIdAsync(int id);
    Task<IEnumerable<InsuranceResponse>> GetAllInsurancesAsync();
    Task<InsuranceReportResponse> GetReportAsync();
}
