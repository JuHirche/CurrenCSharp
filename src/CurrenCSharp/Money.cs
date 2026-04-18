using System.Globalization;
using CurrenCSharp.Exceptions;

namespace CurrenCSharp;

/// <summary>
/// Represents a monetary amount in a specific currency.
/// </summary>
public readonly partial record struct Money
{
    private readonly Currency? _currency;
    
    public decimal Amount { get; }
    public Currency Currency => _currency ?? throw new NoCurrencyException();

    public Money(decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        Amount = amount;
        _currency = currency;
    }

    /// <summary>
    /// Binds this monetary value to an exchange-rate context.
    /// </summary>
    /// <param name="context">The exchange-rate context to bind.</param>
    /// <returns>A context-aware monetary value.</returns>
    public ContextedMoney In(ExchangeRateContext context)
    {
        NoCurrencyException.ThrowIfNoCurrency(_currency);
        ArgumentNullException.ThrowIfNull(context);
        return new ContextedMoney(this, context);
    }

    /// <summary>
    /// Creates a zero monetary value in the ambient default currency.
    /// </summary>
    /// <returns>A zero monetary value.</returns>
    public static Money Zero() => Zero(Currency.Default);

    /// <summary>
    /// Creates a zero monetary value in a specific currency.
    /// </summary>
    /// <param name="currency">The currency of the zero value.</param>
    /// <returns>A zero monetary value in <paramref name="currency"/>.</returns>
    public static Money Zero(Currency currency) => new(decimal.Zero, currency);

    public bool IsZero => Amount == decimal.Zero;

    public bool IsPositive => Amount > decimal.Zero;

    public bool IsNegative => Amount < decimal.Zero;

    /// <summary>
    /// Returns the absolute value of this monetary amount.
    /// </summary>
    /// <returns>A monetary value with the absolute amount.</returns>
    public Money Abs() => new(Math.Abs(Amount), Currency);

    /// <summary>
    /// Rounds this monetary amount to the currency's minor units.
    /// </summary>
    /// <param name="mode">The midpoint rounding strategy to apply.</param>
    /// <returns>A monetary value with the rounded amount.</returns>
    public Money Round(MidpointRounding mode = MidpointRounding.ToEven) =>
        Round(Currency.MinorUnits, mode);

    /// <summary>
    /// Rounds this monetary amount to the specified number of decimal places.
    /// </summary>
    /// <param name="decimals">The number of fractional digits in the rounded result.</param>
    /// <param name="mode">The midpoint rounding strategy to apply.</param>
    /// <returns>A monetary value with the rounded amount.</returns>
    public Money Round(int decimals, MidpointRounding mode = MidpointRounding.ToEven) =>
        new(Math.Round(Amount, decimals, mode), Currency);

    /// <summary>
    /// Returns the smaller of two monetary values.
    /// </summary>
    /// <param name="left">The first monetary value.</param>
    /// <param name="right">The second monetary value.</param>
    /// <returns>The monetary value with the smaller amount.</returns>
    public static Money Min(Money left, Money right)
    {
        DifferentCurrencyException.ThrowIfDifferent(left.Currency, right.Currency);
        return left.Amount <= right.Amount ? left : right;
    }

    /// <summary>
    /// Returns the larger of two monetary values.
    /// </summary>
    /// <param name="left">The first monetary value.</param>
    /// <param name="right">The second monetary value.</param>
    /// <returns>The monetary value with the larger amount.</returns>
    public static Money Max(Money left, Money right)
    {
        DifferentCurrencyException.ThrowIfDifferent(left.Currency, right.Currency);
        return left.Amount >= right.Amount ? left : right;
    }

    /// <summary>
    /// Distributes this monetary amount into equal parts.
    /// </summary>
    /// <param name="count">The number of parts to create.</param>
    /// <returns>A read-only collection containing all distributed parts.</returns>
    public IReadOnlyCollection<Money> Distribute(int count) =>
        count > 0
            ? Distribute([.. Enumerable.Repeat(1m, count)])
            : throw new ArgumentOutOfRangeException(nameof(count), count, "The count must be greater than zero.");

    /// <summary>
    /// Distributes this monetary amount according to weighted ratios.
    /// </summary>
    /// <param name="ratios">The ratios that define the distribution weights.</param>
    /// <returns>A read-only collection containing the distributed shares.</returns>
    public IReadOnlyCollection<Money> Distribute(params Ratio[] ratios)
    {
        if (ratios is null || ratios.Length == 0)
            throw new ArgumentException("At least one ratio must be provided.", nameof(ratios));

        decimal total = ratios.Sum(x => x);

        if (total == 0)
            throw new ArgumentException("The sum of ratios must be greater than zero.", nameof(ratios));

        Money money = this;
        List<Money> shares = [.. ratios.Select(ratio => new Money(Math.Round(money.Amount * (ratio / total), money.Currency.MinorUnits, MidpointRounding.ToZero), money.Currency))];
        Money diff = shares.Aggregate(this, (current, next) => current - next);
        int factor = diff.IsZero || diff.IsPositive ? 1 : -1;
        Money unit = new(1 / (decimal)Math.Pow(10, Currency.MinorUnits), Currency);
        int countUnits = (int)Math.Abs(diff / unit);

        var orderedIndizesByRatio =
            ratios.Select((ratio, index) => (ratio, index))
                  .OrderByDescending(x => x.ratio)
                  .ThenBy(x => x.index)
                  .Select(x => x.index)
                  .Take(countUnits);

        foreach (var i in orderedIndizesByRatio)
            shares[i] += unit * factor;

        return shares.AsReadOnly();
    }

    /// <summary>
    /// Returns a culture-aware currency string representation of this monetary value.
    /// </summary>
    /// <returns>A formatted currency string.</returns>
    public override string ToString() => Amount.ToString("C", CreateCultureInfo());

    private CultureInfo CreateCultureInfo()
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.NumberFormat.CurrencySymbol = Currency.AlphaCode;
        culture.NumberFormat.CurrencyDecimalDigits = Currency.MinorUnits;
        culture.NumberFormat.CurrencyPositivePattern = culture.NumberFormat.CurrencyPositivePattern switch
        {
            0 => 2, // $n -> $ n
            1 => 3, // n$ -> n $
            _ => culture.NumberFormat.CurrencyPositivePattern
        };

        return culture;
    }
}
