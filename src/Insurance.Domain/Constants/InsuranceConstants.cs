namespace Insurance.Domain.Constants;

/// <summary>
/// Constantes utilizadas no cálculo de seguro
/// </summary>
public static class InsuranceConstants
{
    /// <summary>
    /// Taxa de risco base aplicada ao valor do veículo (2.5%)
    /// </summary>
    public const decimal BaseRiskRate = 0.025m;

    /// <summary>
    /// Margem de segurança aplicada ao cálculo (3%)
    /// </summary>
    public const decimal SafetyMargin = 0.03m;

    /// <summary>
    /// Margem de lucro aplicada ao cálculo (5%)
    /// </summary>
    public const decimal ProfitMargin = 0.05m;
}
