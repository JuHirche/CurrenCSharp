namespace CurrenCSharp;

/// <summary>
/// Defines options that control rounding behavior during currency conversion.
/// </summary>
/// <param name="RoundResult">Whether the conversion result is rounded.</param>
/// <param name="RoundingMode">The midpoint rounding strategy to apply when rounding is enabled.</param>
/// <param name="Scale">The optional number of fractional digits to use for rounding.</param>
public sealed record ConversionOptions(
    bool RoundResult = true,
    MidpointRounding RoundingMode = MidpointRounding.ToEven,
    Scale? Scale = null)
{
    /// <summary>
    /// Gets the default conversion options.
    /// </summary>
    public static ConversionOptions Default { get; } = new();
}
