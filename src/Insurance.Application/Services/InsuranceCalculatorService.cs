using Insurance.Application.DTOs;
using Insurance.Application.Interfaces;
using Insurance.Domain.Services;
using Insurance.Domain.ValueObjects;

namespace Insurance.Application.Services;

/// <summary>
/// Implementação do serviço de cálculo de seguros
/// Delega para o PremiumCalculator do domínio
/// </summary>
public class InsuranceCalculatorService : IInsuranceCalculatorService
{
    public InsuranceCalculationResult Calculate(decimal vehicleValue)
    {
        var vehicleValueMoney = Money.Create(vehicleValue);
        var domainResult = PremiumCalculator.Calculate(vehicleValueMoney);

        return new InsuranceCalculationResult
        {
            RiskRate = domainResult.RiskRate,
            RiskPremium = domainResult.RiskPremium.Amount,
            PurePremium = domainResult.PurePremium.Amount,
            CommercialPremium = domainResult.CommercialPremium.Amount,
            InsuranceValue = domainResult.InsuranceValue.Amount
        };
    }
}
