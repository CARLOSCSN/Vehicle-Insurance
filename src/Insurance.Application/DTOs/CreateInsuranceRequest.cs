namespace Insurance.Application.DTOs;

/// <summary>
/// Request para criação de uma apólice de seguro
/// </summary>
public class CreateInsuranceRequest
{
    public decimal VehicleValue { get; set; }
    public string VehicleModel { get; set; } = string.Empty;
    public string InsuredCpf { get; set; } = string.Empty;
}
