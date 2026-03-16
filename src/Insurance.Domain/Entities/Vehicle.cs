namespace Insurance.Domain.Entities;

/// <summary>
/// Entidade que representa um Veículo
/// </summary>
public class Vehicle
{
    public int Id { get; set; }
    
    public decimal Value { get; set; }
    
    public string Model { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
}
