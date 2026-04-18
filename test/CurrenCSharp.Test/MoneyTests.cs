namespace CurrenCSharp.Test;

public partial class MoneyTests : TestFixture
{
    [Fact]
    public void Zero_WhenCurrencyIsNotProvided_ReturnsMoneyWithZeroAmountAndDefaultCurrency()
    {
        // Arrange & Act
        var result = Money.Zero();

        // Assert
        Assert.Equal(decimal.Zero, result.Amount);
        Assert.Equal(Currency.Default, result.Currency);
    }

    [Fact]
    public void Zero_WhenCurrencyIsProvided_ReturnsMoneyWithZeroAmountAndProvidedCurrency()
    {
        // Arrange
        var currency = USD;

        // Act
        var result = Money.Zero(currency);

        // Assert
        Assert.Equal(decimal.Zero, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void IsZero_WhenAmountIsNegativeZeroOrPositive_ReturnsExpectedValue(decimal amount)
    {
        // Arrange
        var sut = new Money(amount, USD);

        // Act
        var result = sut.IsZero;

        // Assert
        Assert.Equal(amount == decimal.Zero, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void IsPositive_WhenAmountIsNegativeZeroOrPositive_ReturnsExpectedValue(decimal amount)
    {
        // Arrange
        var sut = new Money(amount, USD);

        // Act
        var result = sut.IsPositive;

        // Assert
        Assert.Equal(amount > decimal.Zero, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void IsNegative_WhenAmountIsNegativeZeroOrPositive_ReturnsExpectedValue(decimal amount)
    {
        // Arrange
        var sut = new Money(amount, USD);

        // Act
        var result = sut.IsNegative;

        // Assert
        Assert.Equal(amount < decimal.Zero, result);
    }
}
