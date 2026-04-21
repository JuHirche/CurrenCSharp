using System.Collections.Immutable;
using CurrenCSharp.Currencies;

namespace CurrenCSharp.Example;

internal class ExampleExchangeRateProvider : IExchangeRateProvider
{
    public Task<ExchangeRateContext> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var @base = Currency.Default;
        var date = new DateTimeOffset(new DateTime(2026, 4, 21), TimeSpan.Zero);
        var exchangeRates = ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(Iso4217.AUD, new(1.6432m))
                .Add(Iso4217.CAD, new(1.6105m))
                .Add(Iso4217.CHF, new(0.9189m))
                .Add(Iso4217.GBP, new(0.87045m))
                .Add(Iso4217.JPY, new(186.88m))
                .Add(Iso4217.NOK, new(10.9745m))
                .Add(Iso4217.PLN, new(4.2335m))
                .Add(Iso4217.SEK, new(10.7685m))
                .Add(Iso4217.USD, new(1.1760m));

        return Task.FromResult(new ExchangeRateContext(@base, date, exchangeRates));
    }

    public Task<ExchangeRateContext> GetHistoricalAsync(DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        var @base = Currency.Default;
        if (DateOnly.FromDateTime(date.Date) == new DateOnly(2020, 1, 2))
        {
            var exchangeRates = ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(Iso4217.AUD, new(1.6006m))
                .Add(Iso4217.CAD, new(1.4549m))
                .Add(Iso4217.CHF, new(1.0865m))
                .Add(Iso4217.GBP, new(0.84828m))
                .Add(Iso4217.JPY, new(121.75m))
                .Add(Iso4217.NOK, new(9.8408m))
                .Add(Iso4217.PLN, new(4.2544m))
                .Add(Iso4217.SEK, new(10.4728m))
                .Add(Iso4217.USD, new(1.1193m));

            return Task.FromResult(new ExchangeRateContext(@base, date, exchangeRates));
        }

        throw new InvalidOperationException($"No historical exchange rates available for {date:yyyy-MM-dd}.");
    }
}
