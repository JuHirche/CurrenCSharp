namespace CurrenCSharp;

public static class ExtensionMethods
{
    extension (decimal d)
    {
        internal decimal Round(byte minorUnits, ConversionOptions? options)
        {
            options ??= ConversionOptions.Default;
            return options.RoundResult == true
                ? Math.Round(d, options.Scale ?? minorUnits, options.RoundingMode)
                : d;
        }
    }
}
