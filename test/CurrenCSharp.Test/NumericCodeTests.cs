namespace CurrenCSharp.Test;

public sealed class NumericCodeTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(1000)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Constructor_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException(int value)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new NumericCode(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(999)]
    public void Constructor_WhenValueIsAtBoundary_Succeeds(int value)
    {
        // Act
        var code = new NumericCode(value);

        // Assert
        Assert.Equal(value, code.Value);
    }

    [Theory]
    [InlineData("978", 978)]
    [InlineData("840", 840)]
    [InlineData("007", 7)]
    public void Parse_WhenValueIsValid_ReturnsNumericCode(string value, int expected)
    {
        // Act
        var result = NumericCode.Parse(value);

        // Assert
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("1000")]
    public void Parse_WhenValueIsInvalid_ThrowsFormatException(string value)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(value));
    }

    [Fact]
    public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => NumericCode.Parse(null!));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("-978")]
    public void Parse_WhenInputIsNegative_ThrowsFormatException(string value)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(value));
    }

    [Theory]
    [InlineData(" 978")]
    [InlineData("+978")]
    [InlineData("\u0669\u0667\u0668")] // Arabic-Indic 978
    public void Parse_WhenInputIsNonCanonical_RejectsAmbiguousFormats(string value)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(value));
    }

    [Theory]
    [InlineData("007", 7)]
    [InlineData("840", 840)]
    [InlineData("000", 0)]
    [InlineData("999", 999)]
    public void TryParse_WhenValueIsValid_ReturnsTrueAndResult(string value, int expected)
    {
        // Act
        var success = NumericCode.TryParse(value, out var result);

        // Assert
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(expected, result!.Value);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("1000")]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData(null)]
    public void TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult(string? value)
    {
        // Act
        var success = NumericCode.TryParse(value, out var result);

        // Assert
        Assert.False(success);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(7, "007")]
    [InlineData(42, "042")]
    [InlineData(999, "999")]
    [InlineData(0, "000")]
    public void ToString_WhenValueHasLeadingZeros_ReturnsThreeDigits(int value, string expected)
    {
        // Arrange
        var code = new NumericCode(value);

        // Act
        var result = code.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new NumericCode(978);
        var right = new NumericCode(978);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
