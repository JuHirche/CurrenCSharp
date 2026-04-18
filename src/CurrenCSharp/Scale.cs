namespace CurrenCSharp;

/// <summary>
/// Represents a decimal scale used for rounding monetary amounts.
/// </summary>
public sealed record Scale
{
    private readonly byte _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> class.
    /// </summary>
    /// <param name="value">The number of fractional digits from 0 to 28.</param>
    public Scale(byte value)
    {
        if (value > 28)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Scale must be between 0 and 28.");

        _value = value;
    }

    /// <summary>
    /// Converts a <see cref="Scale"/> to its integer value.
    /// </summary>
    /// <param name="scale">The scale to convert.</param>
    /// <returns>The integer representation of the scale.</returns>
    public static implicit operator int(Scale scale) => scale._value;

    /// <summary>
    /// Converts a byte value to a <see cref="Scale"/>.
    /// </summary>
    /// <param name="value">The scale value to convert.</param>
    /// <returns>The converted <see cref="Scale"/>.</returns>
    public static implicit operator Scale(byte value) => new(value);

    /// <summary>
    /// Converts a <see cref="Scale"/> to its byte value.
    /// </summary>
    /// <param name="scale">The scale to convert.</param>
    /// <returns>The byte representation of the scale.</returns>
    public static explicit operator byte(Scale scale) => scale._value;
}
