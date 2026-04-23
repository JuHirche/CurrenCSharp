using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public sealed class ExchangeRateContextTests : TestFixture
{
    public static TheoryData<Func<ExchangeRateContext>> ConstructorNullInputs => new()
    {
        { () => new ExchangeRateContext(null!, DateTimeOffset.UnixEpoch, LatestExchangeRates) },
        { () => new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, null!) },
    };

    [Theory]
    [MemberData(nameof(ConstructorNullInputs))]
    public void Constructor_WhenRequiredArgumentIsNull_ThrowsArgumentNullException(Func<ExchangeRateContext> act)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    public static TheoryData<Func<ExchangeRateContext, ExchangeRate>> GetExchangeRateNullInputs => new()
    {
        { ctx => ctx.GetExchangeRate(null!, TestFixture.USD) },
        { ctx => ctx.GetExchangeRate(TestFixture.USD, null!) },
    };

    [Theory]
    [MemberData(nameof(GetExchangeRateNullInputs))]
    public void GetExchangeRate_WhenEitherCurrencyIsNull_ThrowsArgumentNullException(Func<ExchangeRateContext, ExchangeRate> act)
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => act(ctx));
    }

    public static TheoryData<Currency> SameCurrencyData => new() { EUR, USD, JPY };

    [Theory]
    [MemberData(nameof(SameCurrencyData))]
    public void GetExchangeRate_WhenSourceAndTargetCurrencyMatch_ReturnsOne(Currency currency)
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var rate = ctx.GetExchangeRate(currency, currency);

        // Assert
        Assert.Equal(1m, (decimal)rate);
    }

    [Fact]
    public void GetExchangeRate_WhenFromIsBase_ReturnsDirectRate()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var rate = ctx.GetExchangeRate(EUR, USD);

        // Assert
        Assert.Equal(2m, (decimal)rate);
    }

    [Fact]
    public void GetExchangeRate_WhenToIsBase_ReturnsInverseRate()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var rate = ctx.GetExchangeRate(USD, EUR);

        // Assert
        Assert.Equal(0.5m, (decimal)rate);
    }

    [Fact]
    public void GetExchangeRate_WhenNeitherIsBase_ReturnsCrossRate()
    {
        // Arrange — EUR->USD=2, EUR->JPY=3, so USD->JPY = 3/2 = 1.5
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var rate = ctx.GetExchangeRate(USD, JPY);

        // Assert
        Assert.Equal(1.5m, (decimal)rate);
    }

    [Fact]
    public void GetExchangeRate_WhenPairMissing_ThrowsInvalidOperationException()
    {
        // Arrange — base is a currency for which no rates exist
        var unknownBase = new Currency("XXX", 999, 2);
        var ctx = new ExchangeRateContext(unknownBase, DateTimeOffset.UnixEpoch,
            new Dictionary<Currency, ExchangeRate>().ToImmutableDictionary());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => ctx.GetExchangeRate(EUR, JPY));
    }

    [Fact]
    public void GetExchangeRate_WhenCalledConcurrently_ReturnsConsistentValues()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var results = Enumerable.Range(0, 1000)
            .AsParallel()
            .Select(_ => (direct: (decimal)ctx.GetExchangeRate(EUR, USD),
                          inverse: (decimal)ctx.GetExchangeRate(USD, EUR),
                          cross: (decimal)ctx.GetExchangeRate(USD, JPY)))
            .ToList();

        // Assert
        Assert.All(results, r =>
        {
            Assert.Equal(2m, r.direct);
            Assert.Equal(0.5m, r.inverse);
            Assert.Equal(1.5m, r.cross);
        });
    }

    [Fact]
    public void GetEnumerator_WhenRatesExist_ReturnsAllConfiguredRates()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var items = ctx.ToList();

        // Assert
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public void GetEnumerator_WhenEnumeratedMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var first = ctx.ToList();
        var second = ctx.ToList();

        // Assert
        Assert.Equal(first.Count, second.Count);
    }

    [Fact]
    public void Properties_WhenAccessed_ReturnsConstructorValues()
    {
        // Arrange
        var reference = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var ctx = new ExchangeRateContext(EUR, reference, LatestExchangeRates);

        // Act & Assert
        Assert.Equal(EUR, ctx.Base);
        Assert.Equal(reference, ctx.Reference);
    }
}
