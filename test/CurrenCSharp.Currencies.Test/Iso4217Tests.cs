namespace CurrenCSharp.Currencies.Test;

public sealed class Iso4217Tests
{
    [Fact]
    public void FindByAlphaCode_WhenCodeExists_ReturnsCurrency()
    {
        // Arrange & Act
        var result = Iso4217.FindByAlphaCode("EUR");

        // Assert
        Assert.Equal(Iso4217.EUR, result);
    }

    [Fact]
    public void FindByAlphaCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = Iso4217.FindByAlphaCode("ZZZ"));

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void FindByAlphaCode_WhenCodeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        AlphaCode alphaCode = null!;

        // Act
        var exception = Record.Exception(() => _ = Iso4217.FindByAlphaCode(alphaCode));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void FindByNumericCode_WhenCodeExists_ReturnsCurrency()
    {
        // Arrange & Act
        var result = Iso4217.FindByNumericCode(840);

        // Assert
        Assert.Equal(Iso4217.USD, result);
    }

    [Fact]
    public void FindByNumericCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => _ = Iso4217.FindByNumericCode(1));

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void FindByNumericCode_WhenCodeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        NumericCode numericCode = null!;

        // Act
        var exception = Record.Exception(() => _ = Iso4217.FindByNumericCode(numericCode));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }
}
