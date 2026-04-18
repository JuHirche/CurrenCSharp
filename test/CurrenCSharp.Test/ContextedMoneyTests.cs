using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public sealed class ContextedMoneyTests : TestFixture
{
    [Fact]
    public void Convert_WhenTargetCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var sut = new Money(1m, EUR).In(context);
        Currency targetCurrency = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.Convert(targetCurrency));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Convert_WhenRateIsMissing_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty);
        var sut = new Money(1m, EUR).In(context);

        // Act
        var exception = Record.Exception(() => _ = sut.Convert(USD));

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Convert_WhenRoundResultIsFalse_DoesNotRoundConvertedAmount()
    {
        // Arrange
        var context = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(JPY, new ExchangeRate(1.5m)));
        var sut = new Money(1m, EUR).In(context);
        var options = new ConversionOptions(RoundResult: false);

        // Act
        var result = sut.Convert(JPY, options);

        // Assert
        Assert.Equal(1.5m, result.Amount);
    }

    [Fact]
    public void Convert_WhenScaleIsProvided_UsesProvidedScaleForRounding()
    {
        // Arrange
        var context = new ExchangeRateContext(
            EUR,
            DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(1.234567m)));
        var sut = new Money(1m, EUR).In(context);
        var options = new ConversionOptions(
            RoundResult: true,
            RoundingMode: MidpointRounding.ToEven,
            Scale: new Scale(4));

        // Act
        var result = sut.Convert(USD, options);

        // Assert
        Assert.Equal(1.2346m, result.Amount);
    }
}
