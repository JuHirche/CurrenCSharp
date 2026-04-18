namespace CurrenCSharp.Test;

public sealed class WalletComparisonNullGuardTests : TestFixture
{
    private readonly ExchangeRateContext _context = new(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

    [Fact]
    public void ComparisonOperators_WhenAnyContextedMoneyOperandIsNull_ReturnsFalse()
    {
        // Arrange
        Wallet walletNull = null!;
        ContextedMoney contextedMoneyNull = null!;
        var wallet = Wallet.Of(new Money(1m, EUR));
        var contextedMoney = new Money(1m, USD).In(_context);

        // Act & Assert
        Assert.False(walletNull == contextedMoney);
        Assert.True(walletNull != contextedMoney);
        Assert.False(walletNull < contextedMoney);
        Assert.False(walletNull <= contextedMoney);
        Assert.False(walletNull > contextedMoney);
        Assert.False(walletNull >= contextedMoney);
        Assert.False(wallet == contextedMoneyNull);
        Assert.True(wallet != contextedMoneyNull);
        Assert.False(wallet < contextedMoneyNull);
        Assert.False(wallet <= contextedMoneyNull);
        Assert.False(wallet > contextedMoneyNull);
        Assert.False(wallet >= contextedMoneyNull);
        Assert.False(contextedMoneyNull == wallet);
        Assert.True(contextedMoneyNull != wallet);
        Assert.False(contextedMoneyNull < wallet);
        Assert.False(contextedMoneyNull <= wallet);
        Assert.False(contextedMoneyNull > wallet);
        Assert.False(contextedMoneyNull >= wallet);
        Assert.False(contextedMoney == walletNull);
        Assert.True(contextedMoney != walletNull);
        Assert.False(contextedMoney < walletNull);
        Assert.False(contextedMoney <= walletNull);
        Assert.False(contextedMoney > walletNull);
        Assert.False(contextedMoney >= walletNull);
    }

    [Fact]
    public void ComparisonOperators_WhenAnyContextedWalletOperandIsNull_ReturnsFalse()
    {
        // Arrange
        Wallet walletNull = null!;
        ContextedWallet contextedWalletNull = null!;
        var wallet = Wallet.Of(new Money(1m, EUR));
        var contextedWallet = Wallet.Of(new Money(1m, USD)).In(_context);

        // Act & Assert
        Assert.False(walletNull == contextedWallet);
        Assert.True(walletNull != contextedWallet);
        Assert.False(walletNull < contextedWallet);
        Assert.False(walletNull <= contextedWallet);
        Assert.False(walletNull > contextedWallet);
        Assert.False(walletNull >= contextedWallet);
        Assert.False(wallet == contextedWalletNull);
        Assert.True(wallet != contextedWalletNull);
        Assert.False(wallet < contextedWalletNull);
        Assert.False(wallet <= contextedWalletNull);
        Assert.False(wallet > contextedWalletNull);
        Assert.False(wallet >= contextedWalletNull);
        Assert.False(contextedWalletNull == wallet);
        Assert.True(contextedWalletNull != wallet);
        Assert.False(contextedWalletNull < wallet);
        Assert.False(contextedWalletNull <= wallet);
        Assert.False(contextedWalletNull > wallet);
        Assert.False(contextedWalletNull >= wallet);
        Assert.False(contextedWallet == walletNull);
        Assert.True(contextedWallet != walletNull);
        Assert.False(contextedWallet < walletNull);
        Assert.False(contextedWallet <= walletNull);
        Assert.False(contextedWallet > walletNull);
        Assert.False(contextedWallet >= walletNull);
    }

    [Fact]
    public void CompareTo_WhenOtherIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act
        var compareContextedMoney = sut.CompareTo((ContextedMoney)null!);
        var compareContextedWallet = sut.CompareTo((ContextedWallet)null!);

        // Assert
        Assert.True(compareContextedMoney > 0);
        Assert.True(compareContextedWallet > 0);
    }
}
