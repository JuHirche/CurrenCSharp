using System.Diagnostics.CodeAnalysis;

namespace CurrenCSharp;

/// <summary>
/// Represents an ISO 4217 alphabetic currency code, such as <c>EUR</c>.
/// </summary>
public sealed record AlphaCode
{
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaCode"/> class.
    /// </summary>
    /// <param name="value">The three-letter uppercase alphabetic code.</param>
    public AlphaCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = IsValid(value)
            ? value
            : throw new ArgumentException($"Alpha code must be exactly 3 uppercase ASCII letters, but was '{value}'.", nameof(value));
    }

    /// <summary>
    /// Parses the specified string into an <see cref="AlphaCode"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <returns>The parsed <see cref="AlphaCode"/>.</returns>
    public static AlphaCode Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        return TryParse(s, out var result)
            ? result
            : throw new FormatException($"'{s}' is not a valid alpha code.");
    }

    /// <summary>
    /// Tries to parse the specified string into an <see cref="AlphaCode"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="result">When this method returns, contains the parsed code if parsing succeeds; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if parsing succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, [NotNullWhen(true)] out AlphaCode? result) =>
        (result = s is not null && IsValid(s) ? new(s) : null) is not null;

    /// <summary>
    /// Returns the alphabetic code value.
    /// </summary>
    /// <returns>The underlying code string.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Converts a string value to an <see cref="AlphaCode"/>.
    /// </summary>
    /// <param name="value">The three-letter uppercase alphabetic code.</param>
    /// <returns>The converted <see cref="AlphaCode"/>.</returns>
    public static implicit operator AlphaCode(string value) => new(value);

    /// <summary>
    /// Converts an <see cref="AlphaCode"/> to its string representation.
    /// </summary>
    /// <param name="code">The code to convert.</param>
    /// <returns>The underlying code string.</returns>
    public static implicit operator string(AlphaCode code) => code.Value;

    private static bool IsValid(string value) =>
        value.Length == 3 && value.All(char.IsAsciiLetterUpper);
}
