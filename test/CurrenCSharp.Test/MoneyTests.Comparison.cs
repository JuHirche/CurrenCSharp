using System.Collections.Immutable;
using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    public static TheoryData<decimal, decimal, int> CompareToData => new()
    {
        { 5m, 10m, -1 },
        { 10m, 10m, 0 },
        { 10m, 5m, 1 },
    };

    [Theory]
    [MemberData(nameof(CompareToData))]
    public void CompareTo_WhenCurrenciesMatch_ReturnsExpectedSign(decimal leftAmount, decimal rightAmount, int expectedSign)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = left.CompareTo(right);

        // Assert
        Assert.Equal(expectedSign, Math.Sign(result));
    }

    [Fact]
    public void CompareTo_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(10m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => left.CompareTo(right));
    }

    [Fact]
    public void CompareTo_WhenContextedMoneyIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act
        var result = sut.CompareTo((ContextedMoney?)null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WhenCurrenciesDiffer_ConvertsBeforeComparing()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, new Dictionary<Currency, ExchangeRate>
        {
            { USD, new ExchangeRate(1.1m) }
        }.ToImmutableDictionary());

        var sut = new Money(110m, USD);
        var other = new Money(100m, EUR).In(context); // 100 EUR -> 110 USD

        // Act
        var result = sut.CompareTo(other);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WhenContextedWalletIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act
        var result = sut.CompareTo((ContextedWallet?)null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WhenContextedWalletCurrenciesDiffer_UsesTotalInOwnCurrency()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, LatestExchangeRates); // EUR->USD=2, EUR->JPY=3
        var wallet = Wallet.Of(new Money(50m, EUR), new Money(50m, USD)).In(context); // 50 EUR + 50 USD = 50 EUR + 25 EUR = 75 EUR = 150 USD
        var sut = new Money(150m, USD);

        // Act
        var result = sut.CompareTo(wallet);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CrossTypeComparisonOperators_WithContextedMoney_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, LatestExchangeRates);
        var money = new Money(100m, USD);
        var equalContexted = new Money(100m, USD).In(context);
        var smallerContexted = new Money(50m, USD).In(context);
        var largerContexted = new Money(200m, USD).In(context);

        // Act & Assert — money vs ContextedMoney
        Assert.True(money == equalContexted);
        Assert.False(money != equalContexted);
        Assert.True(money < largerContexted);
        Assert.True(money <= equalContexted);
        Assert.True(money > smallerContexted);
        Assert.True(money >= equalContexted);

        // Reversed operand order
        Assert.True(equalContexted == money);
        Assert.False(equalContexted != money);
        Assert.True(smallerContexted < money);
        Assert.True(equalContexted <= money);
        Assert.True(largerContexted > money);
        Assert.True(equalContexted >= money);
    }

    [Fact]
    public void CrossTypeComparisonOperators_WhenContextedReferenceIsNull_ReturnExpectedResult()
    {
        // Arrange
        var money = new Money(1m, EUR);
        ContextedMoney contextedMoneyNull = null!;
        ContextedWallet contextedWalletNull = null!;

        // Act & Assert — money vs null ContextedMoney
        Assert.False(money == contextedMoneyNull);
        Assert.True(money != contextedMoneyNull);
        Assert.False(money < contextedMoneyNull);
        Assert.False(money <= contextedMoneyNull);
        Assert.True(money > contextedMoneyNull);
        Assert.True(money >= contextedMoneyNull);

        // Reversed
        Assert.False(contextedMoneyNull == money);
        Assert.True(contextedMoneyNull != money);
        Assert.True(contextedMoneyNull < money);
        Assert.True(contextedMoneyNull <= money);
        Assert.False(contextedMoneyNull > money);
        Assert.False(contextedMoneyNull >= money);

        // money vs null ContextedWallet
        Assert.False(money == contextedWalletNull);
        Assert.True(money != contextedWalletNull);
        Assert.False(money < contextedWalletNull);
        Assert.False(money <= contextedWalletNull);
        Assert.True(money > contextedWalletNull);
        Assert.True(money >= contextedWalletNull);

        // Reversed
        Assert.False(contextedWalletNull == money);
        Assert.True(contextedWalletNull != money);
        Assert.True(contextedWalletNull < money);
        Assert.True(contextedWalletNull <= money);
        Assert.False(contextedWalletNull > money);
        Assert.False(contextedWalletNull >= money);
    }

    [Fact]
    public void CrossTypeComparisonOperators_WhenMoneyOperandIsDefault_ThrowNoCurrencyException()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        Money defaultMoney = default;
        var contextedMoney = new Money(1m, USD).In(context);
        var contextedWallet = Wallet.Of(new Money(1m, USD)).In(context);

        // Act & Assert — default money with ContextedMoney
        Assert.Throws<NoCurrencyException>(() => _ = defaultMoney == contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney < defaultMoney);

        // default money with ContextedWallet
        Assert.Throws<NoCurrencyException>(() => _ = defaultMoney == contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet < defaultMoney);

        // default money comparison operators with money
        var money = new Money(1m, EUR);
        Assert.Throws<NoCurrencyException>(() => _ = defaultMoney < money);
        Assert.Throws<NoCurrencyException>(() => _ = money < defaultMoney);
    }

    [Fact]
    public void CrossTypeComparisonOperators_WithContextedWallet_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UtcNow, LatestExchangeRates); // EUR->USD=2
        var smallerTotal = Wallet.Of(new Money(10m, EUR)).In(context);
        var equalTotal = Wallet.Of(new Money(100m, EUR)).In(context);
        var largerTotal = Wallet.Of(new Money(1000m, EUR)).In(context);
        var money = new Money(100m, EUR);

        // Act & Assert — money vs ContextedWallet
        Assert.True(money == equalTotal);
        Assert.False(money != equalTotal);
        Assert.True(money < largerTotal);
        Assert.True(money <= equalTotal);
        Assert.True(money > smallerTotal);
        Assert.True(money >= equalTotal);

        // Reversed operand order
        Assert.True(equalTotal == money);
        Assert.False(equalTotal != money);
        Assert.True(smallerTotal < money);
        Assert.True(equalTotal <= money);
        Assert.True(largerTotal > money);
        Assert.True(equalTotal >= money);
    }
}
