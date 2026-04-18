namespace CurrenCSharp.Exceptions;

/// <summary>
/// The exception that is thrown when two monetary values use different currencies where the operation requires identical currencies.
/// </summary>
/// <param name="left">The left currency involved in the operation.</param>
/// <param name="right">The right currency involved in the operation.</param>
public sealed class DifferentCurrencyException(Currency left, Currency right) : Exception($"Different currencies: {left} and {right}.")
{
    internal static void ThrowIfDifferent(Currency left, Currency right)
    {
        if (left != right)
            throw new DifferentCurrencyException(left, right);
    }
}
