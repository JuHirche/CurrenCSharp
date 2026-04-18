using System.Collections;
using System.Collections.Immutable;

namespace CurrenCSharp;

internal sealed class MoneyCollection(ImmutableDictionary<Currency, Money> items) : IEnumerable<Money>, IEquatable<MoneyCollection>
{
    public ImmutableDictionary<Currency, Money> Items => items;

    public static MoneyCollection Empty { get; } = new(ImmutableDictionary<Currency, Money>.Empty);

    public static MoneyCollection Create(IEnumerable<Money> moneys)
    {
        ArgumentNullException.ThrowIfNull(moneys, nameof(moneys));

        return Aggregate([], moneys);
    }

    public MoneyCollection Add(Money money)
    {
        Money newMoney = items.TryGetValue(money.Currency, out var existing) ? existing + money : money;

        return new(!newMoney.IsZero
            ? items.SetItem(money.Currency, newMoney)
            : items.Remove(money.Currency)
        );
    }

    public MoneyCollection Union(MoneyCollection otherCollection)
    {
        ArgumentNullException.ThrowIfNull(otherCollection);

        return otherCollection switch
        {
            { Items.Count: 0 } => this,
            _ when items.Count == 0 => otherCollection,
            _ => Aggregate(new(items), otherCollection)
        };
    }

    public MoneyCollection Negate() => Mutate(money => -money);

    public MoneyCollection MultiplyBy(decimal factor) => Mutate(money => money * factor);

    public MoneyCollection DivideBy(decimal divisor) =>
        divisor != 0
            ? Mutate(money => money / divisor)
            : throw new DivideByZeroException("Cannot divide by zero.");

    private MoneyCollection Mutate(Func<Money, Money> mutation)
    {
        var builder = items.ToBuilder();
        foreach (var (key, value) in items)
        {
            Money newMoney = mutation(value);
            if (!newMoney.IsZero)
                builder[key] = newMoney;
            else
                builder.Remove(key);
        }

        return new(builder.ToImmutableDictionary());
    }

    public Currency? SingleCurrencyOrDefault() =>
        items.Count == 1 ? items.Keys.First() : null;

    public IEnumerator<Money> GetEnumerator() => items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(MoneyCollection? other) =>
        other is not null && (ReferenceEquals(this, other) ||
        items.Count == other.Items.Count &&
        items.All(kvp => other.Items.TryGetValue(kvp.Key, out var otherMoney) && kvp.Value == otherMoney));

    public override bool Equals(object? obj) => Equals(obj as MoneyCollection);

    public override int GetHashCode() =>
        items.Aggregate(0, (hash, kvp) => hash ^ HashCode.Combine(kvp.Key, kvp.Value));

    private static MoneyCollection Aggregate(Dictionary<Currency, Money> seed, IEnumerable<Money> other)
    {
        foreach (Money money in other)
        { 
            Currency currency = money.Currency;
            
            if (money.IsZero)
                continue;

            if (!seed.TryGetValue(currency, out Money currentValue))
            {
                seed.Add(currency, money);
                continue;
            }

            Money nextValue = currentValue + money;

            if (!nextValue.IsZero)
                seed[currency] = nextValue;
            else
                seed.Remove(currency);
        }

        return new(seed.ToImmutableDictionary());
    }
}
