using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public sealed class ExchangeRateContextTests : TestFixture
{
    [Fact]
    public void Constructor_WhenBaseCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Currency baseCurrency = null!;

        // Act
        var exception = Record.Exception(() => _ = new ExchangeRateContext(baseCurrency, DateTimeOffset.UnixEpoch, LatestExchangeRates));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Constructor_WhenExchangeRatesAreNull_ThrowsArgumentNullException()
    {
        // Arrange
        IImmutableDictionary<Currency, ExchangeRate> exchangeRates = null!;

        // Act
        var exception = Record.Exception(() => _ = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, exchangeRates));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void GetExchangeRate_WhenFromCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        Currency fromCurrency = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.GetExchangeRate(fromCurrency, USD));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void GetExchangeRate_WhenToCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        Currency toCurrency = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.GetExchangeRate(USD, toCurrency));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void GetExchangeRate_WhenRequestedPairIsMissing_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(2m)));

        // Act
        var exception = Record.Exception(() => _ = sut.GetExchangeRate(EUR, JPY));

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void GetExchangeRate_WhenSourceAndTargetCurrencyMatch_ReturnsOne()
    {
        // Arrange
        var sut = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var result = sut.GetExchangeRate(USD, USD);

        // Assert
        Assert.Equal(1m, (decimal)result);
    }
}
