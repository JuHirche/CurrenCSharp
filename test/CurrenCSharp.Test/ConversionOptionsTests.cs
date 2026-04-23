namespace CurrenCSharp.Test;

public sealed class ConversionOptionsTests
{
    [Fact]
    public void Default_WhenAccessed_ReturnsExpectedDefaultValues()
    {
        // Act
        var sut = ConversionOptions.Default;

        // Assert
        Assert.True(sut.RoundResult);
        Assert.Equal(MidpointRounding.ToEven, sut.RoundingMode);
        Assert.Null(sut.Scale);
    }

    [Fact]
    public void Equals_WhenAllFieldsMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new ConversionOptions(RoundResult: true, RoundingMode: MidpointRounding.ToEven, Scale: new Scale(2));
        var right = new ConversionOptions(RoundResult: true, RoundingMode: MidpointRounding.ToEven, Scale: new Scale(2));

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    public static TheoryData<ConversionOptions, ConversionOptions> DifferingOptions => new()
    {
        {
            new ConversionOptions(RoundResult: true),
            new ConversionOptions(RoundResult: false)
        },
        {
            new ConversionOptions(RoundingMode: MidpointRounding.ToEven),
            new ConversionOptions(RoundingMode: MidpointRounding.AwayFromZero)
        },
        {
            new ConversionOptions(Scale: new Scale(2)),
            new ConversionOptions(Scale: new Scale(3))
        },
    };

    [Theory]
    [MemberData(nameof(DifferingOptions))]
    public void Equals_WhenAnyFieldDiffers_ReturnsFalse(ConversionOptions left, ConversionOptions right)
    {
        // Act & Assert
        Assert.NotEqual(left, right);
    }

    [Fact]
    public void WithExpression_WhenFieldChanged_ReturnsModifiedCopy()
    {
        // Arrange
        var sut = ConversionOptions.Default;

        // Act
        var modified = sut with { RoundResult = false };

        // Assert
        Assert.True(sut.RoundResult);
        Assert.False(modified.RoundResult);
    }
}
