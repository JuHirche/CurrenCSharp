namespace CurrenCSharp;

/// <summary>
/// Represents a non-negative ratio used for monetary distribution.
/// </summary>
public sealed record Ratio : IComparable<Ratio>
{
    private readonly decimal _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ratio"/> class.
    /// </summary>
    /// <param name="value">The non-negative ratio value.</param>
    public Ratio(decimal value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "The ratio must be greater or equal than zero.");

        _value = value;
    }

    /// <summary>
    /// Compares the current ratio with another ratio.
    /// </summary>
    /// <param name="other">The ratio to compare with this instance.</param>
    /// <returns>
    /// A value less than zero if this instance is smaller than <paramref name="other"/>,
    /// zero if they are equal, or greater than zero if this instance is larger.
    /// </returns>
    public int CompareTo(Ratio? other) => _value.CompareTo(other?._value);

    /// <summary>
    /// Converts a <see cref="Ratio"/> to its decimal value.
    /// </summary>
    /// <param name="ratio">The ratio to convert.</param>
    /// <returns>The decimal representation of the ratio.</returns>
    public static implicit operator decimal(Ratio ratio) => ratio._value;

    /// <summary>
    /// Converts a decimal value to a <see cref="Ratio"/>.
    /// </summary>
    /// <param name="value">The non-negative ratio value.</param>
    /// <returns>The converted <see cref="Ratio"/>.</returns>
    public static implicit operator Ratio(decimal value) => new(value);
}
