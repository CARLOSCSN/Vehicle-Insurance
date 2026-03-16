namespace Insurance.Application.DTOs;

/// <summary>
/// Resultado do cálculo de seguro para retorno da API
/// </summary>
public class InsuranceCalculationResult
{
    public decimal RiskRate { get; set; }
    public decimal RiskPremium { get; set; }
    public decimal PurePremium { get; set; }
    public decimal CommercialPremium { get; set; }
    public decimal InsuranceValue { get; set; }
}
