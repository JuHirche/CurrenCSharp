using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CurrenCSharp.Exceptions;

namespace CurrenCSharp;

public sealed partial class Wallet
{
    public sealed class Builder : IEnumerable<KeyValuePair<Currency, Money>>
    {
        private readonly Dictionary<Currency, Money> _dictionary;

        internal Builder(ImmutableDictionary<Currency, Money> dictionary)
        {
            ArgumentNullException.ThrowIfNull(dictionary);
            _dictionary = new(dictionary);
        }

        public Money this[Currency key]
        {
            get => _dictionary[key];
            set
            {
                ArgumentNullException.ThrowIfNull(key);
                DifferentCurrencyException.ThrowIfDifferent(key, value.Currency);
                _dictionary[key] = value;
            }
        }

        public Wallet ToWallet() => Wallet.Of(_dictionary.Values);

        public IEnumerable<Currency> Keys => _dictionary.Keys;

        public IEnumerable<Money> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public void Add(Money money)
        {
            if(ContainsKey(money.Currency))
                _dictionary[money.Currency] += money;
            else
                _dictionary[money.Currency] = money;
        }

        public void AddRange(IEnumerable<Money> moneys)
        {
            ArgumentNullException.ThrowIfNull(moneys);
            foreach (var money in moneys)
                Add(money);
        }

        public bool Remove(Currency key) => _dictionary.Remove(key);

        public void Clear() => _dictionary.Clear();

        public bool ContainsKey(Currency key) => _dictionary.ContainsKey(key);

        public bool TryGetValue(Currency key, [MaybeNullWhen(false)] out Money value) => _dictionary.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<Currency, Money>> GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
