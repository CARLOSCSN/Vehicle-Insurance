namespace Insurance.Application.DTOs;

/// <summary>
/// Response com relatório de médias
/// </summary>
public class InsuranceReportResponse
{
    public decimal AverageInsuranceValue { get; set; }
    public decimal AverageVehicleValue { get; set; }
    public int TotalPolicies { get; set; }
}
