using System.Diagnostics.CodeAnalysis;

namespace CurrenCSharp;

/// <summary>
/// Represents an ISO 4217 numeric currency code, such as <c>978</c>.
/// </summary>
public sealed record NumericCode
{
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericCode"/> class.
    /// </summary>
    /// <param name="value">The numeric code in the range 0 to 999.</param>
    public NumericCode(int value)
    {
        Value = IsValid(value)
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), value, "Numeric code must be between 0 and 999.");
    }

    /// <summary>
    /// Parses the specified string into a <see cref="NumericCode"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <returns>The parsed <see cref="NumericCode"/>.</returns>
    public static NumericCode Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return TryParse(s, out var result)
            ? result
            : throw new FormatException($"'{s}' is not a valid numeric code.");
    }

    /// <summary>
    /// Tries to parse the specified string into a <see cref="NumericCode"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="result">When this method returns, contains the parsed code if parsing succeeds; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if parsing succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, [NotNullWhen(true)] out NumericCode? result) =>
        (result = s is not null && int.TryParse(s, out var value) && IsValid(value) ? new(value) : null) is not null;

    private static bool IsValid(int value) => value is >= 0 and <= 999;

    /// <summary>
    /// Returns the numeric code as a three-digit string.
    /// </summary>
    /// <returns>The underlying code string padded to three digits.</returns>
    public override string ToString() => Value.ToString("D3");

    /// <summary>
    /// Converts an integer value to a <see cref="NumericCode"/>.
    /// </summary>
    /// <param name="value">The numeric code in the range 0 to 999.</param>
    /// <returns>The converted <see cref="NumericCode"/>.</returns>
    public static implicit operator NumericCode(int value) => new(value);

    /// <summary>
    /// Converts a <see cref="NumericCode"/> to its integer value.
    /// </summary>
    /// <param name="code">The code to convert.</param>
    /// <returns>The numeric code value.</returns>
    public static implicit operator int(NumericCode code) => code.Value;
}
