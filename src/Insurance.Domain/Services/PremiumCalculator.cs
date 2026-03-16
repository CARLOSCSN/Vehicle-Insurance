using Insurance.Domain.Constants;
using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Services;

/// <summary>
/// Serviço de domínio responsável pelo cálculo de prêmios de seguro
/// </summary>
public class PremiumCalculator
{
    /// <summary>
    /// Calcula todos os valores do seguro baseado no valor do veículo
    /// Fórmulas:
    /// - RiskRate = 2.5% (constante)
    /// - RiskPremium = RiskRate * VehicleValue
    /// - PurePremium = RiskPremium * (1 + SafetyMargin)
    /// - CommercialPremium = PurePremium * (1 + ProfitMargin)
    /// </summary>
    public static PremiumCalculationResult Calculate(Money vehicleValue)
    {
        if (vehicleValue.Amount <= 0)
        {
            throw new ArgumentException("O valor do veículo deve ser maior que zero.", nameof(vehicleValue));
        }

        var riskRate = InsuranceConstants.BaseRiskRate;
        var riskPremium = vehicleValue.Multiply(riskRate);
        var purePremium = riskPremium.ApplyPercentage(InsuranceConstants.SafetyMargin);
        var commercialPremium = purePremium.ApplyPercentage(InsuranceConstants.ProfitMargin);

        return new PremiumCalculationResult(
            RiskRate: riskRate,
            RiskPremium: riskPremium,
            PurePremium: purePremium,
            CommercialPremium: commercialPremium,
            InsuranceValue: commercialPremium
        );
    }
}

/// <summary>
/// Resultado do cálculo de prêmio
/// </summary>
public sealed record PremiumCalculationResult(
    decimal RiskRate,
    Money RiskPremium,
    Money PurePremium,
    Money CommercialPremium,
    Money InsuranceValue
);
