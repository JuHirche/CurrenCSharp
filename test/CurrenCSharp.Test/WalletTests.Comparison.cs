namespace CurrenCSharp.Test;

public sealed partial class WalletTests
{
    [Fact]
    public async Task CompareTo_WhenComparingWithContextedMoney_ReturnsExpectedOrdering()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var sut = Wallet.Of(new Money(1m, EUR));
        var other = new Money(1.5m, USD).In(context);

        // Act
        var result = sut.CompareTo(other);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public async Task CompareTo_WhenComparingWithContextedWallet_ReturnsExpectedOrdering()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var sut = Wallet.Of(new Money(1m, EUR));
        var other = Wallet.Of(new Money(1m, USD)).In(context);

        // Act
        var result = sut.CompareTo(other);

        // Assert
        Assert.True(result > 0);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task EqualityOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftWallet = Wallet.Of(new Money(leftAmount, EUR));
        var rightWallet = Wallet.Of(new Money(rightAmount, EUR));
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var walletContextedWallet = leftWallet == rightContextedWallet;
        var contextedWalletWallet = leftContextedWallet == rightWallet;
        var walletContextedMoney = leftWallet == rightContextedMoney;
        var contextedMoneyWallet = leftContextedMoney == rightWallet;

        // Assert
        Assert.Equal(leftAmount == rightAmount / exchangeRate, walletContextedWallet);
        Assert.Equal(leftAmount / exchangeRate == rightAmount, contextedWalletWallet);
        Assert.Equal(leftAmount == rightAmount / exchangeRate, walletContextedMoney);
        Assert.Equal(leftAmount / exchangeRate == rightAmount, contextedMoneyWallet);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task InequalityOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftWallet = Wallet.Of(new Money(leftAmount, EUR));
        var rightWallet = Wallet.Of(new Money(rightAmount, EUR));
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var walletContextedWallet = leftWallet != rightContextedWallet;
        var contextedWalletWallet = leftContextedWallet != rightWallet;
        var walletContextedMoney = leftWallet != rightContextedMoney;
        var contextedMoneyWallet = leftContextedMoney != rightWallet;

        // Assert
        Assert.Equal(leftAmount != rightAmount / exchangeRate, walletContextedWallet);
        Assert.Equal(leftAmount / exchangeRate != rightAmount, contextedWalletWallet);
        Assert.Equal(leftAmount != rightAmount / exchangeRate, walletContextedMoney);
        Assert.Equal(leftAmount / exchangeRate != rightAmount, contextedMoneyWallet);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task EqualityOperator_WhenOperandsAreSwapped_ReturnsSameResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var wallet = Wallet.Of(new Money(leftAmount, EUR));
        var contextedMoney = new Money(rightAmount, USD).In(context);
        var contextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);

        // Act & Assert
        Assert.Equal(wallet == contextedWallet, contextedWallet == wallet);
        Assert.Equal(wallet != contextedWallet, contextedWallet != wallet);
        Assert.Equal(wallet == contextedMoney, contextedMoney == wallet);
        Assert.Equal(wallet != contextedMoney, contextedMoney != wallet);
    }

    [Fact]
    public async Task LessThanOperator_WhenWalletTotalIsLessThanContextedMoney_ReturnsTrue()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var left = Wallet.Of(new Money(1m, EUR));
        var right = new Money(3m, USD).In(context);

        // Act
        var result = left < right;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GreaterThanOperator_WhenWalletTotalIsGreaterThanContextedWalletTotal_ReturnsTrue()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var left = Wallet.Of(new Money(2m, EUR));
        var right = Wallet.Of(new Money(1m, USD)).In(context);

        // Act
        var result = left > right;

        // Assert
        Assert.True(result);
    }
}
