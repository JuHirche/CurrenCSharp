using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public sealed class ContextedWalletTests : TestFixture
{
    [Fact]
    public void Total_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var sut = Wallet.Of(new Money(1m, EUR)).In(context);
        Currency currency = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.Total(currency));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Total_WhenWalletContainsMixedCurrencies_ReturnsExpectedRoundedAmount()
    {
        // Arrange
        var context = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(3m)));
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(context);

        // Act
        var result = sut.Total(EUR);

        // Assert
        Assert.Equal(1.33m, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Total_WhenRoundResultIsFalse_DoesNotRoundFinalAmount()
    {
        // Arrange
        var context = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(3m)));
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(context);
        var options = new ConversionOptions(RoundResult: false);

        // Act
        var result = sut.Total(EUR, options);

        // Assert
        Assert.Equal(1m + (1m / 3m), result.Amount);
        Assert.Equal(EUR, result.Currency);
    }
}
