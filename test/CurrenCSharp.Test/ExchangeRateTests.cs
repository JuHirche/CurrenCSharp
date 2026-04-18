namespace CurrenCSharp.Test;

public sealed class ExchangeRateTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WhenValueIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException(decimal value)
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = new ExchangeRate(value));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void ExplicitDecimalConversion_WhenExchangeRateIsValid_ReturnsUnderlyingValue()
    {
        // Arrange
        var sut = new ExchangeRate(1.25m);

        // Act
        var result = (decimal)sut;

        // Assert
        Assert.Equal(1.25m, result);
    }
}
