using FluentAssertions;
using Insurance.Application.Interfaces;
using Insurance.Application.Services;

namespace Insurance.Tests.Services;

/// <summary>
/// Testes unitários para o InsuranceCalculatorService
/// </summary>
public class InsuranceCalculatorServiceTests
{
    private readonly IInsuranceCalculatorService _calculatorService;

    public InsuranceCalculatorServiceTests()
    {
        _calculatorService = new InsuranceCalculatorService();
    }

    [Fact]
    public void Calculate_WithVehicleValue10000_ShouldReturn270Point38()
    {
        // Arrange
        decimal vehicleValue = 10000m;
        decimal expectedInsuranceValue = 270.38m;

        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert
        result.Should().NotBeNull();
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Should().Be(250.00m);
        result.PurePremium.Should().Be(257.50m);
        result.CommercialPremium.Should().Be(270.38m);
        result.InsuranceValue.Should().Be(expectedInsuranceValue);
    }

    [Fact]
    public void Calculate_WithVehicleValue50000_ShouldReturn1351Point88()
    {
        // Arrange
        decimal vehicleValue = 50000m;
        decimal expectedRiskPremium = 1250.00m;
        decimal expectedPurePremium = 1287.50m;
        decimal expectedCommercialPremium = 1351.88m;

        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Should().Be(expectedRiskPremium);
        result.PurePremium.Should().Be(expectedPurePremium);
        result.CommercialPremium.Should().Be(expectedCommercialPremium);
        result.InsuranceValue.Should().Be(expectedCommercialPremium);
    }

    [Fact]
    public void Calculate_WithVehicleValue100000_ShouldReturn2703Point75()
    {
        // Arrange
        decimal vehicleValue = 100000m;
        decimal expectedRiskPremium = 2500.00m;
        decimal expectedPurePremium = 2575.00m;
        decimal expectedCommercialPremium = 2703.75m;

        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Should().Be(expectedRiskPremium);
        result.PurePremium.Should().Be(expectedPurePremium);
        result.CommercialPremium.Should().Be(expectedCommercialPremium);
        result.InsuranceValue.Should().Be(expectedCommercialPremium);
    }

    [Fact]
    public void Calculate_WithVehicleValue1000_ShouldReturnCorrectCalculation()
    {
        // Arrange
        decimal vehicleValue = 1000m;
        decimal expectedRiskPremium = 25.00m;
        decimal expectedPurePremium = 25.75m;
        decimal expectedCommercialPremium = 27.04m;

        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Should().Be(expectedRiskPremium);
        result.PurePremium.Should().Be(expectedPurePremium);
        result.CommercialPremium.Should().Be(expectedCommercialPremium);
        result.InsuranceValue.Should().Be(expectedCommercialPremium);
    }

    [Fact]
    public void Calculate_WithZeroValue_ShouldThrowArgumentException()
    {
        // Arrange
        decimal vehicleValue = 0m;

        // Act
        Action act = () => _calculatorService.Calculate(vehicleValue);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O valor do veículo deve ser maior que zero.*");
    }

    [Fact]
    public void Calculate_WithNegativeValue_ShouldThrowArgumentException()
    {
        // Arrange
        decimal vehicleValue = -1000m;

        // Act
        Action act = () => _calculatorService.Calculate(vehicleValue);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O valor monetário não pode ser negativo.*");
    }

    [Fact]
    public void Calculate_RiskRateShouldAlwaysBe2Point5Percent()
    {
        // Arrange & Act
        var result1 = _calculatorService.Calculate(10000m);
        var result2 = _calculatorService.Calculate(50000m);
        var result3 = _calculatorService.Calculate(100000m);

        // Assert - RiskRate deve sempre ser 2.5%
        result1.RiskRate.Should().Be(0.025m);
        result2.RiskRate.Should().Be(0.025m);
        result3.RiskRate.Should().Be(0.025m);
    }

    [Theory]
    [InlineData(10000, 270.38)]
    [InlineData(20000, 540.75)]
    [InlineData(30000, 811.12)]
    [InlineData(40000, 1081.50)]
    [InlineData(50000, 1351.88)]
    public void Calculate_WithDifferentValues_ShouldReturnCorrectInsuranceValue(
        decimal vehicleValue, decimal expectedInsurance)
    {
        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert
        result.InsuranceValue.Should().Be(expectedInsurance);
    }

    [Fact]
    public void Calculate_ShouldApplyCorrectMargins()
    {
        // Arrange
        decimal vehicleValue = 10000m;

        // Act
        var result = _calculatorService.Calculate(vehicleValue);

        // Assert - Verificar aplicação das margens
        // MARGEM_SEGURANCA = 3%
        decimal purePremiumExpected = result.RiskPremium * 1.03m;
        result.PurePremium.Should().Be(Math.Round(purePremiumExpected, 2));

        // LUCRO = 5%
        decimal commercialPremiumExpected = result.PurePremium * 1.05m;
        result.CommercialPremium.Should().Be(Math.Round(commercialPremiumExpected, 2));
    }
}
