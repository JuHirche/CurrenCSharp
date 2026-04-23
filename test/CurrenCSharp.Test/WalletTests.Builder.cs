using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed partial class WalletTests
{
    [Fact]
    public void Add_WhenCurrencyExists_AggregatesAmount()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act
        builder.Add(new Money(2m, EUR));

        // Assert
        Assert.Equal(3m, builder[EUR].Amount);
    }

    [Fact]
    public void Add_WhenCurrencyIsNew_CreatesBucket()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act
        builder.Add(new Money(2m, USD));

        // Assert
        Assert.Equal(1m, builder[EUR].Amount);
        Assert.Equal(2m, builder[USD].Amount);
    }

    [Fact]
    public void AddRange_WhenMoneysProvided_AggregatesPerCurrency()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act
        builder.AddRange(
        [
            new Money(1m, EUR),
            new Money(2m, USD),
            new Money(3m, EUR),
        ]);

        // Assert
        Assert.Equal(4m, builder[EUR].Amount);
        Assert.Equal(2m, builder[USD].Amount);
    }

    [Fact]
    public void AddRange_WhenMoneysIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddRange(null!));
    }

    [Fact]
    public void IndexerSet_WhenMoneyCurrencyDiffers_ThrowsDifferentCurrencyException()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act & Assert
        Assert.Throws<DifferentCurrencyException>(() => builder[EUR] = new Money(1m, USD));
    }

    [Fact]
    public void IndexerSet_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder[null!] = new Money(1m, EUR));
    }

    [Fact]
    public void IndexerGet_WhenKeyMissing_ThrowsKeyNotFoundException()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _ = builder[JPY]);
    }

    public static TheoryData<Currency, bool> RemoveData =>
        new()
        {
            { EUR, true },
            { JPY, false }
        };

    [Theory]
    [MemberData(nameof(RemoveData))]
    public void Remove_WhenKeyExistsOrMissing_ReturnsExpectedResult(Currency key, bool expected)
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act
        var result = builder.Remove(key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Clear_WhenCalled_RemovesAllEntries()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR), new Money(2m, USD)).ToBuilder();

        // Act
        builder.Clear();

        // Assert
        Assert.Equal(0, builder.Count);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrueAndValue()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act
        var success = builder.TryGetValue(EUR, out var value);

        // Assert
        Assert.True(success);
        Assert.Equal(1m, value.Amount);
    }

    [Fact]
    public void TryGetValue_WhenKeyMissing_ReturnsFalse()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act
        var success = builder.TryGetValue(JPY, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void ToWallet_WhenBuilderContainsEntries_ReturnsEquivalentWallet()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();
        builder.Add(new Money(1m, EUR));
        builder.Add(new Money(2m, USD));

        // Act
        var wallet = builder.ToWallet();

        // Assert
        Assert.Equal(Wallet.Of(new Money(1m, EUR), new Money(2m, USD)), wallet);
    }

    [Fact]
    public void ToWallet_WhenBuilderChangesAfterCreation_DoesNotMutateReturnedWallet()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();
        var wallet = builder.ToWallet();

        // Act
        builder.Add(new Money(2m, USD));

        // Assert
        Assert.Single(wallet);
    }

    [Fact]
    public void ToWallet_WhenBuilderIsEmpty_ReturnsEmptyWallet()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act
        var wallet = builder.ToWallet();

        // Assert
        Assert.Empty(wallet);
    }

    [Fact]
    public void GetEnumerator_WhenBuilderHasEntries_YieldsAllKeyValuePairs()
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR), new Money(2m, USD)).ToBuilder();

        // Act
        var pairs = builder.ToList();

        // Assert
        Assert.Equal(2, pairs.Count);
        Assert.All(pairs, p => Assert.IsType<KeyValuePair<Currency, Money>>(p));
    }

    [Fact]
    public void Count_WhenItemsAddedAndRemoved_ReflectsCurrentSize()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();

        // Act & Assert
        Assert.Equal(0, builder.Count);
        builder.Add(new Money(1m, EUR));
        builder.Add(new Money(2m, USD));
        builder.Add(new Money(3m, JPY));
        Assert.Equal(3, builder.Count);
        builder.Remove(JPY);
        Assert.Equal(2, builder.Count);
    }

    [Fact]
    public void KeysAndValues_WhenEnumerated_ReturnCurrentBuilderState()
    {
        // Arrange
        var builder = Wallet.Empty.ToBuilder();
        builder.Add(new Money(1m, EUR));
        builder.Add(new Money(2m, USD));

        // Act
        var keys = builder.Keys.ToList();
        var values = builder.Values.ToList();

        // Assert
        Assert.Contains(EUR, keys);
        Assert.Contains(USD, keys);
        Assert.Contains(values, m => m.Currency.Equals(EUR) && m.Amount == 1m);
        Assert.Contains(values, m => m.Currency.Equals(USD) && m.Amount == 2m);
    }

    public static TheoryData<Currency, bool> ContainsKeyData =>
        new()
        {
            { EUR, true },
            { JPY, false }
        };

    [Theory]
    [MemberData(nameof(ContainsKeyData))]
    public void ContainsKey_WhenKeyExistsOrMissing_ReturnsExpectedResult(Currency key, bool expected)
    {
        // Arrange
        var builder = Wallet.Of(new Money(1m, EUR)).ToBuilder();

        // Act
        var result = builder.ContainsKey(key);

        // Assert
        Assert.Equal(expected, result);
    }
}
