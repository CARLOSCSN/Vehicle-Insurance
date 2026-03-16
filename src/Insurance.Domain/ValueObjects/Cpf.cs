namespace Insurance.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um CPF válido
/// </summary>
public sealed class Cpf : IEquatable<Cpf>
{
    public string Value { get; }

    private Cpf(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Cria um CPF a partir de uma string, normalizando e validando
    /// </summary>
    public static Cpf Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));

        var normalized = Normalize(cpf);

        if (!IsValid(normalized))
            throw new ArgumentException("CPF inválido. Deve conter 11 dígitos numéricos.", nameof(cpf));

        return new Cpf(normalized);
    }

    /// <summary>
    /// Tenta criar um CPF, retornando null se inválido
    /// </summary>
    public static Cpf? TryCreate(string cpf)
    {
        try
        {
            return Create(cpf);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Normaliza o CPF removendo caracteres não numéricos
    /// </summary>
    public static string Normalize(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    /// <summary>
    /// Valida se o CPF tem o formato correto
    /// </summary>
    private static bool IsValid(string cpf)
    {
        return !string.IsNullOrWhiteSpace(cpf) && cpf.Length == 11;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is Cpf other && Equals(other);

    public bool Equals(Cpf? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Cpf? left, Cpf? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Cpf? left, Cpf? right) => !(left == right);

    public static implicit operator string(Cpf cpf) => cpf.Value;
}
