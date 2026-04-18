using System.Collections.Immutable;
using FsCheck;
using FsCheck.Fluent;

namespace CurrenCSharp.Test;

public sealed class PropertyTests : TestFixture
{
    [Fact]
    public void DistributeByCount_WhenCalled_PreservesCountCurrencyAndAmount()
    {
        // Arrange
        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            Gen.Choose(1, 100),
            (amountCents, count) => (amountCents, count));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (amountCents, count) = data;
            var amount = amountCents / 100m;
            var money = new Money(amount, EUR);
            var shares = money.Distribute(count);
            var sum = shares.Aggregate(Money.Zero(EUR), (current, next) => current + next);

            return shares.Count == count && shares.All(share => share.Currency == EUR) && sum == money;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void DistributeByCount_WhenCalled_KeepsSpreadWithinOneMinorUnit()
    {
        // Arrange
        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            Gen.Choose(1, 100),
            (amountCents, count) => (amountCents, count));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (amountCents, count) = data;
            var amount = amountCents / 100m;
            var shares = new Money(amount, EUR).Distribute(count).Select(share => share.Amount).ToArray();
            var spread = shares.Max() - shares.Min();

            return spread <= 0.01m;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void DistributeByRatios_WhenCalled_PreservesTotalAndCurrency()
    {
        // Arrange
        var ratioGenerator =
            from first in Gen.Choose(0, 100)
            from second in Gen.Choose(0, 100)
            from third in Gen.Choose(0, 100)
            where first + second + third > 0
            select (first, second, third);

        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            ratioGenerator,
            (amountCents, ratios) => (amountCents, ratios));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (amountCents, ratios) = data;
            var (first, second, third) = ratios;
            var amount = amountCents / 100m;

            var money = new Money(amount, EUR);
            var shares = money.Distribute((Ratio)first, (Ratio)second, (Ratio)third);
            var sum = shares.Aggregate(Money.Zero(EUR), (current, next) => current + next);

            return shares.All(share => share.Currency == EUR) && sum == money;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void DistributeByRatios_WhenCalled_DeviationFromIdealShareIsWithinOneMinorUnit()
    {
        // Arrange
        var ratioGenerator =
            from first in Gen.Choose(0, 100)
            from second in Gen.Choose(0, 100)
            from third in Gen.Choose(0, 100)
            where first + second + third > 0
            select (first, second, third);

        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            ratioGenerator,
            (amountCents, ratios) => (amountCents, ratios));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (amountCents, ratios) = data;
            var (first, second, third) = ratios;
            var amount = amountCents / 100m;

            var money = new Money(amount, EUR);
            var ratioValues = new[] { (decimal)(Ratio)first, (decimal)(Ratio)second, (decimal)(Ratio)third };
            var shares = money.Distribute((Ratio)first, (Ratio)second, (Ratio)third).ToArray();
            var ratioSum = ratioValues.Sum();
            var minorUnit = 1m / (decimal)Math.Pow(10, EUR.MinorUnits);

            for (var i = 0; i < shares.Length; i++)
            {
                var idealShare = amount * (ratioValues[i] / ratioSum);
                if (Math.Abs(shares[i].Amount - idealShare) > minorUnit)
                    return false;
            }

