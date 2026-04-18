using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public class TestFixture : IDisposable
{
    public static readonly Currency EUR = new(nameof(EUR), 978, 2);
    public static readonly Currency USD = new(nameof(USD), 840, 2);
    public static readonly Currency JPY = new(nameof(JPY), 392, 0);

    public IExchangeRateProvider ExchangeRateProvider { get; } = new MockedExchangeRateProvider();

    public static IImmutableDictionary<Currency, ExchangeRate> LatestExchangeRates { get; } = new Dictionary<Currency, ExchangeRate>
    {
        { USD, new ExchangeRate(2m) },
        { JPY, new ExchangeRate(3m) }
    }.ToImmutableDictionary();

    public static IImmutableDictionary<Currency, ExchangeRate> HistoricalExchangeRates { get; } = new Dictionary<Currency, ExchangeRate>
    {
        { USD, new ExchangeRate(22m) },
        { JPY, new ExchangeRate(33m) }
    }.ToImmutableDictionary();

    private readonly IDisposable _defaultCurrencyScope;

    public TestFixture()
    {
        _defaultCurrencyScope = CurrenC.UseDefaultCurrency(EUR);
    }

    public void Dispose()
    {
        _defaultCurrencyScope.Dispose();
        GC.SuppressFinalize(this);
    }

    private class MockedExchangeRateProvider : IExchangeRateProvider
    {
        public Task<ExchangeRateContext> GetLatestAsync(CancellationToken cancellationToken = default) => 
            Task.FromResult(new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, LatestExchangeRates));

        public Task<ExchangeRateContext> GetHistoricalAsync(DateTimeOffset date, CancellationToken cancellationToken = default) =>
            Task.FromResult(new ExchangeRateContext(EUR, date, HistoricalExchangeRates));
    }
}
