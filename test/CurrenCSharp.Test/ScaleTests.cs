namespace CurrenCSharp.Test;

public sealed class ScaleTests
{
    [Theory]
    [InlineData(29)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    public void Constructor_WhenValueGreaterThan28_ThrowsArgumentOutOfRangeException(byte value)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Scale(value));
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)27)]
    [InlineData((byte)28)]
    public void Constructor_WhenValueIsAtBoundary_Succeeds(byte value)
    {
        // Act
        var scale = new Scale(value);

        // Assert
        Assert.Equal(value, (byte)scale);
    }

    [Fact]
    public void Conversions_ScaleToInt_ReturnsValue()
    {
        // Arrange
        var scale = new Scale(4);

        // Act
        int intValue = scale;

        // Assert
        Assert.Equal(4, intValue);
    }

    [Fact]
    public void Conversions_ScaleToByte_ReturnsValue()
    {
        // Arrange
        var scale = new Scale(6);

        // Act
        byte byteValue = (byte)scale;

        // Assert
        Assert.Equal(6, byteValue);
    }

    [Fact]
    public void Conversions_ByteToScale_ReturnsScale()
    {
        // Arrange
        byte value = 10;

        // Act
        Scale scale = value;

        // Assert
        Assert.Equal(value, (byte)scale);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new Scale(4);
        var right = new Scale(4);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
