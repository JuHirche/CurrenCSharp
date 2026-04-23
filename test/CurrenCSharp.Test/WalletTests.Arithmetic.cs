namespace CurrenCSharp.Test;

public sealed partial class WalletTests
{
    [Fact]
    public void UnaryPlus_WhenCalled_ReturnsSameWallet()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR));

        // Act
        var result = +sut;

        // Assert
        Assert.Same(sut, result);
    }

    [Fact]
    public void UnaryNegation_WhenCalled_NegatesAllBuckets()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR), new Money(-5m, USD));

        // Act
        var result = -sut;

        // Assert
        var items = result.ToDictionary(m => m.Currency, m => m.Amount);
        Assert.Equal(-10m, items[EUR]);
        Assert.Equal(5m, items[USD]);
    }

    public static TheoryData<Action> NullUnaryOperatorActions => new()
    {
        { () => { _ = +((Wallet)null!); } },
        { () => { _ = -((Wallet)null!); } },
    };

    [Theory]
    [MemberData(nameof(NullUnaryOperatorActions))]
    public void UnaryOperators_WhenWalletIsNull_ThrowArgumentNullException(Action act)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Addition_WhenWalletsContainSameCurrencies_AggregatesPerCurrency()
    {
        // Arrange
        var left = Wallet.Of(new Money(10m, EUR), new Money(2m, USD));
        var right = Wallet.Of(new Money(5m, EUR), new Money(3m, USD));

        // Act
        var result = left + right;

        // Assert
        var items = result.ToDictionary(m => m.Currency, m => m.Amount);
        Assert.Equal(15m, items[EUR]);
        Assert.Equal(5m, items[USD]);
    }

    [Fact]
    public void Addition_WhenMoneyCurrencyExists_UpdatesBucketAmount()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR), new Money(2m, USD));

        // Act
        var result = sut + new Money(3m, EUR);

        // Assert
        var items = result.ToDictionary(m => m.Currency, m => m.Amount);
        Assert.Equal(4m, items[EUR]);
        Assert.Equal(2m, items[USD]);
    }

    [Fact]
    public void Subtraction_WhenWalletsContainSameCurrencies_SubtractsPerCurrency()
    {
        // Arrange
        var left = Wallet.Of(new Money(10m, EUR), new Money(5m, USD));
        var right = Wallet.Of(new Money(3m, EUR), new Money(2m, USD));

        // Act
        var result = left - right;

        // Assert
        var items = result.ToDictionary(m => m.Currency, m => m.Amount);
        Assert.Equal(7m, items[EUR]);
        Assert.Equal(3m, items[USD]);
    }

    [Fact]
    public void Subtraction_WhenResultBecomesZero_RemovesBucket()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act
        var result = sut - new Money(1m, EUR);

        // Assert
        Assert.Empty(result);
    }

    public static TheoryData<Action> NullBinaryOperatorActions
    {
        get
        {
            var wallet = Wallet.Of(new Money(1m, new Currency("EUR", 978, 2)));
            var money = new Money(1m, new Currency("EUR", 978, 2));
            return new()
            {
                { () => { _ = ((Wallet)null!) + wallet; } },
                { () => { _ = wallet + ((Wallet)null!); } },
                { () => { _ = ((Wallet)null!) - wallet; } },
                { () => { _ = wallet - ((Wallet)null!); } },
                { () => { _ = ((Wallet)null!) + money; } },
                { () => { _ = ((Wallet)null!) - money; } },
            };
        }
    }

    [Theory]
    [MemberData(nameof(NullBinaryOperatorActions))]
    public void BinaryOperators_WhenEitherOperandIsNull_ThrowArgumentNullException(Action act)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Multiplication_WhenFactorIsScalar_ScalesAllBuckets()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR), new Money(5m, USD));

        // Act
        var leftResult = sut * 2m;
        var rightResult = 2m * sut;

        // Assert
        Assert.Equal(20m, leftResult.Single(m => m.Currency.Equals(EUR)).Amount);
        Assert.Equal(10m, leftResult.Single(m => m.Currency.Equals(USD)).Amount);
        Assert.Equal(leftResult.OrderBy(m => m.Currency.AlphaCode.Value).Select(m => m.Amount),
                     rightResult.OrderBy(m => m.Currency.AlphaCode.Value).Select(m => m.Amount));
    }

    [Fact]
    public void Multiplication_WhenFactorIsZero_ReturnsEmptyWallet()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR));

        // Act
        var result = sut * 0m;

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Multiplication_WhenFactorIsOne_ReturnsEquivalentWallet()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR), new Money(5m, USD));

        // Act
        var result = sut * 1m;

        // Assert
        Assert.Equal(sut, result);
    }

    [Fact]
    public void Division_WhenDivisorIsScalar_ScalesAllBuckets()
    {
        // Arrange
        var sut = Wallet.Of(new Money(10m, EUR), new Money(5m, USD));

        // Act
        var result = sut / 2m;

        // Assert
        var items = result.ToDictionary(m => m.Currency, m => m.Amount);
        Assert.Equal(5m, items[EUR]);
        Assert.Equal(2.5m, items[USD]);
    }

    [Fact]
    public void Division_WhenDivisorIsZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var sut = Wallet.Of(new Money(1m, EUR));

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => sut / 0m);
    }

    public static TheoryData<Action> NullScalarOperatorActions => new()
    {
        { () => { _ = ((Wallet)null!) * 2m; } },
        { () => { _ = 2m * ((Wallet)null!); } },
        { () => { _ = ((Wallet)null!) / 2m; } },
    };

    [Theory]
    [MemberData(nameof(NullScalarOperatorActions))]
    public void ScalarOperators_WhenWalletIsNull_ThrowArgumentNullException(Action act)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(act);
    }
}
