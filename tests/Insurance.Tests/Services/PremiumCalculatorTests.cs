using FluentAssertions;
using Insurance.Domain.Services;
using Insurance.Domain.ValueObjects;

namespace Insurance.Tests.Services;

/// <summary>
/// Testes para o PremiumCalculator do domínio
/// </summary>
public class PremiumCalculatorTests
{
    [Fact]
    public void Calculate_WithVehicleValue10000_ShouldReturnCorrectValues()
    {
        // Arrange
        var vehicleValue = Money.Create(10000m);

        // Act
        var result = PremiumCalculator.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Amount.Should().Be(250.00m);
        result.PurePremium.Amount.Should().Be(257.50m);
        result.CommercialPremium.Amount.Should().Be(270.38m);
        result.InsuranceValue.Amount.Should().Be(270.38m);
    }

    [Fact]
    public void Calculate_WithVehicleValue50000_ShouldReturnCorrectValues()
    {
        // Arrange
        var vehicleValue = Money.Create(50000m);

        // Act
        var result = PremiumCalculator.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Amount.Should().Be(1250.00m);
        result.PurePremium.Amount.Should().Be(1287.50m);
        result.CommercialPremium.Amount.Should().Be(1351.88m);
        result.InsuranceValue.Amount.Should().Be(1351.88m);
    }

    [Fact]
    public void Calculate_WithVehicleValue100000_ShouldReturnCorrectValues()
    {
        // Arrange
        var vehicleValue = Money.Create(100000m);

        // Act
        var result = PremiumCalculator.Calculate(vehicleValue);

        // Assert
        result.RiskRate.Should().Be(0.025m);
        result.RiskPremium.Amount.Should().Be(2500.00m);
        result.PurePremium.Amount.Should().Be(2575.00m);
        result.CommercialPremium.Amount.Should().Be(2703.75m);
        result.InsuranceValue.Amount.Should().Be(2703.75m);
    }

    [Fact]
    public void Calculate_WithZeroValue_ShouldThrowArgumentException()
    {
        // Arrange
        var vehicleValue = Money.Zero;

        // Act
        Action act = () => PremiumCalculator.Calculate(vehicleValue);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O valor do veículo deve ser maior que zero.*");
    }

    [Theory]
    [InlineData(10000, 270.38)]
    [InlineData(20000, 540.75)]
    [InlineData(30000, 811.12)]
    [InlineData(40000, 1081.50)]
    [InlineData(50000, 1351.88)]
    public void Calculate_WithDifferentValues_ShouldReturnCorrectInsuranceValue(
        decimal vehicleAmount, decimal expectedInsurance)
    {
        // Arrange
        var vehicleValue = Money.Create(vehicleAmount);

        // Act
        var result = PremiumCalculator.Calculate(vehicleValue);

        // Assert
        result.InsuranceValue.Amount.Should().Be(expectedInsurance);
    }

    [Fact]
    public void Calculate_RiskRateShouldAlwaysBe2Point5Percent()
    {
        // Arrange & Act
        var result1 = PremiumCalculator.Calculate(Money.Create(10000m));
        var result2 = PremiumCalculator.Calculate(Money.Create(50000m));
        var result3 = PremiumCalculator.Calculate(Money.Create(100000m));

        // Assert
        result1.RiskRate.Should().Be(0.025m);
        result2.RiskRate.Should().Be(0.025m);
        result3.RiskRate.Should().Be(0.025m);
    }

    [Fact]
    public void Calculate_ShouldApplyCorrectMargins()
    {
        // Arrange
        var vehicleValue = Money.Create(10000m);

        // Act
        var result = PremiumCalculator.Calculate(vehicleValue);

        // Assert - Verificar aplicação das margens
        // SafetyMargin = 3%
        var expectedPurePremium = result.RiskPremium.ApplyPercentage(0.03m);
        result.PurePremium.Should().Be(expectedPurePremium);

        // ProfitMargin = 5%
        var expectedCommercialPremium = result.PurePremium.ApplyPercentage(0.05m);
        result.CommercialPremium.Should().Be(expectedCommercialPremium);
    }
}
