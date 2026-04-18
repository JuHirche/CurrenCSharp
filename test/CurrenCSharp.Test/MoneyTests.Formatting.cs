using System.Globalization;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    [Fact]
    public void ToString_WhenCalled_UsesCurrencyAlphaCodeAsSymbol()
    {
        // Arrange
        using var _ = new CultureScope("en-US");
        var sut = new Money(12.34m, EUR);

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Contains("EUR", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ToString_WhenCurrencyHasSpecificMinorUnits_UsesExpectedDecimalDigits()
    {
        // Arrange
        using var _ = new CultureScope("en-US");
        var customCurrency = new Currency("TST", 999, 3);
        var sut = new Money(1.2345m, customCurrency);

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Matches(@"\d+\.\d{3}", result);
    }

    private sealed class CultureScope : IDisposable
    {
        private readonly CultureInfo _previousCulture;
        private readonly CultureInfo _previousUiCulture;

        public CultureScope(string cultureName)
        {
            _previousCulture = CultureInfo.CurrentCulture;
            _previousUiCulture = CultureInfo.CurrentUICulture;

            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _previousCulture;
            CultureInfo.CurrentUICulture = _previousUiCulture;
        }
    }
}
