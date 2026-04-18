using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    [Fact]
    public void UnaryPlus_WhenMoneyIsNotNull_ReturnsSameMoney()
    {
        // Arrange
        var sut = new Money(12.34m, USD);

        // Act
        var result = +sut;

        // Assert
        Assert.Equal(12.34m, result.Amount);
        Assert.Equal(USD, result.Currency);
        Assert.Equal(sut, result);
    }

    [Fact]
    public void UnaryPlus_WhenMoneyIsNull_ThrowsNoCurrencyException()
    {
        // Arrange
        Money sut = default;

        // Act
        var exception = Record.Exception(() => _ = +sut);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Fact]
    public void UnaryMinus_WhenMoneyIsNotNull_ReturnsMoneyWithNegatedAmount()
    {
        // Arrange
        var sut = new Money(12.34m, USD);

        // Act
        var result = -sut;

        // Assert
        Assert.Equal(-12.34m, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void UnaryMinus_WhenMoneyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money sut = default;

        // Act
        var exception = Record.Exception(() => _ = -sut);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    [InlineData(0.00, 0.00)]
    public void Addition_WhenCurrenciesMatch_ReturnsMoneyWithSummedAmount(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var left = new Money(leftAmount, USD);
        var right = new Money(rightAmount, USD);

        // Act
        var result = left + right;

        // Assert
        Assert.Equal(leftAmount + rightAmount, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Addition_WhenCurrenciesDoNotMatch_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, USD);
        var right = new Money(5m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left + right);

        // Assert
        Assert.IsType<DifferentCurrencyException>(exception);
    }

    [Fact]
    public void Addition_WhenLeftOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money left = default;
        var right = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left + right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Fact]
    public void Addition_WhenRightOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var left = new Money(1m, EUR);
        Money right = default;

        // Act
        var exception = Record.Exception(() => _ = left + right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    [InlineData(0.00, 0.00)]
    public void Subtraction_WhenCurrenciesMatch_ReturnsMoneyWithSubtractedAmount(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var left = new Money(leftAmount, USD);
        var right = new Money(rightAmount, USD);

        // Act
        var result = left - right;

        // Assert
        Assert.Equal(leftAmount - rightAmount, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Subtraction_WhenCurrenciesDoNotMatch_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, USD);
        var right = new Money(5m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left - right);

        // Assert
        Assert.IsType<DifferentCurrencyException>(exception);
    }

    [Fact]
    public void Subtraction_WhenLeftOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money left = default;
        var right = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left - right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Fact]
    public void Subtraction_WhenRightOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var left = new Money(1m, EUR);
        Money right = default;

        // Act
        var exception = Record.Exception(() => _ = left - right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    [InlineData(2.00, 0.00)]
    public void Multiplication_WhenMoneyIsLeftOperand_ReturnsMoneyWithMultipliedAmount(decimal amount, decimal multiplier)
    {
        // Arrange
        var money = new Money(amount, USD);

        // Act
        var result = money * multiplier;

        // Assert
        Assert.Equal(amount * multiplier, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    [InlineData(2.00, 0.00)]
    public void Multiplication_WhenMoneyIsRightOperand_ReturnsMoneyWithMultipliedAmount(decimal multiplier, decimal amount)
    {
        // Arrange
        var money = new Money(amount, USD);

        // Act
        var result = multiplier * money;

        // Assert
        Assert.Equal(amount * multiplier, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void Multiplication_WhenLeftMoneyOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money money = default;

        // Act
        var exception = Record.Exception(() => _ = money * 2m);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Fact]
    public void Multiplication_WhenRightMoneyOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money money = default;

        // Act
        var exception = Record.Exception(() => _ = 2m * money);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    public void DivisionByMoney_WhenCurrenciesMatch_ReturnsDecimalRatio(decimal leftAmount, decimal rightAmount)
    {
        // Arrange
        var left = new Money(leftAmount, USD);
        var right = new Money(rightAmount, USD);

        // Act
        var result = left / right;

        // Assert
        Assert.Equal(leftAmount / rightAmount, result);
    }

    [Fact]
    public void DivisionByMoney_WhenCurrenciesDoNotMatch_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, USD);
        var right = new Money(5m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left / right);

        // Assert
        Assert.IsType<DifferentCurrencyException>(exception);
    }

    [Fact]
    public void DivisionByMoney_WhenDivisorAmountIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var left = new Money(10m, USD);
        var right = Money.Zero(USD);

        // Act
        var exception = Record.Exception(() => _ = left / right);

        // Assert
        Assert.IsType<DivideByZeroException>(exception);
    }

    [Fact]
    public void DivisionByMoney_WhenLeftOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money left = default;
        var right = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = left / right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Fact]
    public void DivisionByMoney_WhenRightOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var left = new Money(1m, EUR);
        Money right = default;

        // Act
        var exception = Record.Exception(() => _ = left / right);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }

    [Theory]
    [InlineData(10.00, 5.00)]
    [InlineData(5.00, 10.00)]
    public void DivisionByDecimal_WhenDivisorIsNotZero_ReturnsMoneyWithDividedAmount(decimal amount, decimal divisor)
    {
        // Arrange
        var sut = new Money(amount, USD);

        // Act
        var result = sut / divisor;

        // Assert
        Assert.Equal(amount / divisor, result.Amount);
        Assert.Equal(USD, result.Currency);
    }

    [Fact]
    public void DivisionByDecimal_WhenDivisorIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = sut / 0m);

        // Assert
        Assert.IsType<DivideByZeroException>(exception);
    }

    [Fact]
    public void DivisionByDecimal_WhenLeftOperandIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Money sut = default;

        // Act
        var exception = Record.Exception(() => _ = sut / 2m);

        // Assert
        Assert.IsType<NoCurrencyException>(exception);
    }
}
