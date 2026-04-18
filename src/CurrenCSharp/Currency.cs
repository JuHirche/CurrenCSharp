namespace CurrenCSharp;

/// <summary>
/// Represents a currency using ISO 4217 alpha and numeric codes plus minor units.
/// </summary>
/// <param name="AlphaCode">The three-letter ISO 4217 currency code.</param>
/// <param name="NumericCode">The numeric ISO 4217 currency code.</param>
/// <param name="MinorUnits">The number of decimal places used by the currency.</param>
public sealed record Currency(AlphaCode AlphaCode, NumericCode NumericCode, byte MinorUnits)
{
    /// <summary>
    /// Gets the ambient default currency for the current scope.
    /// </summary>
    public static Currency Default => CurrenC.DefaultCurrency;
}
