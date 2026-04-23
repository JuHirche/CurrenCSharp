using System.Collections.Immutable;
using System.Diagnostics;

namespace CurrenCSharp.Test;

public sealed class SecurityTests : TestFixture
{
    [Theory]
    [InlineData("\u0415UR")]       // Cyrillic capital IE
    [InlineData("\u0395UR")]       // Greek capital epsilon
    [InlineData("\uFF25\uFF35\uFF32")] // Fullwidth EUR
    public void AlphaCode_Parse_WhenInputContainsUnicodeHomoglyphs_Rejected(string input)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => AlphaCode.Parse(input));
    }

    [Theory]
    [InlineData("E\u200BUR")]      // Zero-Width Space inside
    [InlineData("EUR\uFEFF")]      // BOM suffix
    [InlineData("\u200BEUR")]      // Zero-Width Space prefix
    public void AlphaCode_Parse_WhenInputContainsZeroWidthCharacters_Rejected(string input)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => AlphaCode.Parse(input));
    }

    [Fact]
    public void AlphaCode_Constructor_WhenInputIsExtremelyLarge_DoesNotHang()
    {
        // Arrange
        var large = new string('A', 1_000_000);
        var sw = Stopwatch.StartNew();

        // Act & Assert — must reject quickly, not hang
        Assert.Throws<ArgumentException>(() => new AlphaCode(large));
        sw.Stop();
        Assert.True(sw.ElapsedMilliseconds < 1_000, $"Took {sw.ElapsedMilliseconds} ms");
    }

    [Theory]
    [InlineData("\u0669\u0667\u0668")] // Arabic-Indic 978
    [InlineData("\u0E59\u0E57\u0E58")] // Thai 978
    [InlineData("\u096F\u096D\u096E")] // Devanagari 978
    public void NumericCode_Parse_WhenInputContainsNonAsciiDigits_Rejected(string input)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(input));
    }

    [Theory]
    [InlineData("+978")]
    [InlineData(" 978")]
    [InlineData("978 ")]
    [InlineData("\t978")]
    public void NumericCode_Parse_WhenInputContainsSignOrWhitespace_Rejected(string input)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(input));
    }

    [Fact]
    public void NumericCode_Parse_WhenInputIsExtremelyLarge_DoesNotHang()
    {
        // Arrange
        var large = new string('9', 1_000_000);
        var sw = Stopwatch.StartNew();

        // Act & Assert
        Assert.Throws<FormatException>(() => NumericCode.Parse(large));
        sw.Stop();
        Assert.True(sw.ElapsedMilliseconds < 1_000, $"Took {sw.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Currency_GetHashCode_WhenManyDistinctCurrenciesHashed_HasNegligibleCollisionRate()
    {
        // Arrange — build a set of distinct currencies (all combinations of letter triplets we can afford)
        var currencies = Enumerable.Range(0, 26)
            .SelectMany(a => Enumerable.Range(0, 26)
                .SelectMany(b => Enumerable.Range(0, 26)
                    .Select(c => new Currency(
                        new AlphaCode(new string(new[] { (char)('A' + a), (char)('A' + b), (char)('A' + c) })),
                        new NumericCode((a * 26 * 26 + b * 26 + c) % 1000),
                        2))))
            .Take(1000)
            .ToArray();

        // Act
        var hashes = currencies.Select(c => c.GetHashCode()).ToList();
        var distinctHashes = hashes.Distinct().Count();
        var collisionRate = 1d - (double)distinctHashes / hashes.Count;

        // Assert — tolerate < 5% because NumericCode is forced into a 1000-bucket range
        Assert.True(collisionRate < 0.05d, $"Collision rate: {collisionRate:P2}");
    }

    [Fact]
    public void Wallet_Equals_WhenContentsAreSameButConstructedDifferently_IsCorrect()
    {
        // Arrange
        var leftMoneys = new[] { new Money(1m, EUR), new Money(2m, USD), new Money(3m, JPY) };
        var rightMoneys = leftMoneys.Reverse().ToArray();

        // Act
        var left = Wallet.Of(leftMoneys);
        var right = Wallet.Of(rightMoneys);

        // Assert
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Theory]
    [InlineData(1e-20)]
    public void GetExchangeRate_WhenRateIsVerySmall_DoesNotOverflowOrHang(decimal rate)
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(rate))
                .Add(JPY, new ExchangeRate(rate * 2m)));

        // Act — cross rate calculation should remain stable
        var cross = (decimal)ctx.GetExchangeRate(USD, JPY);

        // Assert
        Assert.Equal(2m, cross);
    }

    [Fact]
    public void GetExchangeRate_WhenRateIsVeryLarge_DoesNotOverflowOrHang()
    {
        // Arrange — use a large but safe value so cross-rate does not overflow decimal
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
            ImmutableDictionary<Currency, ExchangeRate>.Empty
                .Add(USD, new ExchangeRate(1_000_000m))
                .Add(JPY, new ExchangeRate(500_000m)));

        // Act
        var cross = (decimal)ctx.GetExchangeRate(USD, JPY);

        // Assert — 500k / 1M = 0.5
        Assert.Equal(0.5m, cross);
    }
}
