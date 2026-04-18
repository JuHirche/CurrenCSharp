namespace CurrenCSharp.Test;

public sealed class NumericCodeTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(978)]
    [InlineData(999)]
    public void Constructor_WhenValueIsInRange_SetsValue(int value)
    {
        // Arrange & Act
        var result = new NumericCode(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1000)]
    public void Constructor_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException(int value)
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = new NumericCode(value));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void Parse_WhenValueIsValid_ReturnsNumericCode()
    {
        // Arrange & Act
        var result = NumericCode.Parse("978");

        // Assert
        Assert.Equal(978, result.Value);
    }

    [Fact]
    public void Parse_WhenValueIsInvalid_ThrowsFormatException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = NumericCode.Parse("ABC"));

        // Assert
        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string value = null!;

        // Act
        var exception = Record.Exception(() => _ = NumericCode.Parse(value));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void TryParse_WhenValueIsValid_ReturnsTrueAndResult()
    {
        // Arrange & Act
        var success = NumericCode.TryParse("840", out var result);

        // Assert
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(840, result.Value);
    }

    [Fact]
    public void TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult()
    {
        // Arrange & Act
        var success = NumericCode.TryParse("1000", out var result);

        // Assert
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsThreeDigitRepresentation()
    {
        // Arrange
        var sut = new NumericCode(7);

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Equal("007", result);
    }
}
