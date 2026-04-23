using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    public static TheoryData<decimal, decimal, decimal> AdditionData => new()
    {
        { 10m, 5m, 15m },
        { 0m, 0m, 0m },
        { 10m, -5m, 5m },
    };

    [Theory]
    [MemberData(nameof(AdditionData))]
    public void Addition_WhenCurrenciesMatch_ReturnsSum(decimal leftAmount, decimal rightAmount, decimal expected)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = left + right;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Addition_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(5m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => left + right);
    }

    public static TheoryData<decimal, decimal, decimal> SubtractionData => new()
    {
        { 10m, 5m, 5m },
        { 0m, 0m, 0m },
        { 5m, 10m, -5m },
    };

    [Theory]
    [MemberData(nameof(SubtractionData))]
    public void Subtraction_WhenCurrenciesMatch_ReturnsDifference(decimal leftAmount, decimal rightAmount, decimal expected)
    {
        // Arrange
        var left = new Money(leftAmount, EUR);
        var right = new Money(rightAmount, EUR);

        // Act
        var result = left - right;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void Subtraction_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(5m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => left - right);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(-5)]
    public void UnaryPlus_WhenCalled_ReturnsSameAmount(decimal amount)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = +sut;

        // Assert
        Assert.Equal(amount, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Theory]
    [InlineData(10, -10)]
    [InlineData(0, 0)]
    [InlineData(-5, 5)]
    public void UnaryNegation_WhenCalled_ReturnsNegatedAmount(decimal amount, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = -sut;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    public static TheoryData<decimal, decimal, decimal> MultiplicationData => new()
    {
        { 10m, 2m, 20m },
        { 10m, 0m, 0m },
        { 10m, -1m, -10m },
        { 10m, 1m, 10m },
    };

    [Theory]
    [MemberData(nameof(MultiplicationData))]
    public void Multiplication_WhenFactorIsScalar_ReturnsScaledMoney(decimal amount, decimal factor, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = sut * factor;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Theory]
    [InlineData(2, 10, 20)]
    [InlineData(0, 10, 0)]
    public void Multiplication_WhenOrderIsReversed_ReturnsSameResult(decimal factor, decimal amount, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = factor * sut;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Theory]
    [InlineData(10, 5, 2)]
    [InlineData(10, 4, 2.5)]
    public void DivisionByMoney_WhenCurrenciesMatch_ReturnsRatio(decimal dividend, decimal divisor, decimal expected)
    {
        // Arrange
        var left = new Money(dividend, EUR);
        var right = new Money(divisor, EUR);

        // Act
        var result = left / right;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DivisionByMoney_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(5m, USD);

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => left / right);
    }

    [Fact]
    public void DivisionByMoney_WhenDivisorIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var left = new Money(10m, EUR);
        var right = new Money(0m, EUR);

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => left / right);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(10, 4, 2.5)]
    public void DivisionByDecimal_WhenDivisorIsScalar_ReturnsScaledMoney(decimal amount, decimal divisor, decimal expected)
    {
        // Arrange
        var sut = new Money(amount, EUR);

        // Act
        var result = sut / divisor;

        // Assert
        Assert.Equal(expected, result.Amount);
        Assert.Equal(EUR, result.Currency);
    }

    [Fact]
    public void DivisionByDecimal_WhenDivisorIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => sut / 0m);
    }
}
