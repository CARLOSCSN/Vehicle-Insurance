using Insurance.Application.DTOs;

namespace Insurance.Application.Interfaces;

/// <summary>
/// Serviço responsável pelo cálculo de seguros
/// </summary>
public interface IInsuranceCalculatorService
{
    /// <summary>
    /// Calcula todos os valores do seguro baseado no valor do veículo
    /// </summary>
    InsuranceCalculationResult Calculate(decimal vehicleValue);
}