            return true;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ExchangeRateContext_WhenRatesAreValid_ProducesConsistentCrossAndInverseRates()
    {
        // Arrange
        var generator = Gen.Zip(
            Gen.Choose(1, 50_000),
            Gen.Choose(1, 50_000),
            (usdRaw, jpyRaw) => (usdRaw, jpyRaw));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (usdRaw, jpyRaw) = data;
            var usdRate = usdRaw / 10_000m;
            var jpyRate = jpyRaw / 10_000m;

            var context = new ExchangeRateContext(
                EUR,
                DateTimeOffset.UnixEpoch,
                ImmutableDictionary<Currency, ExchangeRate>.Empty
                    .Add(USD, new ExchangeRate(usdRate))
                    .Add(JPY, new ExchangeRate(jpyRate)));

            var usdToJpy = (decimal)context.GetExchangeRate(USD, JPY);
            var expectedUsdToJpy = jpyRate / usdRate;
            var inverseProduct = usdToJpy * (decimal)context.GetExchangeRate(JPY, USD);

            return IsClose(usdToJpy, expectedUsdToJpy)
                   && IsClose(inverseProduct, 1m)
                   && (decimal)context.GetExchangeRate(USD, USD) == 1m;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void WalletPlusNegatedWallet_WhenComputed_CancelsToEmptyWallet()
    {
        // Arrange
        var generator = Gen.ArrayOf(Gen.Choose(-200_000, 200_000), 20);

        var property = Prop.ForAll(Arb.From(generator), cents =>
        {
            var amounts = cents.Select(value => value / 100m).ToArray();
            var wallet = Wallet.Of(amounts.Select(amount => new Money(amount, EUR)).ToArray());
            var canceled = wallet + (-wallet);

            return !canceled.Any();
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void MoneyAddition_WhenCurrenciesMatch_IsCommutative()
    {
        // Arrange
        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            Gen.Choose(-1_000_000, 1_000_000),
            (leftCents, rightCents) => (leftCents, rightCents));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (leftCents, rightCents) = data;
            var left = new Money(leftCents / 100m, EUR);
            var right = new Money(rightCents / 100m, EUR);

            return left + right == right + left;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void MoneyAddition_WhenCurrenciesMatch_IsAssociative()
    {
        // Arrange
        var generator =
            from firstCents in Gen.Choose(-1_000_000, 1_000_000)
            from secondCents in Gen.Choose(-1_000_000, 1_000_000)
            from thirdCents in Gen.Choose(-1_000_000, 1_000_000)
            select (firstCents, secondCents, thirdCents);

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (firstCents, secondCents, thirdCents) = data;
            var first = new Money(firstCents / 100m, EUR);
            var second = new Money(secondCents / 100m, EUR);
            var third = new Money(thirdCents / 100m, EUR);

            return (first + second) + third == first + (second + third);
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void MoneyPlusNegation_WhenCurrenciesMatch_ReturnsZeroMoney()
    {
        // Arrange
        var generator = Gen.Choose(-1_000_000, 1_000_000);

        var property = Prop.ForAll(Arb.From(generator), cents =>
        {
            var money = new Money(cents / 100m, EUR);
            var result = money + (-money);

            return result == Money.Zero(EUR);
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void MoneyComparison_WhenCurrenciesMatch_IsAntisymmetric()
    {
        // Arrange
        var generator = Gen.Zip(
            Gen.Choose(-1_000_000, 1_000_000),
            Gen.Choose(-1_000_000, 1_000_000),
            (leftCents, rightCents) => (leftCents, rightCents));

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (leftCents, rightCents) = data;
            var left = new Money(leftCents / 100m, EUR);
            var right = new Money(rightCents / 100m, EUR);

            var leftToRight = Math.Sign(left.CompareTo(right));
            var rightToLeft = Math.Sign(right.CompareTo(left));

            return leftToRight == -rightToLeft;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void MoneyComparison_WhenCurrenciesMatch_IsTransitive()
    {
        // Arrange
        var generator =
            from leftCents in Gen.Choose(-1_000_000, 1_000_000)
            from middleCents in Gen.Choose(-1_000_000, 1_000_000)
            from rightCents in Gen.Choose(-1_000_000, 1_000_000)
            select (leftCents, middleCents, rightCents);

        var property = Prop.ForAll(Arb.From(generator), data =>
        {
            var (leftCents, middleCents, rightCents) = data;
            var left = new Money(leftCents / 100m, EUR);
            var middle = new Money(middleCents / 100m, EUR);
            var right = new Money(rightCents / 100m, EUR);

            return !(left <= middle && middle <= right) || left <= right;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void AlphaCodeParseToString_WhenCodeIsValid_RoundTrips()
    {
        // Arrange
        var generator =
            from first in Gen.Choose(0, 25)
            from second in Gen.Choose(0, 25)
            from third in Gen.Choose(0, 25)
            select new string([(char)('A' + first), (char)('A' + second), (char)('A' + third)]);

        var property = Prop.ForAll(Arb.From(generator), code =>
        {
            var parsed = AlphaCode.Parse(code);
            return parsed.ToString() == code;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void NumericCodeParseToString_WhenCodeIsValid_RoundTrips()
    {
        // Arrange
        var generator = Gen.Choose(0, 999).Select(value => value.ToString("D3"));

        var property = Prop.ForAll(Arb.From(generator), code =>
        {
            var parsed = NumericCode.Parse(code);
            return parsed.ToString() == code;
        });

        // Act
        var exception = Record.Exception(() => property.QuickCheckThrowOnFailure());

        // Assert
        Assert.Null(exception);
    }

    private static bool IsClose(decimal left, decimal right) =>
        Math.Abs(left - right) <= 0.000000000000000001m;
}
