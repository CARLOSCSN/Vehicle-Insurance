namespace Insurance.Application.DTOs;

/// <summary>
/// Response com os detalhes completos de uma apólice
/// </summary>
public class InsuranceResponse
{
    public int Id { get; set; }
    
    // Dados do Segurado
    public InsuredDto Insured { get; set; } = new();
    
    // Dados do Veículo
    public VehicleDto Vehicle { get; set; } = new();
    
    // Campos de Cálculo
    public decimal RiskRate { get; set; }
    public decimal RiskPremium { get; set; }
    public decimal PurePremium { get; set; }
    public decimal CommercialPremium { get; set; }
    public decimal InsuranceValue { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class InsuredDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class VehicleDto
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public string Model { get; set; } = string.Empty;
}
