using System.Reflection;

namespace CurrenCSharp.Currencies;

internal class Iso4217Cache
{
    private sealed record CacheData(
        Dictionary<AlphaCode, Currency> ByAlphaCode,
        Dictionary<NumericCode, Currency> ByNumericCode
    );

    private readonly Lazy<CacheData> Cache = new(CreateCache);

    public Currency? FindByAlphaCode(AlphaCode alphaCode) =>
        Cache.Value.ByAlphaCode.GetValueOrDefault(alphaCode);

    public Currency? FindByNumericCode(NumericCode numericCode) =>
        Cache.Value.ByNumericCode.GetValueOrDefault(numericCode);

    private static CacheData CreateCache()
    {
        var currencies = typeof(Iso4217)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(x => x.FieldType == typeof(Currency))
            .Select(x => (Currency)x.GetValue(null)!)
            .ToList();

        return new CacheData(
            currencies.ToDictionary(x => x.AlphaCode),
            currencies.ToDictionary(x => x.NumericCode));
    }
}
