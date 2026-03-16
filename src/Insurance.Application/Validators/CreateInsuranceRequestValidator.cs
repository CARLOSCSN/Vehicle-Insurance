using FluentValidation;
using Insurance.Application.DTOs;
using Insurance.Domain.ValueObjects;

namespace Insurance.Application.Validators;

/// <summary>
/// Validador para CreateInsuranceRequest
/// </summary>
public class CreateInsuranceRequestValidator : AbstractValidator<CreateInsuranceRequest>
{
    public CreateInsuranceRequestValidator()
    {
        RuleFor(x => x.VehicleValue)
            .GreaterThan(0)
            .WithMessage("O valor do veículo deve ser maior que zero.");

        RuleFor(x => x.VehicleModel)
            .NotEmpty()
            .WithMessage("O modelo do veículo é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O modelo do veículo deve ter no máximo 100 caracteres.");

        RuleFor(x => x.InsuredCpf)
            .NotEmpty()
            .WithMessage("O CPF do segurado é obrigatório.")
            .Must(BeValidCpf)
            .WithMessage("CPF inválido. Formato esperado: 11 dígitos numéricos.");
    }

    private bool BeValidCpf(string cpf)
    {
        return Cpf.TryCreate(cpf) is not null;
    }
}
