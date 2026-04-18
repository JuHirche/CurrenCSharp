namespace CurrenCSharp.Test;

public sealed partial class WalletTests
{
    [Fact]
    public void Addition_WhenWalletsContainSameCurrency_CombinesAmounts()
    {
        // Arrange
        var left = Wallet.Of(new Money(10m, EUR), new Money(2m, USD));
        var right = Wallet.Of(new Money(5m, EUR), new Money(3m, USD));

        // Act
        var result = left + right;

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(15m, Assert.Single(result, money => money.Currency == EUR).Amount);
        Assert.Equal(5m, Assert.Single(result, money => money.Currency == USD).Amount);
    }

    [Fact]
    public void Subtraction_WhenResultingCurrencyAmountIsZero_RemovesCurrencyEntry()
    {
        // Arrange
        var left = Wallet.Of(new Money(5m, EUR));
        var right = Wallet.Of(new Money(5m, EUR));

        // Act
        var result = left - right;

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Multiplication_WhenFactorIsApplied_ScalesAllEntries()
    {
        // Arrange
        var sut = Wallet.Of(new Money(2m, EUR), new Money(3m, USD));

        // Act
        var result = sut * 2m;

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(4m, Assert.Single(result, money => money.Currency == EUR).Amount);
        Assert.Equal(6m, Assert.Single(result, money => money.Currency == USD).Amount);
    }

    [Fact]
    public void Division_WhenDivisorIsNotZero_ScalesAllEntries()
    {
        // Arrange
        var sut = Wallet.Of(new Money(6m, EUR), new Money(3m, USD));

        // Act
        var result = sut / 3m;

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(2m, Assert.Single(result, money => money.Currency == EUR).Amount);
        Assert.Equal(1m, Assert.Single(result, money => money.Currency == USD).Amount);
    }

    [Fact]
    public void Division_WhenDivisorIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act
        var exception = Record.Exception(() => _ = sut / 0m);

        // Assert
        Assert.IsType<DivideByZeroException>(exception);
    }

    [Fact]
    public void UnaryMinus_WhenWalletIsNotNull_NegatesAllEntries()
    {
        // Arrange
        var sut = Wallet.Of(new Money(6m, EUR), new Money(-3m, USD));

        // Act
        var result = -sut;

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(-6m, Assert.Single(result, money => money.Currency == EUR).Amount);
        Assert.Equal(3m, Assert.Single(result, money => money.Currency == USD).Amount);
    }

    [Fact]
    public void UnaryMinus_WhenWalletIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Wallet sut = null!;

        // Act
        var exception = Record.Exception(() => _ = -sut);

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Addition_WhenLeftWalletIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Wallet left = null!;
        var right = Wallet.Of(new Money(1m, EUR));

        // Act
        var exception = Record.Exception(() => _ = left + right);

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Addition_WhenRightWalletIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var left = Wallet.Of(new Money(1m, EUR));
        Wallet right = null!;

        // Act
        var exception = Record.Exception(() => _ = left + right);

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }
}
