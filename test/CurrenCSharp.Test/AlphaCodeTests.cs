namespace CurrenCSharp.Test;

public sealed class AlphaCodeTests
{
    [Fact]
    public void Constructor_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AlphaCode(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("EU")]
    [InlineData("EURO")]
    [InlineData("eur")]
    [InlineData("E1R")]
    [InlineData("\u0415UR")] // Cyrillic capital IE
    public void Constructor_WhenValueIsInvalid_ThrowsArgumentException(string value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AlphaCode(value));
    }

    [Theory]
    [InlineData("\u00C7ur")]        // Ç
    [InlineData("\u00CBur")]        // Ë
    [InlineData("\uD835\uDC00BC")]  // Mathematical bold A
    public void Constructor_WhenValueContainsNonAsciiLetters_ThrowsArgumentException(string value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AlphaCode(value));
    }

    [Fact]
    public void Constructor_WhenValueIsExcessivelyLong_ThrowsArgumentException()
    {
        // Arrange
        var value = new string('A', 10_000);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AlphaCode(value));
    }

    [Theory]
    [InlineData("EUR")]
    [InlineData("USD")]
    [InlineData("CHF")]
    [InlineData("JPY")]
    public void Parse_WhenValueIsValid_ReturnsAlphaCode(string value)
    {
        // Act
        var result = AlphaCode.Parse(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [InlineData("eur")]
    [InlineData("EU1")]
    [InlineData("EURO")]
    public void Parse_WhenValueIsInvalid_ThrowsFormatException(string value)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => AlphaCode.Parse(value));
    }

    [Fact]
    public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => AlphaCode.Parse(null!));
    }

    [Theory]
    [InlineData("E\0R")]
    [InlineData("EU\nR")]
    [InlineData("E\tR")]
    public void Parse_WhenInputContainsControlCharacters_ThrowsFormatException(string value)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => AlphaCode.Parse(value));
    }

    [Theory]
    [InlineData("JPY")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void TryParse_WhenValueIsValid_ReturnsTrueAndResult(string value)
    {
        // Act
        var success = AlphaCode.TryParse(value, out var result);

        // Assert
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(value, result!.Value);
    }

    [Theory]
    [InlineData("jpy")]
    [InlineData("JP")]
    [InlineData("JP\u00A5")]
    [InlineData("")]
    [InlineData(null)]
    public void TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult(string? value)
    {
        // Act
        var success = AlphaCode.TryParse(value, out var result);

        // Assert
        Assert.False(success);
        Assert.Null(result);
    }

    [Theory]
    [InlineData("CHF")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void Conversions_WhenRoundTripped_PreserveValue(string value)
    {
        // Act
        AlphaCode code = value;
        string roundTripped = code;

        // Assert
        Assert.Equal(value, roundTripped);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new AlphaCode("EUR");
        var right = new AlphaCode("EUR");

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WhenComparedWithDifferentType_ReturnsFalse()
    {
        // Arrange
        var code = new AlphaCode("EUR");

        // Act & Assert
        Assert.False(code.Equals((object)"EUR"));
    }
}
