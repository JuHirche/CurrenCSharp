using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public partial class MoneyTests : TestFixture
{
    [Fact]
    public void Constructor_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Money(1m, null!));
    }

    [Fact]
    public void Currency_WhenMoneyIsDefault_ThrowsNoCurrencyException()
    {
        // Arrange
        var sut = default(Money);

        // Act & Assert
        Assert.Throws<NoCurrencyException>(() => _ = sut.Currency);
    }

    [Fact]
    public void DefaultMoney_WhenAmountAccessed_ReturnsZero()
    {
        // Arrange
        var sut = default(Money);

        // Act
        var amount = sut.Amount;

        // Assert
        Assert.Equal(0m, amount);
    }

    [Fact]
    public void Zero_WhenNoArgumentProvided_ReturnsMoneyWithZeroAmountAndDefaultCurrency()
    {
        // Act
        var result = Money.Zero();

        // Assert
        Assert.Equal(0m, result.Amount);
        Assert.Equal(Currency.Default, result.Currency);
    }

    [Fact]
    public void Zero_WhenNoDefaultCurrencyConfigured_ThrowsNoDefaultCurrencyException()
    {
        // Arrange
        Dispose();
        try
        {
            // Act & Assert
            Assert.Throws<NoDefaultCurrencyException>(() => Money.Zero());
        }
        finally
        {
            _ = CurrenC.UseDefaultCurrency(EUR);
        }
    }

    [Fact]
    public void Zero_WhenCurrencyIsProvided_ReturnsMoneyWithZeroAmountAndProvidedCurrency()
    {
        // Act
        var result = Money.Zero(USD);

        // Assert
        Assert.Equal(0m, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Zero_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Money.Zero(null!));
    }

    [Fact]
    public void In_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.In(null!));
    }

    [Fact]
    public void In_WhenMoneyHasNoCurrency_ThrowsNoCurrencyException()
    {
        // Arrange
        var sut = default(Money);
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, LatestExchangeRates);

        // Act & Assert
        Assert.Throws<NoCurrencyException>(() => sut.In(context));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void SignProperties_WhenAmountVaries_ReturnExpectedFlags(decimal amount)
    {
        // Arrange
        var sut = new Money(amount, USD);

        // Act & Assert
        Assert.Equal(amount == 0m, sut.IsZero);
        Assert.Equal(amount > 0m, sut.IsPositive);
        Assert.Equal(amount < 0m, sut.IsNegative);
    }

    [Theory]
    [InlineData(-10.25, 10.25)]
    [InlineData(0, 0)]
    [InlineData(10.25, 10.25)]
    public void Abs_WhenCalled_ReturnsPositiveMoney(decimal amount, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = sut.Abs();

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Round_WhenCurrencyHasMinorUnits_RoundsToCurrencyScale()
    {
        // Arrange
        var tst = new Currency(new AlphaCode("TST"), new NumericCode(900), 2);
        var sut = new Money(1.235m, tst);

        // Act
        var result = sut.Round();

        // Assert
        Assert.Equal(1.24m, result.Amount);
    }

    [Fact]
    public void Round_WhenCurrencyHasZeroMinorUnits_RoundsToInteger()
    {
        // Arrange
        var sut = new Money(1.5m, JPY);

        // Act
        var result = sut.Round();

        // Assert
        Assert.Equal(2m, result.Amount);
    }

    [Theory]
    [InlineData(1.005, 2, MidpointRounding.ToEven, 1.00)]
    [InlineData(1.005, 2, MidpointRounding.AwayFromZero, 1.01)]
    public void Round_WhenScaleAndModeProvided_UsesProvidedRounding(
        decimal amount, int decimals, MidpointRounding mode, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = sut.Round(decimals, mode);

        // Assert
        Assert.Equal(expected, result.Amount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(29)]
    public void Round_WhenDecimalsOutOfRange_ThrowsArgumentOutOfRangeException(int decimals)
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Round(decimals));
    }

    public static TheoryData<decimal, decimal, decimal> MinData => new()
    {
        { 10m, 5m, 5m },
        { 5m, 5m, 5m },
    };

    [Theory]
    [MemberData(nameof(MinData))]
    public void Min_WhenCurrenciesMatch_ReturnsSmallerAmount(decimal leftAmount, decimal rightAmount, decimal expected)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = Money.Min(left, right);

        // Assert
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void Min_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(10m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => Money.Min(left, right));
    }

    public static TheoryData<decimal, decimal, decimal> MaxData => new()
    {
        { 10m, 5m, 10m },
        { 5m, 5m, 5m },
    };

    [Theory]
    [MemberData(nameof(MaxData))]
    public void Max_WhenCurrenciesMatch_ReturnsGreaterAmount(decimal leftAmount, decimal rightAmount, decimal expected)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = Money.Max(left, right);

        // Assert
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void Max_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(10m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => Money.Max(left, right));
    }

    [Fact]
    public void Equals_WhenAmountAndCurrencyMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(10m, EUR);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    public static TheoryData<Money, Money> DifferingMoney => new()
    {
        { new Money(10m, TestFixture.EUR), new Money(11m, TestFixture.EUR) },
        { new Money(10m, TestFixture.EUR), new Money(10m, TestFixture.USD) },
    };

    [Theory]
    [MemberData(nameof(DifferingMoney))]
    public void Equals_WhenAnyFieldDiffers_ReturnsFalse(Money left, Money right)
    {
        // Act & Assert
        Assert.NotEqual(left, right);
    }
}
