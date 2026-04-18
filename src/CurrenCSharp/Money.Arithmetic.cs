using CurrenCSharp.Exceptions;

namespace CurrenCSharp;

public readonly partial record struct Money
{
    /// <summary>
    /// Returns the same monetary value.
    /// </summary>
    /// <param name="money">The monetary value.</param>
    /// <returns>The unchanged monetary value.</returns>
    public static Money operator +(Money money) => new(money.Amount, money.Currency);

    /// <summary>
    /// Negates the amount of a monetary value.
    /// </summary>
    /// <param name="money">The monetary value to negate.</param>
    /// <returns>A monetary value with the negated amount.</returns>
    public static Money operator -(Money money) => new(-money.Amount, money.Currency);

    /// <summary>
    /// Adds two monetary values with the same currency.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns>The sum of both values.</returns>
    public static Money operator +(Money left, Money right)
    {
        DifferentCurrencyException.ThrowIfDifferent(left.Currency, right.Currency);
        return new(left.Amount + right.Amount, left.Currency); 
    }

    /// <summary>
    /// Subtracts one monetary value from another with the same currency.
    /// </summary>
    /// <param name="left">The left monetary value.</param>
    /// <param name="right">The right monetary value.</param>
    /// <returns>The subtraction result.</returns>
    public static Money operator -(Money left, Money right)
    {
        DifferentCurrencyException.ThrowIfDifferent(left.Currency, right.Currency);
        return new(left.Amount - right.Amount, left.Currency);
    }

    /// <summary>
    /// Multiplies a monetary value by a scalar.
    /// </summary>
    /// <param name="left">The monetary value.</param>
    /// <param name="right">The multiplication factor.</param>
    /// <returns>The scaled monetary value.</returns>
    public static Money operator *(Money left, decimal right) => new(left.Amount * right, left.Currency);

    /// <summary>
    /// Multiplies a monetary value by a scalar.
    /// </summary>
    /// <param name="left">The multiplication factor.</param>
    /// <param name="right">The monetary value.</param>
    /// <returns>The scaled monetary value.</returns>
    public static Money operator *(decimal left, Money right) => right * left;

    /// <summary>
    /// Divides one monetary value by another with the same currency to produce a ratio.
    /// </summary>
    /// <param name="left">The dividend monetary value.</param>
    /// <param name="right">The divisor monetary value.</param>
    /// <returns>The ratio of both amounts.</returns>
    public static decimal operator /(Money left, Money right)
    {
        DifferentCurrencyException.ThrowIfDifferent(left.Currency, right.Currency);
        return !right.IsZero 
            ? left.Amount / right.Amount 
            : throw new DivideByZeroException("Cannot divide by zero money.");
    }

    /// <summary>
    /// Divides a monetary value by a scalar.
    /// </summary>
    /// <param name="left">The dividend monetary value.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The scaled monetary value.</returns>
    public static Money operator /(Money left, decimal right) =>
        right != 0m
            ? new(left.Amount / right, left.Currency)
            : throw new DivideByZeroException("Cannot divide by zero.");
}
