using FluentAssertions;
using FluentValidation.TestHelper;
using Insurance.Application.DTOs;
using Insurance.Application.Validators;

namespace Insurance.Tests.Validators;

/// <summary>
/// Testes para o CreateInsuranceRequestValidator
/// </summary>
public class CreateInsuranceRequestValidatorTests
{
    private readonly CreateInsuranceRequestValidator _validator;

    public CreateInsuranceRequestValidatorTests()
    {
        _validator = new CreateInsuranceRequestValidator();
    }

    [Fact]
    public void Validate_WithValidRequest_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = "Honda Civic",
            InsuredCpf = "12345678900"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Validate_WithInvalidVehicleValue_ShouldHaveValidationError(decimal invalidValue)
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = invalidValue,
            VehicleModel = "Honda Civic",
            InsuredCpf = "12345678900"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleValue)
            .WithErrorMessage("O valor do veículo deve ser maior que zero.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_WithEmptyVehicleModel_ShouldHaveValidationError(string invalidModel)
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = invalidModel,
            InsuredCpf = "12345678900"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleModel);
    }

    [Fact]
    public void Validate_WithVehicleModelTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = new string('A', 101), // 101 caracteres
            InsuredCpf = "12345678900"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VehicleModel)
            .WithErrorMessage("O modelo do veículo deve ter no máximo 100 caracteres.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_WithEmptyCpf_ShouldHaveValidationError(string invalidCpf)
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = "Honda Civic",
            InsuredCpf = invalidCpf
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.InsuredCpf);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345678")]
    [InlineData("123456789012")]
    [InlineData("abc")]
    public void Validate_WithInvalidCpf_ShouldHaveValidationError(string invalidCpf)
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = "Honda Civic",
            InsuredCpf = invalidCpf
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.InsuredCpf)
            .WithErrorMessage("CPF inválido. Formato esperado: 11 dígitos numéricos.");
    }

    [Theory]
    [InlineData("12345678900")]
    [InlineData("123.456.789-00")]
    [InlineData("123 456 789 00")]
    public void Validate_WithValidCpfFormats_ShouldNotHaveValidationError(string validCpf)
    {
        // Arrange
        var request = new CreateInsuranceRequest
        {
            VehicleValue = 10000m,
            VehicleModel = "Honda Civic",
            InsuredCpf = validCpf
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.InsuredCpf);
    }
}
