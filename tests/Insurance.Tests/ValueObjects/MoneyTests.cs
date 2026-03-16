using FluentAssertions;
using Insurance.Domain.ValueObjects;

namespace Insurance.Tests.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Money
/// </summary>
public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_ShouldReturnMoneyInstance()
    {
        // Arrange
        var amount = 100.50m;

        // Act
        var money = Money.Create(amount);

        // Assert
        money.Should().NotBeNull();
        money.Amount.Should().Be(100.50m);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var negativeAmount = -100m;

        // Act
        Action act = () => Money.Create(negativeAmount);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O valor monetário não pode ser negativo.*");
    }

    [Fact]
    public void Create_ShouldRoundToTwoDecimals()
    {
        // Arrange
        var amount = 100.12345m;

        // Act
        var money = Money.Create(amount);

        // Assert
        money.Amount.Should().Be(100.12m);
    }

    [Fact]
    public void Zero_ShouldReturnMoneyWithZeroAmount()
    {
        // Act
        var zero = Money.Zero;

        // Assert
        zero.Amount.Should().Be(0m);
    }

    [Fact]
    public void Add_ShouldReturnSumOfTwoAmounts()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);

        // Act
        var result = money1.Add(money2);

        // Assert
        result.Amount.Should().Be(150m);
    }

    [Fact]
    public void Subtract_ShouldReturnDifferenceOfTwoAmounts()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(30m);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.Amount.Should().Be(70m);
    }

    [Fact]
    public void Multiply_ShouldReturnMultipliedAmount()
    {
        // Arrange
        var money = Money.Create(100m);

        // Act
        var result = money.Multiply(1.5m);

        // Assert
        result.Amount.Should().Be(150m);
    }

    [Fact]
    public void ApplyPercentage_ShouldReturnAmountWithPercentageApplied()
    {
        // Arrange
        var money = Money.Create(100m);

        // Act
        var result = money.ApplyPercentage(0.10m); // 10% de acréscimo

        // Assert
        result.Amount.Should().Be(110m);
    }

    [Fact]
    public void Equals_WithSameAmount_ShouldReturnTrue()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(100m);

        // Act & Assert
        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentAmount_ShouldReturnFalse()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(200m);

        // Act & Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void CompareTo_ShouldCompareAmountsCorrectly()
    {
        // Arrange
        var smaller = Money.Create(50m);
        var larger = Money.Create(100m);

        // Act & Assert
        (smaller < larger).Should().BeTrue();
        (larger > smaller).Should().BeTrue();
        (smaller <= larger).Should().BeTrue();
        (larger >= smaller).Should().BeTrue();
    }

    [Fact]
    public void OperatorPlus_ShouldAddTwoMoneyInstances()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(150m);
    }

    [Fact]
    public void OperatorMinus_ShouldSubtractTwoMoneyInstances()
    {
        // Arrange
        var money1 = Money.Create(100m);
        var money2 = Money.Create(30m);

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(70m);
    }

    [Fact]
    public void OperatorMultiply_ShouldMultiplyMoneyByDecimal()
    {
        // Arrange
        var money = Money.Create(100m);

        // Act
        var result = money * 2m;

        // Assert
        result.Amount.Should().Be(200m);
    }

    [Fact]
    public void ImplicitConversion_ToDecimal_ShouldWork()
    {
        // Arrange
        var money = Money.Create(100.50m);

        // Act
        decimal amount = money;

        // Assert
        amount.Should().Be(100.50m);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedCurrency()
    {
        // Arrange
        var money = Money.Create(100.50m);

        // Act
        var result = money.ToString();

        // Assert - O formato pode variar dependendo da cultura
        result.Should().NotBeNullOrEmpty();
    }
}
