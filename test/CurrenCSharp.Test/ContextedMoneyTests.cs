using System.Collections.Immutable;

namespace CurrenCSharp.Test;

public sealed class ContextedMoneyTests : TestFixture
{
    private readonly ExchangeRateContext _context = new(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

    [Fact]
    public void Convert_WhenTargetCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new Money(1m, EUR).In(_context);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.Convert(null!));
    }

    [Fact]
    public void Convert_WhenRateIsMissing_ThrowsInvalidOperationException()
    {
        // Arrange — a USD base context without any rates
        var ctxWithoutRates = new ExchangeRateContext(USD, DateTimeOffset.UnixEpoch,
            new Dictionary<Currency, ExchangeRate>().ToImmutableDictionary());
        var sut = new Money(1m, EUR).In(ctxWithoutRates);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sut.Convert(JPY));
    }

    [Fact]
    public void Convert_WhenSourceEqualsTarget_ReturnsSameAmountAndCurrency()
    {
        // Arrange
        var sut = new Money(12.34m, USD).In(_context);

        // Act
        var result = sut.Convert(USD);

        // Assert
        Assert.Equal(12.34m, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Convert_WhenAmountIsZero_ReturnsZeroInTargetCurrency()
    {
        // Arrange
        var sut = new Money(0m, EUR).In(_context);

        // Act
        var result = sut.Convert(USD);

        // Assert
        Assert.Equal(0m, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Convert_WhenRoundResultIsFalse_DoesNotRound()
    {
        // Arrange — rate 1.5, amount 1 EUR -> 1.5 JPY (JPY has 0 minor units)
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
            new Dictionary<Currency, ExchangeRate> { { JPY, new ExchangeRate(1.5m) } }.ToImmutableDictionary());
        var sut = new Money(1m, EUR).In(ctx);

        // Act
        var result = sut.Convert(JPY, new ConversionOptions(RoundResult: false));

        // Assert
        Assert.Equal(1.5m, result.Amount);
    }

    public static TheoryData<MidpointRounding, decimal> RoundingModes => new()
    {
        { MidpointRounding.ToEven, 1.2346m },
        { MidpointRounding.AwayFromZero, 1.2346m },
    };

    [Theory]
    [MemberData(nameof(RoundingModes))]
    public void Convert_WhenScaleProvided_UsesScaleAndMode(MidpointRounding mode, decimal expected)
    {
        // Arrange
        var target = new Currency("TST", 999, 2);
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
            new Dictionary<Currency, ExchangeRate> { { target, new ExchangeRate(1.234567m) } }.ToImmutableDictionary());
        var sut = new Money(1m, EUR).In(ctx);

        // Act
        var result = sut.Convert(target, new ConversionOptions(RoundingMode: mode, Scale: new Scale(4)));

        // Assert
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void Convert_WhenOptionsAreNull_UsesDefaultRoundingByTargetMinorUnits()
    {
        // Arrange — 1.005 EUR * 1 = 1.005 USD, USD has 2 minor units, ToEven -> 1.00
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
            new Dictionary<Currency, ExchangeRate> { { USD, new ExchangeRate(1m) } }.ToImmutableDictionary());
        var sut = new Money(1.005m, EUR).In(ctx);

        // Act
        var result = sut.Convert(USD, options: null);

        // Assert
        Assert.Equal(1.00m, result.Amount);
    }

    [Fact]
    public void ToString_WhenCalled_DelegatesToUnderlyingMoney()
    {
        // Arrange
        var sut = new Money(12.34m, EUR).In(_context);

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Contains("EUR", result, StringComparison.Ordinal);
    }

    [Fact]
    public void Properties_WhenAccessed_DelegateToUnderlyingMoney()
    {
        // Arrange
        var sut = new Money(12.34m, EUR).In(_context);

        // Act & Assert
        Assert.Equal(12.34m, sut.Amount);
        Assert.Equal(EUR, sut.Currency);
    }

    [Fact]
    public void Equals_WhenSameMoneyAndContext_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new Money(1m, EUR).In(_context);
        var right = new Money(1m, EUR).In(_context);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
