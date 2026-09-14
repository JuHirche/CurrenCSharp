using System.Collections.ObjectModel;

namespace CurrenCSharp.Currencies;

public static partial class Iso4217
{
    // See docs/historical-currencies-data.md for the data sources and decisions.
    internal static ReadOnlyCollection<Currency> HistoricalCurrencies { get; } = Array.AsReadOnly<Currency>(
    [
        new("ADP", 020, 0),
        new("AFA", 004, 2),
        new("ATS", 040, 2),
        new("AZM", 031, 2),
        new("BEF", 056, 0),
        new("BGN", 975, 2),
        new("BYB", 112, 0),
        new("BYR", 974, 0),
        new("CUC", 931, 2),
        new("CYP", 196, 2),
        new("DEM", 276, 2),
        new("EEK", 233, 2),
        new("ESP", 724, 0),
        new("FIM", 246, 2),
        new("FRF", 250, 2),
        new("GHC", 288, 2),
        new("GRD", 300, 0),
        new("GWP", 624, 2),
        new("IEP", 372, 2),
        new("ITL", 380, 0),
        new("LTL", 440, 2),
        new("LUF", 442, 0),
        new("LVL", 428, 2),
        new("MGF", 450, 0),
        new("MRO", 478, 2),
        new("MTL", 470, 2),
        new("MZM", 508, 2),
        new("NLG", 528, 2),
        new("PTE", 620, 0),
        new("ROL", 642, 0),
        new("SIT", 705, 2),
        new("SKK", 703, 2),
        new("SRG", 740, 2),
        new("STD", 678, 2),
        new("TMM", 795, 2),
        new("TPE", 626, 0),
        new("TRL", 792, 0),
        new("VEB", 862, 2),
        new("VEF", 937, 2),
        new("ZMK", 894, 2),
        new("ZWN", 942, 2),
        new("ZWR", 935, 2),
    ]);
}
