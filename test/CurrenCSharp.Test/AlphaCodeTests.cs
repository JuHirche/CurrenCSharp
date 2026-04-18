namespace CurrenCSharp.Test;

public sealed class AlphaCodeTests
{
    [Fact]
    public void Constructor_WhenValueIsValid_SetsValue()
    {
        // Arrange & Act
        var result = new AlphaCode("EUR");

        // Assert
        Assert.Equal("EUR", result.Value);
    }

    [Fact]
    public void Constructor_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string value = null!;

        // Act
        var exception = Record.Exception(() => _ = new AlphaCode(value));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("EU")]
    [InlineData("EURO")]
    [InlineData("eur")]
    [InlineData("E1R")]
    public void Constructor_WhenValueIsInvalid_ThrowsArgumentException(string value)
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = new AlphaCode(value));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Parse_WhenValueIsValid_ReturnsAlphaCode()
    {
        // Arrange & Act
        var result = AlphaCode.Parse("USD");

        // Assert
        Assert.Equal("USD", result.Value);
    }

    [Fact]
    public void Parse_WhenValueIsInvalid_ThrowsFormatException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = AlphaCode.Parse("usd"));

        // Assert
        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string value = null!;

        // Act
        var exception = Record.Exception(() => _ = AlphaCode.Parse(value));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void TryParse_WhenValueIsValid_ReturnsTrueAndResult()
    {
        // Arrange & Act
        var success = AlphaCode.TryParse("JPY", out var result);

        // Assert
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal("JPY", result.Value);
    }

    [Fact]
    public void TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult()
    {
        // Arrange & Act
        var success = AlphaCode.TryParse("jpy", out var result);

        // Assert
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsUnderlyingValue()
    {
        // Arrange
        var sut = new AlphaCode("EUR");

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Equal("EUR", result);
    }
}
