namespace CurrenCSharp.Test;

public sealed partial class WalletTests
{
    [Fact]
    public void Equals_WhenInsertionOrderDiffers_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = Wallet.Of(new Money(1m, EUR), new Money(2m, USD));
        var right = Wallet.Of(new Money(2m, USD), new Money(1m, EUR));

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WhenComparedWithNull_ReturnsFalse()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act & Assert
        Assert.False(sut.Equals((Wallet?)null));
        Assert.False(sut.Equals((object?)null));
    }

    [Fact]
    public void Equals_WhenComparedWithDifferentType_ReturnsFalse()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act & Assert
        Assert.False(sut.Equals((object)"some string"));
    }

    [Fact]
    public void CompareTo_WhenContextedMoneyIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act
        var result = sut.CompareTo((ContextedMoney?)null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WhenContextedWalletIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act
        var result = sut.CompareTo((ContextedWallet?)null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WhenWalletTotalEqualsContextedMoney_ReturnsZero()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates); // EUR->USD=2
        var sut = Wallet.Of(new Money(50m, EUR), new Money(50m, USD)); // 50 EUR + 50 USD = 50+25 = 75 EUR = 150 USD
        var other = new Money(150m, USD).In(context);

        // Act
        var result = sut.CompareTo(other);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WhenBothTotalsConvertedToSameCurrency_ComparesTotals()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates); // EUR->USD=2
        var sut = Wallet.Of(new Money(10m, EUR));
        var other = Wallet.Of(new Money(9m, EUR), new Money(1m, USD)).In(context); // 9 EUR + 0.5 EUR = 9.50 EUR

        // Act
        var result = sut.CompareTo(other);

        // Assert
        Assert.True(result > 0); // 10 EUR > 9.50 EUR
    }

    [Fact]
    public void EqualityOperators_WithContextedWallet_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var wallet = Wallet.Of(new Money(10m, EUR));
        var equal = Wallet.Of(new Money(10m, EUR)).In(context);
        var different = Wallet.Of(new Money(11m, EUR)).In(context);

        // Act & Assert
        Assert.True(wallet == equal);
        Assert.False(wallet != equal);
        Assert.False(wallet == different);
        Assert.True(wallet != different);

        // Reversed
        Assert.True(equal == wallet);
        Assert.False(different == wallet);
    }

    [Fact]
    public void EqualityOperators_WithContextedMoney_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates); // EUR->USD=2
        var wallet = Wallet.Of(new Money(50m, EUR), new Money(50m, USD));
        var equal = new Money(150m, USD).In(context);
        var different = new Money(151m, USD).In(context);

        // Act & Assert
        Assert.True(wallet == equal);
        Assert.False(wallet != equal);
        Assert.False(wallet == different);
        Assert.True(wallet != different);

        // Reversed
        Assert.True(equal == wallet);
        Assert.False(different == wallet);
    }

    [Fact]
    public void OrderOperators_WithContextedMoney_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var wallet = Wallet.Of(new Money(10m, EUR));
        var smaller = new Money(5m, EUR).In(context);
        var equal = new Money(10m, EUR).In(context);
        var larger = new Money(15m, EUR).In(context);

        // Act & Assert
        Assert.True(wallet > smaller);
        Assert.True(wallet >= equal);
        Assert.True(wallet < larger);
        Assert.True(wallet <= equal);

        Assert.True(smaller < wallet);
        Assert.True(larger > wallet);
    }

    [Fact]
    public void OrderOperators_WithContextedWallet_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var sut = Wallet.Of(new Money(10m, EUR));
        var smaller = Wallet.Of(new Money(5m, EUR)).In(context);
        var equal = Wallet.Of(new Money(10m, EUR)).In(context);
        var larger = Wallet.Of(new Money(15m, EUR)).In(context);

        // Act & Assert
        Assert.True(sut > smaller);
        Assert.True(sut >= equal);
        Assert.True(sut < larger);
        Assert.True(sut <= equal);
    }

    [Fact]
    public void ComparisonOperators_WhenAnyReferenceOperandIsNull_ReturnExpectedResult()
    {
        // Arrange
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        Wallet walletNull = null!;
        ContextedMoney contextedMoneyNull = null!;
        ContextedWallet contextedWalletNull = null!;
        var wallet = Wallet.Of(new Money(1m, EUR));
        var contextedMoney = new Money(1m, USD).In(context);
        var contextedWallet = Wallet.Of(new Money(1m, USD)).In(context);

        // Act & Assert — null wallet against contexted types
        Assert.False(walletNull == contextedMoney);
        Assert.True(walletNull != contextedMoney);
        Assert.False(walletNull < contextedMoney);
        Assert.False(walletNull <= contextedMoney);
        Assert.False(walletNull > contextedMoney);
        Assert.False(walletNull >= contextedMoney);

        Assert.False(walletNull == contextedWallet);
        Assert.True(walletNull != contextedWallet);
        Assert.False(walletNull < contextedWallet);

        // null ContextedMoney/ContextedWallet against wallet
        Assert.False(wallet == contextedMoneyNull);
        Assert.True(wallet != contextedMoneyNull);
        Assert.False(wallet < contextedMoneyNull);
        Assert.False(wallet <= contextedMoneyNull);
        Assert.False(wallet > contextedMoneyNull);
        Assert.False(wallet >= contextedMoneyNull);

        Assert.False(wallet == contextedWalletNull);
        Assert.True(wallet != contextedWalletNull);
        Assert.False(wallet < contextedWalletNull);

        // Reversed
        Assert.False(contextedMoneyNull == wallet);
        Assert.True(contextedMoneyNull != wallet);
        Assert.False(contextedMoney == walletNull);
        Assert.True(contextedMoney != walletNull);
    }
}
