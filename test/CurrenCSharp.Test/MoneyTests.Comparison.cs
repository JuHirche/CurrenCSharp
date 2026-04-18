using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public void CompareTo_WhenComparingWithMoneyOfSameCurrency_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = left.CompareTo(right);

        // Assert
        Assert.Equal(left.Amount.CompareTo(right.Amount), result);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task CompareTo_WhenComparingWithContextedMoney_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, USD).In(context);
        var convertedRight = rightAmount / (decimal)LatestExchangeRates[USD];

        // Act
        var result = left.CompareTo(right);

        // Assert
        Assert.Equal(left.Amount.CompareTo(convertedRight), result);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task CompareTo_WhenComparingWithContextedWallet_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var left = new Money(leftAmount, EUR);
        var right = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var convertedRight = rightAmount / (decimal)LatestExchangeRates[USD];

        // Act
        var result = left.CompareTo(right);

        // Assert
        Assert.Equal(left.Amount.CompareTo(convertedRight), result);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task EqualityOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyContextedMoney = leftMoney == rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney == rightMoney;
        var moneyContextedWallet = leftMoney == rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet == rightMoney;

        // Assert
        Assert.Equal(leftAmount == rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate == rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount == rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate == rightAmount, contextedWalletMoney);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task InequalityOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyContextedMoney = leftMoney != rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney != rightMoney;
        var moneyContextedWallet = leftMoney != rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet != rightMoney;

        // Assert
        Assert.Equal(leftAmount != rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate != rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount != rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate != rightAmount, contextedWalletMoney);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task EqualityOperator_WhenOperandsAreSwapped_ReturnsSameResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var money = new Money(leftAmount, EUR);
        var contextedMoney = new Money(rightAmount, USD).In(context);
        var contextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);

        // Act & Assert
        Assert.Equal(money == contextedMoney, contextedMoney == money);
        Assert.Equal(money != contextedMoney, contextedMoney != money);
        Assert.Equal(money == contextedWallet, contextedWallet == money);
        Assert.Equal(money != contextedWallet, contextedWallet != money);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task LessThanOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyMoney = leftMoney < rightMoney;
        var moneyContextedMoney = leftMoney < rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney < rightMoney;
        var moneyContextedWallet = leftMoney < rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet < rightMoney;

        // Assert
        Assert.Equal(leftAmount < rightAmount, moneyMoney);
        Assert.Equal(leftAmount < rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate < rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount < rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate < rightAmount, contextedWalletMoney);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task LessThanOrEqualOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyMoney = leftMoney <= rightMoney;
        var moneyContextedMoney = leftMoney <= rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney <= rightMoney;
        var moneyContextedWallet = leftMoney <= rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet <= rightMoney;

        // Assert
        Assert.Equal(leftAmount <= rightAmount, moneyMoney);
        Assert.Equal(leftAmount <= rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate <= rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount <= rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate <= rightAmount, contextedWalletMoney);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task GreaterThanOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyMoney = leftMoney > rightMoney;
        var moneyContextedMoney = leftMoney > rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney > rightMoney;
        var moneyContextedWallet = leftMoney > rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet > rightMoney;

        // Assert
        Assert.Equal(leftAmount > rightAmount, moneyMoney);
        Assert.Equal(leftAmount > rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate > rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount > rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate > rightAmount, contextedWalletMoney);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task GreaterThanOrEqualOperator_WhenComparingAcrossSupportedOverloads_ReturnsExpectedResult(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var leftMoney = new Money(leftAmount, EUR);
        var rightMoney = new Money(rightAmount, EUR);
        var leftContextedMoney = new Money(leftAmount, USD).In(context);
        var rightContextedMoney = new Money(rightAmount, USD).In(context);
        var leftContextedWallet = Wallet.Of(new Money(leftAmount, USD)).In(context);
        var rightContextedWallet = Wallet.Of(new Money(rightAmount, USD)).In(context);
        var exchangeRate = (decimal)LatestExchangeRates[USD];

        // Act
        var moneyMoney = leftMoney >= rightMoney;
        var moneyContextedMoney = leftMoney >= rightContextedMoney;
        var contextedMoneyMoney = leftContextedMoney >= rightMoney;
        var moneyContextedWallet = leftMoney >= rightContextedWallet;
        var contextedWalletMoney = leftContextedWallet >= rightMoney;

        // Assert
        Assert.Equal(leftAmount >= rightAmount, moneyMoney);
        Assert.Equal(leftAmount >= rightAmount / exchangeRate, moneyContextedMoney);
        Assert.Equal(leftAmount / exchangeRate >= rightAmount, contextedMoneyMoney);
        Assert.Equal(leftAmount >= rightAmount / exchangeRate, moneyContextedWallet);
        Assert.Equal(leftAmount / exchangeRate >= rightAmount, contextedWalletMoney);
    }

    [Fact]
    public void CompareTo_WhenComparingWithMoneyOfDifferentCurrency_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(10m, USD);

        // Act
        var exception = Record.Exception(() => _ = left.CompareTo(right));

        // Assert
        Assert.IsType<DifferentCurrencyException>(exception);
    }
}
