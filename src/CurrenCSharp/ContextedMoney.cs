namespace CurrenCSharp;

/// <summary>
/// Represents a <see cref="Money"/> value bound to an <see cref="ExchangeRateContext"/>.
/// </summary>
public sealed record ContextedMoney
{
    private readonly Money _money;

    public decimal Amount => _money.Amount;
    public Currency Currency => _money.Currency;
    public ExchangeRateContext Context { get; }

    internal ContextedMoney(Money money, ExchangeRateContext context)
    {
        _money = money;
        Context = context;
    }

    /// <summary>
    /// Converts the monetary value to another currency using the associated exchange-rate context.
    /// </summary>
    /// <param name="to">The target currency.</param>
    /// <param name="options">Optional conversion and rounding options.</param>
    /// <returns>The converted monetary value.</returns>
    public Money Convert(Currency to, ConversionOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(to);

        var rate = Context.GetExchangeRate(Currency, to);
        var amount = (Amount * (decimal)rate).Round(to.MinorUnits, options);

        return new Money(amount, to);
    }

    /// <summary>
    /// Returns the string representation of the wrapped monetary value.
    /// </summary>
    /// <returns>A currency-formatted string representation.</returns>
    public override string ToString() => _money.ToString();
}
