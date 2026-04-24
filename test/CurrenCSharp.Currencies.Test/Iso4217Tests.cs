using System.Reflection;

namespace CurrenCSharp.Currencies.Test;

public sealed class Iso4217Tests
{
    [Theory]
    [InlineData("EUR")]
    [InlineData("USD")]
    [InlineData("JPY")]
    [InlineData("CHF")]
    public void FindByAlphaCode_WhenCodeExists_ReturnsCurrency(string code)
    {
        // Act
        var result = Iso4217.FindByAlphaCode(code);

        // Assert
        Assert.Equal(code, result.AlphaCode.Value);
    }

    [Theory]
    [InlineData("ZZZ")]
    [InlineData("XXX")]
    public void FindByAlphaCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException(string code)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => Iso4217.FindByAlphaCode(code));
    }

    [Fact]
    public void FindByAlphaCode_WhenCodeIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Iso4217.FindByAlphaCode(null!));
    }

    [Theory]
    [InlineData(978)]
    [InlineData(840)]
    [InlineData(392)]
    public void FindByNumericCode_WhenCodeExists_ReturnsCurrency(int code)
    {
        // Act
        var result = Iso4217.FindByNumericCode(code);

        // Assert
        Assert.Equal(code, result.NumericCode.Value);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void FindByNumericCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException(int code)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => Iso4217.FindByNumericCode(code));
    }

    [Fact]
    public void FindByNumericCode_WhenCodeIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Iso4217.FindByNumericCode(null!));
    }

    [Fact]
    public void Catalog_WhenLoaded_HasUniqueAlphaAndNumericCodes()
    {
        // Arrange
        var currencies = GetCatalogCurrencies();

        // Act
        var alphaCodes = currencies.Select(c => c.AlphaCode.Value).ToList();
        var numericCodes = currencies.Select(c => c.NumericCode.Value).ToList();

        // Assert
        Assert.Equal(alphaCodes.Count, alphaCodes.Distinct().Count());
        Assert.Equal(numericCodes.Count, numericCodes.Distinct().Count());
    }

    public static TheoryData<string, byte> MinorUnitsSamples => new()
    {
        { "JPY", 0 },
        { "EUR", 2 },
        { "KWD", 3 },
    };

    [Theory]
    [MemberData(nameof(MinorUnitsSamples))]
    public void Catalog_WhenLoaded_ContainsExpectedMinorUnitsSamples(string alpha, byte expectedMinorUnits)
    {
        // Act
        var currency = Iso4217.FindByAlphaCode(alpha);

        // Assert
        Assert.Equal(expectedMinorUnits, currency.MinorUnits);
    }

    [Fact]
    public void Catalog_WhenInspected_AllCurrenciesAreNonNull()
    {
        // Arrange
        var currencies = GetCatalogCurrencies();

        // Act & Assert
        Assert.All(currencies, Assert.NotNull);
    }

    [Fact]
    public void FindByAlphaCode_WhenRoundTrippedThroughNumericLookup_ReturnsSameCurrencyInstance()
    {
        // Arrange
        var byAlpha = Iso4217.FindByAlphaCode("EUR");

        // Act
        var byNumeric = Iso4217.FindByNumericCode(978);

        // Assert
        Assert.Same(byAlpha, byNumeric);
    }

    [Fact]
    public void Lookup_WhenCalledConcurrently_ReturnsConsistentCurrencies()
    {
        // Arrange
        const int iterations = 10_000;

        // Act
        var results = Enumerable.Range(0, iterations)
            .AsParallel()
            .Select(i =>
            {
                var alpha = Iso4217.FindByAlphaCode("EUR");
                var numeric = Iso4217.FindByNumericCode(978);
                return ReferenceEquals(alpha, numeric) && alpha.AlphaCode.Value == "EUR";
            })
            .ToList();

        // Assert
        Assert.All(results, Assert.True);
    }

    private static IReadOnlyList<Currency> GetCatalogCurrencies() =>
        [.. typeof(Iso4217)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(Currency))
            .Select(f => (Currency)f.GetValue(null)!)];
}
