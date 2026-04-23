namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Distribute_WhenCountIsLessThanOrEqualZero_ThrowsArgumentOutOfRangeException(int count)
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Distribute(count));
    }

    [Fact]
    public void Distribute_WhenCountIsValid_PreservesSumAndCount()
    {
        // Arrange
        var sut = new Money(47.11m, EUR);

        // Act
        var result = sut.Distribute(3).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(47.11m, result.Sum(m => m.Amount));
        Assert.All(result, m => Assert.Equal(EUR, m.Currency));
        Assert.Equal(new[] { 15.71m, 15.70m, 15.70m }, result.Select(m => m.Amount));
    }

    [Fact]
    public void Distribute_WhenRatiosAreNull_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sut.Distribute((Ratio[])null!));
    }

    [Fact]
    public void Distribute_WhenRatiosAreEmpty_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sut.Distribute(Array.Empty<Ratio>()));
    }

    [Fact]
    public void Distribute_WhenRatioSumIsZero_ThrowsArgumentException()
    {
        // Arrange
        var sut = new Money(10m, EUR);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            sut.Distribute(new Ratio(0m), new Ratio(0m), new Ratio(0m)));
    }

    [Fact]
    public void Distribute_WhenRatiosHaveTie_DistributesRemainderByIndex()
    {
        // Arrange
        var sut = new Money(0.02m, EUR);

        // Act
        var result = sut.Distribute(new Ratio(1m), new Ratio(1m), new Ratio(1m)).ToList();

        // Assert
        Assert.Equal(new[] { 0.01m, 0.01m, 0.00m }, result.Select(m => m.Amount));
    }

    [Fact]
    public void Distribute_WhenAmountIsNegative_DistributesNegativeRemainderByIndex()
    {
        // Arrange
        var sut = new Money(-0.02m, EUR);

        // Act
        var result = sut.Distribute(new Ratio(1m), new Ratio(1m), new Ratio(1m)).ToList();

        // Assert
        Assert.Equal(new[] { -0.01m, -0.01m, 0.00m }, result.Select(m => m.Amount));
    }
}
