namespace Insurance.Domain.Entities;

/// <summary>
/// Entidade que representa uma Apólice de Seguro
/// </summary>
public class InsurancePolicy
{
    public int Id { get; set; }
    
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    
    public int InsuredId { get; set; }
    public Insured Insured { get; set; } = null!;
    
    /// <summary>
    /// Taxa de Risco (%)
    /// </summary>
    public decimal RiskRate { get; set; }
    
    /// <summary>
    /// Prêmio de Risco
    /// </summary>
    public decimal RiskPremium { get; set; }
    
    /// <summary>
    /// Prêmio Puro (com margem de segurança)
    /// </summary>
    public decimal PurePremium { get; set; }
    
    /// <summary>
    /// Prêmio Comercial (com lucro)
    /// </summary>
    public decimal CommercialPremium { get; set; }
    
    /// <summary>
    /// Valor Final do Seguro
    /// </summary>
    public decimal InsuranceValue { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
