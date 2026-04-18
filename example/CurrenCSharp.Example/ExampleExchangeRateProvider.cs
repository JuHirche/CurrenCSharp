using System.Collections.Immutable;

namespace CurrenCSharp.Example;

internal class ExampleExchangeRateProvider : IExchangeRateProvider
{
    public Task<ExchangeRateContext> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var @base = Currency.Default;
        var now = DateTimeOffset.UtcNow;
        var exchangeRates = ImmutableDictionary<Currency, ExchangeRate>.Empty
            .Add(Currencies.Iso4217.CHF, new(0.94571668m))
            .Add(Currencies.Iso4217.USD, new(1.0911735m));

        return Task.FromResult(new ExchangeRateContext(@base, now, exchangeRates));
    }

    public Task<ExchangeRateContext> GetHistoricalAsync(DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        var @base = Currency.Default;
        if (DateOnly.FromDateTime(date.Date) == new DateOnly(2020, 1, 1))
        {
            var exchangeRates = ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(Currencies.Iso4217.CHF, new(0.9200518049479665m))
                .Add(Currencies.Iso4217.USD, new(0.8931785039507516m));

            return Task.FromResult(new ExchangeRateContext(@base, date, exchangeRates));
        }

        throw new InvalidOperationException($"No historical exchange rates available for {date:yyyy-MM-dd}.");
    }
}
