using System.Globalization;

namespace CurrenCSharp.Currencies.Test;

public sealed partial class Iso4217Tests
{
    public static TheoryData<string, int, byte> HistoricalCurrencies => new()
    {
        { "ADP", 020, 0 },
        { "AFA", 004, 2 },
        { "ATS", 040, 2 },
        { "AZM", 031, 2 },
        { "BEF", 056, 0 },
        { "BGN", 975, 2 },
        { "BYB", 112, 0 },
        { "BYR", 974, 0 },
        { "CUC", 931, 2 },
        { "CYP", 196, 2 },
        { "DEM", 276, 2 },
        { "EEK", 233, 2 },
        { "ESP", 724, 0 },
        { "FIM", 246, 2 },
        { "FRF", 250, 2 },
        { "GHC", 288, 2 },
        { "GRD", 300, 0 },
        { "GWP", 624, 2 },
        { "IEP", 372, 2 },
        { "ITL", 380, 0 },
        { "LTL", 440, 2 },
        { "LUF", 442, 0 },
        { "LVL", 428, 2 },
        { "MGF", 450, 0 },
        { "MRO", 478, 2 },
        { "MTL", 470, 2 },
        { "MZM", 508, 2 },
        { "NLG", 528, 2 },
        { "PTE", 620, 0 },
        { "ROL", 642, 0 },
        { "SIT", 705, 2 },
        { "SKK", 703, 2 },
        { "SRG", 740, 2 },
        { "STD", 678, 2 },
        { "TMM", 795, 2 },
        { "TPE", 626, 0 },
        { "TRL", 792, 0 },
        { "VEB", 862, 2 },
        { "VEF", 937, 2 },
        { "ZMK", 894, 2 },
        { "ZWN", 942, 2 },
        { "ZWR", 935, 2 },
    };

    [Theory]
    [MemberData(nameof(HistoricalCurrencies))]
    public void HistoricalCatalog_WhenLookedUp_ContainsExpectedCurrency(
        string alphaCode,
        int numericCode,
        byte minorUnits)
    {
        // Act
        var byAlpha = Iso4217.FindByAlphaCode(alphaCode);
        var byNumeric = Iso4217.FindByNumericCode(numericCode);

        // Assert
        Assert.Equal(numericCode, byAlpha.NumericCode.Value);
        Assert.Equal(minorUnits, byAlpha.MinorUnits);
        Assert.Same(byAlpha, byNumeric);
    }

    [Fact]
    public void HistoricalCatalog_WhenInspected_ContainsExactlyApprovedAlphaCodes()
    {
        // Arrange
        var expected = "ADP AFA ATS AZM BEF BGN BYB BYR CUC CYP DEM EEK ESP FIM FRF GHC GRD GWP "
            + "IEP ITL LTL LUF LVL MGF MRO MTL MZM NLG PTE ROL SIT SKK SRG STD TMM TPE TRL "
            + "VEB VEF ZMK ZWN ZWR";

        // Act
        var result = Iso4217.HistoricalCurrencies
            .Select(currency => currency.AlphaCode.Value)
            .Order()
            .ToArray();

        // Assert
        Assert.Equal(expected.Split(' '), result);
    }

    [Theory]
    [InlineData("PES")]
    [InlineData("XFO")]
    [InlineData("DDM")]
    public void FindByAlphaCode_WhenHistoricalCodeIsExcluded_ThrowsInvalidOperationException(string alphaCode)
    {
        // Act
        var action = () => Iso4217.FindByAlphaCode(alphaCode);

        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void FindByNumericCode_WhenCurrentCodeConflictedHistorically_ReturnsCurrentCurrency()
    {
        // Act
        var result = Iso4217.FindByNumericCode(604);

        // Assert
        Assert.Same(Iso4217.PEN, result);
    }

    [Fact]
    public void Money_WhenConstructedWithHistoricalCurrency_PreservesCurrency()
    {
        // Arrange
        var deutscheMark = Iso4217.FindByAlphaCode("DEM");

        // Act
        var result = new Money(100m, deutscheMark);

        // Assert
        Assert.Same(deutscheMark, result.Currency);
    }

    [Fact]
    public void Money_WhenFormattedWithHistoricalCurrency_UsesAlphaCodeAndMinorUnits()
    {
        // Arrange
        using var _ = new CultureScope(CultureInfo.InvariantCulture);
        var sut = new Money(100m, Iso4217.FindByAlphaCode("DEM"));

        // Act
        var result = sut.ToString();

        // Assert
        Assert.Equal("DEM 100.00", result);
    }

    private sealed class CultureScope : IDisposable
    {
        private readonly CultureInfo _previousCulture = CultureInfo.CurrentCulture;

        public CultureScope(CultureInfo culture) => CultureInfo.CurrentCulture = culture;

        public void Dispose() => CultureInfo.CurrentCulture = _previousCulture;
    }
}
