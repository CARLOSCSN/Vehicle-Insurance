using Insurance.Application.DTOs;
using Insurance.Domain.Entities;

namespace Insurance.Application.Mappers;

/// <summary>
/// Mapper para conversão de InsurancePolicy para DTOs
/// Elimina duplicação de código de mapeamento
/// </summary>
public static class InsurancePolicyMapper
{
    public static InsuranceResponse ToResponse(this InsurancePolicy policy)
    {
        return new InsuranceResponse
        {
            Id = policy.Id,
            Insured = new InsuredDto
            {
                Id = policy.Insured.Id,
                Name = policy.Insured.Name,
                Cpf = policy.Insured.Cpf,
                Age = policy.Insured.Age
            },
            Vehicle = new VehicleDto
            {
                Id = policy.Vehicle.Id,
                Value = policy.Vehicle.Value,
                Model = policy.Vehicle.Model
            },
            RiskRate = policy.RiskRate,
            RiskPremium = policy.RiskPremium,
            PurePremium = policy.PurePremium,
            CommercialPremium = policy.CommercialPremium,
            InsuranceValue = policy.InsuranceValue,
            CreatedAt = policy.CreatedAt
        };
    }
}
