namespace Insurance.Domain.Entities;

/// <summary>
/// Entidade que representa um Segurado
/// </summary>
public class Insured
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Cpf { get; set; } = string.Empty;
    
    public int Age { get; set; }
    
    // Navigation property
    public ICollection<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
}
