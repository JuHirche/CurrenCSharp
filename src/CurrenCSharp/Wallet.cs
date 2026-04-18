using System.Collections;

namespace CurrenCSharp;

/// <summary>
/// Represents a collection of monetary values across one or multiple currencies.
/// </summary>
public sealed partial class Wallet : IEnumerable<Money>
{
    private readonly MoneyCollection _moneyCollection;

    internal Wallet(MoneyCollection moneyCollection)
    {
        _moneyCollection = moneyCollection;
    }

    public static Wallet Empty { get; } = new(MoneyCollection.Empty);

    /// <summary>
    /// Creates a wallet from the specified monetary values.
    /// </summary>
    /// <param name="moneys">The monetary values to include in the wallet.</param>
    /// <returns>A wallet containing the provided values.</returns>
    public static Wallet Of(params Money[] moneys) => new(MoneyCollection.Create(moneys));

    /// <summary>
    /// Creates a wallet from the specified monetary values.
    /// </summary>
    /// <param name="moneys">The monetary values to include in the wallet.</param>
    /// <returns>A wallet containing the provided values.</returns>
    public static Wallet Of(IReadOnlyCollection<Money> moneys) => new(MoneyCollection.Create(moneys));

    internal Currency ResolveCurrency() => _moneyCollection.SingleCurrencyOrDefault() ?? Currency.Default;

    /// <summary>
    /// Binds this wallet to an exchange-rate context.
    /// </summary>
    /// <param name="context">The exchange-rate context to bind.</param>
    /// <returns>A context-aware wallet.</returns>
    public ContextedWallet In(ExchangeRateContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new ContextedWallet(this, context);
    }

    public Wallet.Builder ToBuilder() => new(_moneyCollection.Items);

    /// <summary>
    /// Returns an enumerator that iterates through the monetary values in the wallet.
    /// </summary>
    /// <returns>An enumerator for the wallet contents.</returns>
    public IEnumerator<Money> GetEnumerator() => _moneyCollection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
