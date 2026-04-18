namespace CurrenCSharp.Test;

public sealed partial class WalletTests : TestFixture
{
    [Fact]
    public async Task Total_WhenWalletIsEmptyAndTargetCurrencyIsProvided_ReturnsZeroMoneyInTargetCurrency()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var sut = Wallet.Empty.In(context);

        // Act
        var result = sut.Total(USD);

        // Assert
        Assert.Equal(decimal.Zero, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public async Task Total_WhenWalletIsEmptyAndTargetCurrencyIsNotProvided_ReturnsZeroMoneyInDefaultCurrency()
    {
        // Arrange
        var context = await ExchangeRateProvider.GetLatestAsync(TestContext.Current.CancellationToken);
        var sut = Wallet.Empty.In(context);

        // Act
        var result = sut.Total();

        // Assert
        Assert.Equal(decimal.Zero, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void In_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));
        ExchangeRateContext context = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.In(context));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Of_WhenMoneyArrayIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money[] moneys = null!;

        // Act
        var exception = Record.Exception(() => _ = Wallet.Of(moneys));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Of_WhenMoneyCollectionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IReadOnlyCollection<Money> moneys = null!;

        // Act
        var exception = Record.Exception(() => _ = Wallet.Of(moneys));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }
}
