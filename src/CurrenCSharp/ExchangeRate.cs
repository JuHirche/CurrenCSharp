namespace CurrenCSharp;

/// <summary>
/// Represents a positive exchange rate between two currencies.
/// </summary>
public sealed record ExchangeRate
{
    private readonly decimal _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRate"/> class.
    /// </summary>
    /// <param name="value">The positive exchange rate value.</param>
    public ExchangeRate(decimal value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Conversion rate must be greater than zero.");

        _value = value;
    }

    /// <summary>
    /// Converts an <see cref="ExchangeRate"/> to its decimal value.
    /// </summary>
    /// <param name="exchangeRate">The exchange rate to convert.</param>
    /// <returns>The decimal exchange rate value.</returns>
    public static explicit operator decimal(ExchangeRate exchangeRate) => exchangeRate._value;
}
