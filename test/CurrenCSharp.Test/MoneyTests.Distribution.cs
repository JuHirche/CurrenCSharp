namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    [Fact]
    public void In_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new Money(1m, EUR);
        ExchangeRateContext context = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.In(context));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Distribute_WhenCountIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException(int count)
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = sut.Distribute(count));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void Distribute_WhenRatiosAreNull_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(1m, EUR);
        Ratio[] ratios = null!;

        // Act
        var exception = Record.Exception(() => _ = sut.Distribute(ratios));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Distribute_WhenRatiosAreEmpty_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = sut.Distribute([]));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Distribute_WhenRatioSumIsZero_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(1m, EUR);

        // Act
        var exception = Record.Exception(() => _ = sut.Distribute(0m, 0m, 0m));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }
}
