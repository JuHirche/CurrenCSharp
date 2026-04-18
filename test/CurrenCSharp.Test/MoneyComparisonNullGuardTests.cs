using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed class MoneyComparisonNullGuardTests : TestFixture
{
    private readonly ExchangeRateContext _context = new(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

    [Fact]
    public void ComparisonOperators_WhenAnyMoneyOperandIsDefault_ThrowNoCurrencyException()
    {
        // Arrange
        Money leftDefault = default;
        Money rightDefault = default;
        var value = new Money(1m, EUR);

        // Act & Assert
        Assert.Throws<NoCurrencyException>(() => _ = leftDefault < value);
        Assert.Throws<NoCurrencyException>(() => _ = leftDefault <= value);
        Assert.Throws<NoCurrencyException>(() => _ = leftDefault > value);
        Assert.Throws<NoCurrencyException>(() => _ = leftDefault >= value);
        Assert.Throws<NoCurrencyException>(() => _ = value < rightDefault);
        Assert.Throws<NoCurrencyException>(() => _ = value <= rightDefault);
        Assert.Throws<NoCurrencyException>(() => _ = value > rightDefault);
        Assert.Throws<NoCurrencyException>(() => _ = value >= rightDefault);
    }

    [Fact]
    public void ComparisonOperators_WhenAnyContextedMoneyOperandIsNull_ReturnsExpectedResult()
    {
        // Arrange
        Money moneyDefault = default;
        ContextedMoney contextedMoneyNull = null!;
        var money = new Money(1m, EUR);
        var contextedMoney = new Money(1m, USD).In(_context);

        // Act & Assert
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault == contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault != contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault < contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault <= contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault > contextedMoney);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault >= contextedMoney);
        Assert.False(money == contextedMoneyNull);
        Assert.True(money != contextedMoneyNull);
        Assert.False(money < contextedMoneyNull);
        Assert.False(money <= contextedMoneyNull);
        Assert.True(money > contextedMoneyNull);
        Assert.True(money >= contextedMoneyNull);
        Assert.False(contextedMoneyNull == money);
        Assert.True(contextedMoneyNull != money);
        Assert.True(contextedMoneyNull < money);
        Assert.True(contextedMoneyNull <= money);
        Assert.False(contextedMoneyNull > money);
        Assert.False(contextedMoneyNull >= money);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney == moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney != moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney < moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney <= moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney > moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedMoney >= moneyDefault);
    }

    [Fact]
    public void ComparisonOperators_WhenAnyContextedWalletOperandIsNull_ReturnsExpectedResult()
    {
        // Arrange
        Money moneyDefault = default;
        ContextedWallet contextedWalletNull = null!;
        var money = new Money(1m, EUR);
        var contextedWallet = Wallet.Of(new Money(1m, USD)).In(_context);

        // Act & Assert
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault == contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault != contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault < contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault <= contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault > contextedWallet);
        Assert.Throws<NoCurrencyException>(() => _ = moneyDefault >= contextedWallet);
        Assert.False(money == contextedWalletNull);
        Assert.True(money != contextedWalletNull);
        Assert.False(money < contextedWalletNull);
        Assert.False(money <= contextedWalletNull);
        Assert.True(money > contextedWalletNull);
        Assert.True(money >= contextedWalletNull);
        Assert.False(contextedWalletNull == money);
        Assert.True(contextedWalletNull != money);
        Assert.True(contextedWalletNull < money);
        Assert.True(contextedWalletNull <= money);
        Assert.False(contextedWalletNull > money);
        Assert.False(contextedWalletNull >= money);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet == moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet != moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet < moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet <= moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet > moneyDefault);
        Assert.Throws<NoCurrencyException>(() => _ = contextedWallet >= moneyDefault);
    }

    [Fact]
    public void CompareTo_WhenOtherContextedOperandIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act
        var compareContextedMoney = sut.CompareTo((ContextedMoney)null!);
        var compareContextedWallet = sut.CompareTo((ContextedWallet)null!);

        // Assert
        Assert.True(compareContextedMoney > 0);
        Assert.True(compareContextedWallet > 0);
    }
}
