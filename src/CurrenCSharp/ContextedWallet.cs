namespace CurrenCSharp;

/// <summary>
/// Represents a <see cref="Wallet"/> bound to an <see cref="ExchangeRateContext"/>.
/// </summary>
public sealed class ContextedWallet
{
    public Wallet Wallet { get; }
    public ExchangeRateContext Context { get; }

    private static readonly ConversionOptions _withoutRounding = new(RoundResult: false);

    internal ContextedWallet(Wallet wallet, ExchangeRateContext context)
    {
        Wallet = wallet;
        Context = context;
    }

    /// <summary>
    /// Calculates the total wallet value using the wallet's resolved currency.
    /// </summary>
    /// <returns>The aggregated wallet total.</returns>
    public Money Total() => Total(Wallet.ResolveCurrency());

    /// <summary>
    /// Calculates the total wallet value converted to a specific currency.
    /// </summary>
    /// <param name="currency">The target currency for the total.</param>
    /// <param name="options">Optional conversion and rounding options for the final total.</param>
    /// <returns>The aggregated wallet total in <paramref name="currency"/>.</returns>
    public Money Total(Currency currency, ConversionOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(currency);

        Money total = Wallet.Aggregate(
            Money.Zero(currency),
            (current, money) => current + money.In(Context).Convert(currency, _withoutRounding));

        return new Money(total.Amount.Round(total.Currency.MinorUnits, options), currency);
    }
}
