using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed class ContextedWalletTests : TestFixture
{
    private readonly ExchangeRateContext _context = new(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

    [Fact]
    public void Total_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR)).In(_context);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.Total(null!));
    }

    [Fact]
    public void Total_WhenSingleCurrencyWallet_ReturnsTotalInSingleCurrency()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, USD), new Money(2m, USD)).In(_context);

        // Act
        var result = sut.Total();

        // Assert
        Assert.Equal(3m, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Total_WhenMultiCurrencyWallet_UsesResolvedDefaultCurrency()
    {
        // Arrange — EUR->USD=2, so 1 USD == 0.5 EUR. Wallet[1 EUR, 1 USD] in EUR = 1 + 0.5 = 1.50 EUR
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(_context);

        // Act
        var result = sut.Total();

        // Assert
        Assert.Equal(1.50m, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Total_WhenRoundResultIsFalse_ReturnsUnroundedTotal()
    {
        // Arrange — Wallet[1 EUR, 1 USD] in EUR = 1 + 1/2 = 1.5 (not rounded)
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(_context);

        // Act
        var result = sut.Total(EUR, new ConversionOptions(RoundResult: false));

        // Assert
        Assert.Equal(1.5m, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Total_WhenScaleAndModeProvided_UsesFinalRoundingOptions()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(_context);

        // Act
        var result = sut.Total(EUR,
            new ConversionOptions(RoundingMode: MidpointRounding.AwayFromZero, Scale: new Scale(4)));

        // Assert — still 1.5 EUR, but rounding-options applied
        Assert.Equal(1.5m, result.Amount);
    }

    [Fact]
    public void Total_WhenWalletIsMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD)).In(_context);
        Dispose();

        try
        {
            // Act & Assert
            Assert.Throws<NoDefaultCurrencyException>(() => sut.Total());
        }
        finally
        {
            _ = CurrenC.UseDefaultCurrency(EUR);
        }
    }

    [Fact]
    public void Total_WhenWalletIsEmpty_ReturnsZeroInResolvedCurrency()
    {
        // Arrange
        var sut = Wallet.Empty.In(_context);

        // Act
        var result = sut.Total();

        // Assert — default is EUR from TestFixture
        Assert.Equal(0m, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Properties_WhenAccessed_ReturnOriginalBoundInstances()
    {
        // Arrange
        var wallet = Wallet.Of(new Money(1m, EUR));
        var sut = wallet.In(_context);

        // Act & Assert
        Assert.Same(wallet, sut.Wallet);
        Assert.Same(_context, sut.Context);
    }
}
