namespace CurrenCSharp.Test;

public sealed class ExchangeRateTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WhenValueIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException(decimal value)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExchangeRate(value));
    }

    [Fact]
    public void Constructor_WhenValueIsDecimalMinValue_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExchangeRate(decimal.MinValue));
    }

    [Theory]
    [InlineData(0.00000001)]
    [InlineData(1)]
    [InlineData(1.25)]
    public void Constructor_WhenValueIsPositive_Succeeds(decimal value)
    {
        // Act
        var rate = new ExchangeRate(value);

        // Assert
        Assert.Equal(value, (decimal)rate);
    }

    [Theory]
    [InlineData(1.25)]
    [InlineData(2)]
    [InlineData(0.5)]
    public void ExplicitDecimalConversion_WhenExchangeRateIsValid_ReturnsUnderlyingValue(decimal value)
    {
        // Arrange
        var rate = new ExchangeRate(value);

        // Act
        var result = (decimal)rate;

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new ExchangeRate(1.25m);
        var right = new ExchangeRate(1.25m);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
