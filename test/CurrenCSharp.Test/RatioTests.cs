namespace CurrenCSharp.Test;

public sealed class RatioTests
{
    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    public void Constructor_WhenValueIsNegative_ThrowsArgumentOutOfRangeException(decimal value)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Ratio(value));
    }

    [Fact]
    public void Constructor_WhenValueIsDecimalMinValue_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Ratio(decimal.MinValue));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1.5)]
    public void Constructor_WhenValueIsNonNegative_Succeeds(decimal value)
    {
        // Act
        var ratio = new Ratio(value);

        // Assert
        Assert.Equal(value, (decimal)ratio);
    }

    [Fact]
    public void Constructor_WhenValueIsDecimalMaxValue_Succeeds()
    {
        // Act
        var ratio = new Ratio(decimal.MaxValue);

        // Assert
        Assert.Equal(decimal.MaxValue, (decimal)ratio);
    }

    [Fact]
    public void CompareTo_WhenOtherIsNull_ReturnsPositiveValue()
    {
        // Arrange
        var ratio = new Ratio(1m);

        // Act
        var result = ratio.CompareTo(null);

        // Assert
        Assert.True(result > 0);
    }

    public static TheoryData<decimal, decimal, int> CompareToData => new()
    {
        { 1m, 1m, 0 },
        { 1m, 2m, -1 },
        { 2m, 1m, 1 },
    };

    [Theory]
    [MemberData(nameof(CompareToData))]
    public void CompareTo_WhenOtherIsEqualOrLessOrGreater_ReturnsExpectedSign(
        decimal left, decimal right, int expectedSign)
    {
        // Arrange
        var leftRatio = new Ratio(left);
        var rightRatio = new Ratio(right);

        // Act
        var result = leftRatio.CompareTo(rightRatio);

        // Assert
        Assert.Equal(expectedSign, Math.Sign(result));
    }

    [Theory]
    [InlineData(2.25)]
    [InlineData(0)]
    [InlineData(100)]
    public void Conversions_WhenRoundTripped_PreserveValue(decimal value)
    {
        // Act
        Ratio ratio = value;
        decimal roundTripped = ratio;

        // Assert
        Assert.Equal(value, roundTripped);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new Ratio(1.5m);
        var right = new Ratio(1.5m);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
