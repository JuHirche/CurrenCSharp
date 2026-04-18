namespace CurrenCSharp.Test;

public sealed class ScaleTests
{
    [Fact]
    public void Constructor_WhenValueGreaterThan28_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const byte value = 29;

        // Act
        var exception = Record.Exception(() => _ = new Scale(value));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void ImplicitIntConversion_WhenScaleIsValid_ReturnsExpectedValue()
    {
        // Arrange
        var sut = new Scale(4);

        // Act
        int result = sut;

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void ExplicitByteConversion_WhenScaleIsValid_ReturnsExpectedValue()
    {
        // Arrange
        var sut = new Scale(6);

        // Act
        var result = (byte)sut;

        // Assert
        Assert.Equal(6, result);
    }
}
