using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed class CurrencyTests : TestFixture
{
    [Fact]
    public void Default_WhenScopeExists_ReturnsScopedCurrency()
    {
        // Arrange — TestFixture already opens an EUR scope
        // Act
        var result = Currency.Default;

        // Assert
        Assert.Equal(EUR, result);
    }

    [Fact]
    public void Default_WhenNoScopeExists_ThrowsNoDefaultCurrencyException()
    {
        // Arrange — dispose the fixture-level scope first
        Dispose();

        try
        {
            // Act & Assert
            Assert.Throws<NoDefaultCurrencyException>(() => _ = Currency.Default);
        }
        finally
        {
            // Re-open a scope so the fixture's second Dispose is a no-op on our own scope
            _ = CurrenC.UseDefaultCurrency(EUR);
        }
    }

    [Fact]
    public void Equals_WhenAlphaCodeNumericCodeAndMinorUnitsMatch_ReturnsTrueAndSameHashCode()
    {
        // Arrange
        var left = new Currency(new AlphaCode("EUR"), new NumericCode(978), 2);
        var right = new Currency(new AlphaCode("EUR"), new NumericCode(978), 2);

        // Act & Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    public static TheoryData<Currency, Currency> DifferingCurrencies => new()
    {
        {
            new Currency(new AlphaCode("EUR"), new NumericCode(978), 2),
            new Currency(new AlphaCode("EUR"), new NumericCode(978), 3)
        },
        {
            new Currency(new AlphaCode("EUR"), new NumericCode(978), 2),
            new Currency(new AlphaCode("USD"), new NumericCode(840), 2)
        },
    };

    [Theory]
    [MemberData(nameof(DifferingCurrencies))]
    public void Equals_WhenAnyFieldDiffers_ReturnsFalse(Currency left, Currency right)
    {
        // Act & Assert
        Assert.NotEqual(left, right);
    }
}
