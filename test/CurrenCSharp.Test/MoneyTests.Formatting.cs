using System.Globalization;

namespace CurrenCSharp.Test;

public partial class MoneyTests
{
    public static TheoryData<decimal, string, byte, string> ToStringData => new()
    {
        { 12.34m, "EUR", (byte)2, @"^EUR\s?12\.34$" },
        { 1.2345m, "TST", (byte)3, @"^TST\s?1\.234$|^TST\s?1\.235$" },
        { 1234m, "JPY", (byte)0, @"^JPY\s?1,234$" },
        { -12.34m, "EUR", (byte)2, @"^-?\(?EUR\s?12\.34\)?$|^\(EUR\s?12\.34\)$" },
    };

    [Theory]
    [MemberData(nameof(ToStringData))]
    public void ToString_WhenCalled_UsesAlphaCodeAndMinorUnits(decimal amount, string alpha, byte minorUnits, string pattern)
    {
        // Arrange
        using var _ = new CultureScope("en-US");
        var currency = new Currency(alpha, alpha == "EUR" ? 978 : alpha == "JPY" ? 392 : 999, minorUnits);
        var sut = new Money(amount, currency);

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Contains(alpha, result, StringComparison.Ordinal);
        Assert.Matches(pattern, result);
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
