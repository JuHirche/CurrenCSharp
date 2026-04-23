using System.Collections;
using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed partial class WalletTests : TestFixture
{
    [Fact]
    public void Empty_WhenAccessed_ReturnsWalletWithoutEntries()
    {
        // Act
        var sut = Wallet.Empty;

        // Assert
        Assert.Empty(sut);
    }


    [Fact]
    public void Of_WhenMoneysIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Wallet.Of((Money[])null!));
        Assert.Throws<ArgumentNullException>(() => Wallet.Of((IReadOnlyCollection<Money>)null!));
    }


    [Fact]
    public void Of_WhenInputIsEmpty_ReturnsEmptyWallet()
    {
        // Act & Assert
        Assert.Empty(Wallet.Of());
        Assert.Empty(Wallet.Of(new List<Money>()));
    }

    [Fact]
    public void Of_WhenSameCurrencyAppearsMultipleTimes_AggregatesAmounts()
    {
        // Act
        var sut = Wallet.Of(new Money(1m, EUR), new Money(2m, EUR), new Money(3m, EUR));

        // Assert
        var single = Assert.Single(sut);
        Assert.Equal(6m, single.Amount);
        Assert.Equal(EUR, single.Currency);
    }

    [Fact]
    public void Of_WhenAmountsCancelOut_ReturnsEmptyWallet()
    {
        // Act
        var sut = Wallet.Of(new Money(10m, EUR), new Money(-10m, EUR));

        // Assert
        Assert.Empty(sut);
    }

    [Fact]
    public void In_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.In(null!));
    }

    [Fact]
    public void ToBuilder_WhenWalletIsMutatedThroughBuilder_DoesNotMutateOriginalWallet()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(2m, USD));
        var builder = sut.ToBuilder();

        // Act
        builder.Add(new Money(10m, JPY));

        // Assert
        Assert.Equal(2, sut.Count());
        Assert.DoesNotContain(sut, m => m.Currency.Equals(JPY));
    }

    [Fact]
    public void GetEnumerator_WhenWalletHasEntries_YieldsAllMoneys()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(2m, USD));

        // Act
        var items = sut.ToList();

        // Assert
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public void GetEnumerator_WhenWalletIsEmpty_YieldsNothing()
    {
        // Act
        var items = Wallet.Empty.ToList();

        // Assert
        Assert.Empty(items);
    }

    [Fact]
    public void NonGenericEnumerator_WhenIterated_ReturnsSameResultsAsGenericEnumerator()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(2m, USD));

        // Act
        var generic = sut.ToList();
        var nonGeneric = new List<object>();
        foreach (var item in (IEnumerable)sut)
            nonGeneric.Add(item);

        // Assert
        Assert.Equal(generic.Count, nonGeneric.Count);
        Assert.All(nonGeneric, o => Assert.IsType<Money>(o));
    }

    [Fact]
    public void ResolveCurrency_WhenSingleCurrencyWallet_ReturnsThatCurrency()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, USD), new Money(2m, USD));

        // Act — resolved via single-currency Total
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var total = sut.In(context).Total();

        // Assert
        Assert.Equal(USD, total.Currency);
    }

    [Fact]
    public void ResolveCurrency_WhenMultiCurrencyWallet_UsesDefaultCurrency()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD));

        // Act
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        var total = sut.In(context).Total();

        // Assert — default is EUR from TestFixture
        Assert.Equal(EUR, total.Currency);
    }

    [Fact]
    public void ResolveCurrency_WhenMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(1m, USD));
        var context = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);
        Dispose();

        try
        {
            // Act & Assert
            Assert.Throws<NoDefaultCurrencyException>(() => sut.In(context).Total());
        }
        finally
        {
            _ = CurrenC.UseDefaultCurrency(EUR);
        }
    }
}
