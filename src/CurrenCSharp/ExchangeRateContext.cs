using System.Collections;
using System.Collections.Immutable;

namespace CurrenCSharp;

/// <summary>
/// Provides exchange rates relative to a base currency and reference timestamp.
/// </summary>
public sealed class ExchangeRateContext : IEnumerable<(Currency, ExchangeRate)>
{
    public Currency Base { get; }
    public DateTimeOffset Reference { get; }

    private readonly IImmutableDictionary<Currency, ExchangeRate> _exchangeRates;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateContext"/> class.
    /// </summary>
    /// <param name="base">The base currency for all stored exchange rates.</param>
    /// <param name="reference">The timestamp that describes when the rates are valid.</param>
    /// <param name="exchangeRates">The exchange rates from the base currency to other currencies.</param>
    public ExchangeRateContext(Currency @base, DateTimeOffset reference, IImmutableDictionary<Currency, ExchangeRate> exchangeRates)
    {
        ArgumentNullException.ThrowIfNull(@base);
        ArgumentNullException.ThrowIfNull(exchangeRates);

        Base = @base;
        Reference = reference;
        _exchangeRates = exchangeRates;
    }

    /// <summary>
    /// Gets the exchange rate from one currency to another.
    /// </summary>
    /// <param name="from">The source currency.</param>
    /// <param name="to">The target currency.</param>
    /// <returns>The exchange rate from <paramref name="from"/> to <paramref name="to"/>.</returns>
    public ExchangeRate GetExchangeRate(Currency from, Currency to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from == to)
            return new(1m);

        if (from == Base && _exchangeRates.TryGetValue(to, out var exchangeRate) && exchangeRate is not null)
            return exchangeRate;

        if (to == Base && _exchangeRates.TryGetValue(from, out exchangeRate) && exchangeRate is not null)
            return new(1m / (decimal)exchangeRate);

        if (from != Base && to != Base)
            return new((decimal)GetExchangeRate(Base, to) / (decimal)GetExchangeRate(Base, from));

        throw new InvalidOperationException(
            $"Exchange rate from {from.AlphaCode} to {to.AlphaCode} is not available in this context.");
    }

    /// <summary>
    /// Returns an enumerator over all configured rates from the base currency.
    /// </summary>
    /// <returns>An enumerator of currency and exchange-rate pairs.</returns>
    public IEnumerator<(Currency, ExchangeRate)> GetEnumerator() =>
        _exchangeRates.Select(kvp => (kvp.Key, kvp.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
