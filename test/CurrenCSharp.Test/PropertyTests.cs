using System.Collections.Immutable;
using FsCheck;
using FsCheck.Fluent;

namespace CurrenCSharp.Test;

public sealed class PropertyTests : TestFixture
{
    [Fact]
    public void Distribute_WhenCalled_PreservesTotalCurrencyAndCount_Property()
    {
        // Arrange
        var gen =
            from amountCents in Gen.Choose(-1_000_000, 1_000_000)
            from count in Gen.Choose(1, 100)
            select (amountCents, count);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var money = new Money(data.amountCents / 100m, EUR);
            var shares = money.Distribute(data.count);
            var sum = shares.Aggregate(Money.Zero(EUR), (c, n) => c + n);

            return shares.Count == data.count
                && shares.All(s => s.Currency == EUR)
                && sum == money;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Distribute_WhenCalled_DeviationIsAtMostOneMinorUnit_Property()
    {
        // Arrange
        var gen =
            from amountCents in Gen.Choose(-1_000_000, 1_000_000)
            from first in Gen.Choose(0, 100)
            from second in Gen.Choose(0, 100)
            from third in Gen.Choose(0, 100)
            where first + second + third > 0
            select (amountCents, first, second, third);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var amount = data.amountCents / 100m;
            var money = new Money(amount, EUR);
            var ratios = new decimal[] { data.first, data.second, data.third };
            var shares = money.Distribute((Ratio)data.first, (Ratio)data.second, (Ratio)data.third).ToArray();
            var ratioSum = ratios.Sum();
            var minorUnit = 1m / (decimal)Math.Pow(10, EUR.MinorUnits);

            for (int i = 0; i < shares.Length; i++)
            {
                var ideal = amount * (ratios[i] / ratioSum);
                if (Math.Abs(shares[i].Amount - ideal) > minorUnit)
                    return false;
            }
            return true;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void GetExchangeRate_WhenRatesAreValid_IsInverseAndCrossConsistent_Property()
    {
        // Arrange
        var gen =
            from usdRaw in Gen.Choose(1, 50_000)
            from jpyRaw in Gen.Choose(1, 50_000)
            select (usdRaw, jpyRaw);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var usdRate = data.usdRaw / 10_000m;
            var jpyRate = data.jpyRaw / 10_000m;

            var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
                ImmutableDictionary<Currency, ExchangeRate>.Empty
                    .Add(USD, new ExchangeRate(usdRate))
                    .Add(JPY, new ExchangeRate(jpyRate)));

            var cross = (decimal)ctx.GetExchangeRate(USD, JPY);
            var expected = jpyRate / usdRate;
            var inverseProduct = cross * (decimal)ctx.GetExchangeRate(JPY, USD);

            return IsClose(cross, expected)
                && IsClose(inverseProduct, 1m)
                && (decimal)ctx.GetExchangeRate(USD, USD) == 1m;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Of_WhenInputOrderChanges_RemainsEqual_Property()
    {
        // Arrange
        var gen = Gen.ArrayOf(Gen.Choose(-200_000, 200_000), 10);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var moneys = cents.Select(v => new Money(v / 100m, EUR)).ToArray();
            var wallet1 = Wallet.Of(moneys);
            var reversed = moneys.Reverse().ToArray();
            var wallet2 = Wallet.Of(reversed);

            return wallet1.Equals(wallet2);
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Convert_WhenRoundResultIsFalse_RoundTripAtoBtoA_IsWithinTolerance_Property()
    {
        // Arrange
        var gen =
            from amountCents in Gen.Choose(1, 1_000_000)
            from rateRaw in Gen.Choose(1, 50_000)
            select (amountCents, rateRaw);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var rate = data.rateRaw / 10_000m;
            var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch,
                ImmutableDictionary<Currency, ExchangeRate>.Empty.Add(USD, new ExchangeRate(rate)));

            var amount = data.amountCents / 100m;
            var original = new Money(amount, EUR);
            var options = new ConversionOptions(RoundResult: false);

            var inUsd = original.In(ctx).Convert(USD, options);
            var backToEur = inUsd.In(ctx).Convert(EUR, options);

            return Math.Abs(backToEur.Amount - amount) <= 0.000000000000000001m;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Addition_WhenSameCurrency_IsCommutative_Property()
    {
        // Arrange
        var gen =
            from a in Gen.Choose(-1_000_000, 1_000_000)
            from b in Gen.Choose(-1_000_000, 1_000_000)
            select (a, b);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var left = new Money(data.a / 100m, EUR);
            var right = new Money(data.b / 100m, EUR);
            return left + right == right + left;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Addition_WhenSameCurrency_IsAssociative_Property()
    {
        // Arrange
        var gen =
            from a in Gen.Choose(-1_000_000, 1_000_000)
            from b in Gen.Choose(-1_000_000, 1_000_000)
            from c in Gen.Choose(-1_000_000, 1_000_000)
            select (a, b, c);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var a = new Money(data.a / 100m, EUR);
            var b = new Money(data.b / 100m, EUR);
            var c = new Money(data.c / 100m, EUR);
            return (a + b) + c == a + (b + c);
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void AdditionWithNegation_WhenSameCurrency_ReturnsZero_Property()
    {
        // Arrange
        var gen = Gen.Choose(-1_000_000, 1_000_000);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var money = new Money(cents / 100m, EUR);
            return money + (-money) == Money.Zero(EUR);
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CompareTo_WhenSameCurrency_IsAntisymmetricAndTransitive_Property()
    {
        // Arrange
        var antiSymGen =
            from a in Gen.Choose(-1_000_000, 1_000_000)
            from b in Gen.Choose(-1_000_000, 1_000_000)
            select (a, b);

        var antiSym = Prop.ForAll(Arb.From(antiSymGen), data =>
        {
            var l = new Money(data.a / 100m, EUR);
            var r = new Money(data.b / 100m, EUR);
            return Math.Sign(l.CompareTo(r)) == -Math.Sign(r.CompareTo(l));
        });

        var transGen =
            from a in Gen.Choose(-1_000_000, 1_000_000)
            from b in Gen.Choose(-1_000_000, 1_000_000)
            from c in Gen.Choose(-1_000_000, 1_000_000)
            select (a, b, c);

        var trans = Prop.ForAll(Arb.From(transGen), data =>
        {
            var l = new Money(data.a / 100m, EUR);
            var m = new Money(data.b / 100m, EUR);
            var r = new Money(data.c / 100m, EUR);
            return !(l <= m && m <= r) || l <= r;
        });

        // Act
        var ex1 = Record.Exception(antiSym.QuickCheckThrowOnFailure);
        var ex2 = Record.Exception(trans.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(ex1);
        Assert.Null(ex2);
    }

    [Fact]
    public void WalletPlusNegatedWallet_WhenComputed_CancelsToEmptyWallet_Property()
    {
        // Arrange
        var gen = Gen.ArrayOf(Gen.Choose(-200_000, 200_000), 20);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var wallet = Wallet.Of(cents.Select(v => new Money(v / 100m, EUR)).ToArray());
            return !(wallet + (-wallet)).Any();
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ParseToString_WhenAlphaCodeIsValid_RoundTrips_Property()
    {
        // Arrange
        var gen =
            from a in Gen.Choose(0, 25)
            from b in Gen.Choose(0, 25)
            from c in Gen.Choose(0, 25)
            select new string(new[] { (char)('A' + a), (char)('A' + b), (char)('A' + c) });

        var property = Prop.ForAll(Arb.From(gen), code =>
            AlphaCode.Parse(code).ToString() == code);

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ParseToString_WhenNumericCodeIsValid_RoundTrips_Property()
    {
        // Arrange
        var gen = Gen.Choose(0, 999).Select(v => v.ToString("D3"));

        var property = Prop.ForAll(Arb.From(gen), code =>
            NumericCode.Parse(code).ToString() == code);

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Subtraction_AddingBackRightOperand_EqualsLeftOperand_Property()
    {
        // Arrange
        var gen =
            from a in Gen.Choose(-1_000_000, 1_000_000)
            from b in Gen.Choose(-1_000_000, 1_000_000)
            select (a, b);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var a = new Money(data.a / 100m, EUR);
            var b = new Money(data.b / 100m, EUR);
            return (a - b) + b == a;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Multiplication_ByOne_IsIdentity_Property()
    {
        // Arrange
        var gen = Gen.Choose(-1_000_000, 1_000_000);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var money = new Money(cents / 100m, EUR);
            return money * 1m == money;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Multiplication_ByMinusOne_EqualsNegation_Property()
    {
        // Arrange
        var gen = Gen.Choose(-1_000_000, 1_000_000);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var money = new Money(cents / 100m, EUR);
            return money * -1m == -money;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Abs_WhenAppliedTwice_IsIdempotent_Property()
    {
        // Arrange
        var gen = Gen.Choose(-1_000_000, 1_000_000);

        var property = Prop.ForAll(Arb.From(gen), cents =>
        {
            var money = new Money(cents / 100m, EUR);
            return money.Abs().Abs() == money.Abs();
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Round_WhenAppliedTwiceWithSameArgs_IsIdempotent_Property()
    {
        // Arrange
        var gen =
            from cents in Gen.Choose(-1_000_000, 1_000_000)
            from decimals in Gen.Choose(0, 10)
            select (cents, decimals);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var money = new Money(data.cents / 100m, EUR);
            var once = money.Round(data.decimals, MidpointRounding.ToEven);
            var twice = once.Round(data.decimals, MidpointRounding.ToEven);
            return once == twice;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Multiplication_DivisionByNonZeroScalar_RoundTrips_Property()
    {
        // Arrange
        var gen =
            from cents in Gen.Choose(-10_000, 10_000)
            from factor in Gen.Choose(1, 10)
            select (cents, factor);

        var property = Prop.ForAll(Arb.From(gen), data =>
        {
            var wallet = Wallet.Of(new Money(data.cents / 100m, EUR));
            var round = (wallet * data.factor) / data.factor;
            return wallet.Equals(round);
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ExchangeRate_InverseOfInverse_EqualsOriginal_Property()
    {
        // Arrange
        var gen = Gen.Choose(1, 100_000);

        var property = Prop.ForAll(Arb.From(gen), raw =>
        {
            var rate = raw / 1_000m;
            var inverse = 1m / rate;
            var doubleInverse = 1m / inverse;
            return Math.Abs(doubleInverse - rate) <= 0.000000000001m;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Builder_WhenRandomOperationsApplied_MatchesReferenceModel_Property()
    {
        // Arrange — generate 1..50 Add/Remove/Clear operations
        var opGen =
            from kind in Gen.Choose(0, 3) // 0=Add, 1=Remove, 2=Clear, 3=Add (weight)
            from curr in Gen.Choose(0, 2) // 0=EUR, 1=USD, 2=JPY
            from amt in Gen.Choose(-1000, 1000)
            select (kind, curr, amt);
        var seqGen = Gen.ArrayOf(opGen);

        var property = Prop.ForAll(Arb.From(seqGen), ops =>
        {
            var builder = Wallet.Empty.ToBuilder();
            var model = new Dictionary<Currency, decimal>();
            var currencies = new[] { EUR, USD, JPY };

            foreach (var (kind, currIdx, amt) in ops)
            {
                var c = currencies[currIdx];
                if (kind == 2)
                {
                    builder.Clear();
                    model.Clear();
                }
                else if (kind == 1)
                {
                    builder.Remove(c);
                    model.Remove(c);
                }
                else
                {
                    var money = new Money(amt / 10m, c);
                    builder.Add(money);
                    model.TryGetValue(c, out var existing);
                    var next = existing + money.Amount;
                    if (next == 0m)
                        model.Remove(c);
                    else
                        model[c] = next;
                }
            }

            // Verify builder matches model
            if (builder.Count != model.Count) return false;
            foreach (var (c, amount) in model)
            {
                if (!builder.TryGetValue(c, out var money)) return false;
                if (money.Amount != amount) return false;
            }
            return true;
        });

        // Act
        var exception = Record.Exception(property.QuickCheckThrowOnFailure);

        // Assert
        Assert.Null(exception);
    }

    private static bool IsClose(decimal left, decimal right) =>
        Math.Abs(left - right) <= 0.000000000000000001m;
}
