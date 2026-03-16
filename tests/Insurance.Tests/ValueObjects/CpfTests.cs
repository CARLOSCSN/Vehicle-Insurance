using FluentAssertions;
using Insurance.Domain.ValueObjects;

namespace Insurance.Tests.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Cpf
/// </summary>
public class CpfTests
{
    [Fact]
    public void Create_WithValidCpf_ShouldReturnCpfInstance()
    {
        // Arrange
        var validCpf = "12345678900";

        // Act
        var cpf = Cpf.Create(validCpf);

        // Assert
        cpf.Should().NotBeNull();
        cpf.Value.Should().Be("12345678900");
    }

    [Fact]
    public void Create_WithCpfWithSpecialCharacters_ShouldNormalizeAndCreate()
    {
        // Arrange
        var cpfWithMask = "123.456.789-00";

        // Act
        var cpf = Cpf.Create(cpfWithMask);

        // Assert
        cpf.Value.Should().Be("12345678900");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithEmptyCpf_ShouldThrowArgumentException(string invalidCpf)
    {
        // Act
        Action act = () => Cpf.Create(invalidCpf);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF não pode ser vazio.*");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345678")]
    [InlineData("123456789001")]
    public void Create_WithInvalidLength_ShouldThrowArgumentException(string invalidCpf)
    {
        // Act
        Action act = () => Cpf.Create(invalidCpf);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF inválido.*");
    }

    [Fact]
    public void TryCreate_WithValidCpf_ShouldReturnCpfInstance()
    {
        // Arrange
        var validCpf = "12345678900";

        // Act
        var cpf = Cpf.TryCreate(validCpf);

        // Assert
        cpf.Should().NotBeNull();
        cpf!.Value.Should().Be("12345678900");
    }

    [Fact]
    public void TryCreate_WithInvalidCpf_ShouldReturnNull()
    {
        // Arrange
        var invalidCpf = "123";

        // Act
        var cpf = Cpf.TryCreate(invalidCpf);

        // Assert
        cpf.Should().BeNull();
    }

    [Theory]
    [InlineData("123.456.789-00", "12345678900")]
    [InlineData("123 456 789 00", "12345678900")]
    [InlineData("12345678900", "12345678900")]
    [InlineData("abc123def456ghi789jkl00", "12345678900")]
    public void Normalize_ShouldRemoveNonNumericCharacters(string input, string expected)
    {
        // Act
        var result = Cpf.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Equals_WithSameCpf_ShouldReturnTrue()
    {
        // Arrange
        var cpf1 = Cpf.Create("12345678900");
        var cpf2 = Cpf.Create("12345678900");

        // Act & Assert
        cpf1.Should().Be(cpf2);
        (cpf1 == cpf2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentCpf_ShouldReturnFalse()
    {
        // Arrange
        var cpf1 = Cpf.Create("12345678900");
        var cpf2 = Cpf.Create("98765432100");

        // Act & Assert
        cpf1.Should().NotBe(cpf2);
        (cpf1 != cpf2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnCpfValue()
    {
        // Arrange
        var cpf = Cpf.Create("12345678900");

        // Act
        var result = cpf.ToString();

        // Assert
        result.Should().Be("12345678900");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var cpf = Cpf.Create("12345678900");

        // Act
        string cpfString = cpf;

        // Assert
        cpfString.Should().Be("12345678900");
    }
}
