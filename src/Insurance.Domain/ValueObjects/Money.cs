namespace Insurance.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um valor monetário
/// </summary>
public sealed class Money : IEquatable<Money>, IComparable<Money>
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        Amount = amount;
    }

    /// <summary>
    /// Cria um valor monetário
    /// </summary>
    public static Money Create(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("O valor monetário não pode ser negativo.", nameof(amount));

        return new Money(Math.Round(amount, 2));
    }

    /// <summary>
    /// Cria um valor monetário zero
    /// </summary>
    public static Money Zero => new(0);

    /// <summary>
    /// Adiciona um valor monetário
    /// </summary>
    public Money Add(Money other) => Create(Amount + other.Amount);

    /// <summary>
    /// Subtrai um valor monetário
    /// </summary>
    public Money Subtract(Money other) => Create(Amount - other.Amount);

    /// <summary>
    /// Multiplica por um decimal
    /// </summary>
    public Money Multiply(decimal multiplier) => Create(Amount * multiplier);

    /// <summary>
    /// Aplica uma porcentagem de acréscimo
    /// </summary>
    public Money ApplyPercentage(decimal percentage) => Create(Amount * (1 + percentage));

    public override string ToString() => Amount.ToString("C2");

    public override bool Equals(object? obj) => obj is Money other && Equals(other);

    public bool Equals(Money? other) => other is not null && Amount == other.Amount;

    public override int GetHashCode() => Amount.GetHashCode();

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        return Amount.CompareTo(other.Amount);
    }

    public static bool operator ==(Money? left, Money? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Money? left, Money? right) => !(left == right);

    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

    public static Money operator +(Money left, Money right) => left.Add(right);

    public static Money operator -(Money left, Money right) => left.Subtract(right);

    public static Money operator *(Money left, decimal right) => left.Multiply(right);

    public static implicit operator decimal(Money money) => money.Amount;
}
