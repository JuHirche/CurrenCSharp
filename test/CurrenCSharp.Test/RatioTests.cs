namespace CurrenCSharp.Test;

public sealed class RatioTests
{
    [Fact]
    public void Constructor_WhenValueIsNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = new Ratio(-0.01m));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void CompareTo_WhenOtherIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var sut = new Ratio(1m);

        // Act
        var result = sut.CompareTo(null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void ImplicitDecimalConversion_WhenRatioIsProvided_ReturnsUnderlyingValue()
    {
        // Arrange
        var sut = new Ratio(3.5m);

        // Act
        decimal result = sut;

        // Assert
        Assert.Equal(3.5m, result);
    }

    [Fact]
    public void ImplicitRatioConversion_WhenDecimalIsProvided_CreatesRatio()
    {
        // Arrange
        decimal value = 2.25m;

        // Act
        Ratio result = value;

        // Assert
        Assert.Equal(value, (decimal)result);
    }
}
