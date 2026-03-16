using Insurance.Application.DTOs;

namespace Insurance.Tests.Fixtures;

/// <summary>
/// Dados de teste reutilizáveis
/// </summary>
public static class TestData
{
    public static CreateInsuranceRequest ValidRequest => new()
    {
        VehicleValue = 10000m,
        VehicleModel = "Honda Civic",
        InsuredCpf = "12345678900"
    };

    public static CreateInsuranceRequest InvalidRequestZeroValue => new()
    {
        VehicleValue = 0m,
        VehicleModel = "Honda Civic",
        InsuredCpf = "12345678900"
    };

    public static CreateInsuranceRequest InvalidRequestEmptyModel => new()
    {
        VehicleValue = 10000m,
        VehicleModel = "",
        InsuredCpf = "12345678900"
    };

    public static CreateInsuranceRequest InvalidRequestInvalidCpf => new()
    {
        VehicleValue = 10000m,
        VehicleModel = "Honda Civic",
        InsuredCpf = "123"
    };
}
