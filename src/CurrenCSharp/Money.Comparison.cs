using CurrenCSharp.Exceptions;

namespace CurrenCSharp;

public readonly partial record struct Money : IComparable<Money>, IComparable<ContextedMoney>, IComparable<ContextedWallet>
{
    /// <summary>
    /// Determines whether a monetary value is equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Money left, ContextedMoney right) => left.CompareTo(right) == 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ContextedMoney left, Money right) => right.CompareTo(left) == 0;

    /// <summary>
    /// Determines whether a monetary value is equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Money left, ContextedWallet right) => left.CompareTo(right) == 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ContextedWallet left, Money right) => right.CompareTo(left) == 0;

    /// <summary>
    /// Determines whether a monetary value is not equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Money left, ContextedMoney right) => left.CompareTo(right) != 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is not equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ContextedMoney left, Money right) => right.CompareTo(left) != 0;

    /// <summary>
    /// Determines whether a monetary value is not equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Money left, ContextedWallet right) => left.CompareTo(right) != 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is not equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ContextedWallet left, Money right) => right.CompareTo(left) != 0;

    /// <summary>
    /// Determines whether one monetary value is less than another monetary value.
    /// Throws <see cref="DifferentCurrencyException"/> when both values use different currencies.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DifferentCurrencyException">Thrown when <paramref name="left"/> and <paramref name="right"/> have different currencies.</exception>
    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether a monetary value is less than a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Money left, ContextedMoney right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is less than a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(ContextedMoney left, Money right) => right.CompareTo(left) > 0;

    /// <summary>
    /// Determines whether a monetary value is less than a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Money left, ContextedWallet right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is less than a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(ContextedWallet left, Money right) => right.CompareTo(left) > 0;

    /// <summary>
    /// Determines whether one monetary value is less than or equal to another monetary value.
    /// Throws <see cref="DifferentCurrencyException"/> when both values use different currencies.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DifferentCurrencyException">Thrown when <paramref name="left"/> and <paramref name="right"/> have different currencies.</exception>
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether a monetary value is less than or equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Money left, ContextedMoney right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is less than or equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(ContextedMoney left, Money right) => right.CompareTo(left) >= 0;

    /// <summary>
    /// Determines whether a monetary value is less than or equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Money left, ContextedWallet right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is less than or equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(ContextedWallet left, Money right) => right.CompareTo(left) >= 0;

    /// <summary>
    /// Determines whether one monetary value is greater than another monetary value.
    /// Throws <see cref="DifferentCurrencyException"/> when both values use different currencies.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DifferentCurrencyException">Thrown when <paramref name="left"/> and <paramref name="right"/> have different currencies.</exception>
    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether a monetary value is greater than a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Money left, ContextedMoney right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is greater than a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(ContextedMoney left, Money right) => right.CompareTo(left) < 0;

    /// <summary>
    /// Determines whether a monetary value is greater than a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Money left, ContextedWallet right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is greater than a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(ContextedWallet left, Money right) => right.CompareTo(left) < 0;

    /// <summary>
    /// Determines whether one monetary value is greater than or equal to another monetary value.
    /// Throws <see cref="DifferentCurrencyException"/> when both values use different currencies.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="DifferentCurrencyException">Thrown when <paramref name="left"/> and <paramref name="right"/> have different currencies.</exception>
    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether a monetary value is greater than or equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Money left, ContextedMoney right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is greater than or equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(ContextedMoney left, Money right) => right.CompareTo(left) <= 0;

    /// <summary>
    /// Determines whether a monetary value is greater than or equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Money left, ContextedWallet right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is greater than or equal to a monetary value.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(ContextedWallet left, Money right) => right.CompareTo(left) <= 0;

    /// <summary>
    /// Compares this monetary value with another monetary value.
    /// Throws <see cref="DifferentCurrencyException"/> when both values use different currencies.
    /// </summary>
    /// <param name="other">The monetary value to compare with this instance.</param>
    /// <returns>
    /// A value less than zero if this instance is less than <paramref name="other"/>,
    /// zero if both are equal, or greater than zero if this instance is greater.
    /// </returns>
    /// <exception cref="DifferentCurrencyException">Thrown when this instance and <paramref name="other"/> have different currencies.</exception>
    public int CompareTo(Money other)
    {
        DifferentCurrencyException.ThrowIfDifferent(Currency, other.Currency);
        return Amount.CompareTo(other.Amount);
    }

    /// <summary>
    /// Compares this monetary value with a context-aware monetary value.
    /// </summary>
    /// <param name="other">The context-aware monetary value to compare with this instance.</param>
    /// <returns>
    /// A value less than zero if this instance is less than <paramref name="other"/>,
    /// zero if both are equal, or greater than zero if this instance is greater.
    /// </returns>
    public int CompareTo(ContextedMoney? other) => other switch
    {
        null => 1,
        _ => CompareTo(other.Convert(Currency))
    };

    /// <summary>
    /// Compares this monetary value with the total of a context-aware wallet.
    /// </summary>
    /// <param name="other">The context-aware wallet to compare with this instance.</param>
    /// <returns>
    /// A value less than zero if this instance is less than <paramref name="other"/>,
    /// zero if both are equal, or greater than zero if this instance is greater.
    /// </returns>
    public int CompareTo(ContextedWallet? other) => other switch
    {
        null => 1,
        _ => CompareTo(other.Total(Currency))
    };
}
