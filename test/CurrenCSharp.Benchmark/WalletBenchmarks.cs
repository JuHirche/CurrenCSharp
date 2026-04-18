using BenchmarkDotNet.Attributes;
using CurrenCSharp.Currencies;

namespace CurrenCSharp.Benchmark;

[MemoryDiagnoser]
public class WalletBenchmarks
{
    private static readonly Currency[] Currencies =
    [
        Iso4217.EUR,
        Iso4217.USD,
        Iso4217.GBP,
        Iso4217.JPY,
        Iso4217.CHF,
        Iso4217.CAD,
        Iso4217.AUD,
        Iso4217.PLN,
        Iso4217.SEK,
        Iso4217.NOK
    ];

    private Money[] _moneys = [];

    [Params(1_000, 10_000, 100_000, 1_000_000)]
    public int MoneyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Money[] moneys = new Money[MoneyCount];

        for (int i = 0; i < moneys.Length; i++)
        {
            decimal amount = (i + 1) / 100m;
            Currency currency = Currencies[i % Currencies.Length];

            moneys[i] = new Money(amount, currency);
        }

        _moneys = moneys;
    }

    [Benchmark(Baseline = true)]
    public Wallet CreateWalletFromCollection() => Wallet.Of(_moneys);

    [Benchmark]
    public Wallet CreateWalletByIterativeAdd()
    {
        Wallet wallet = Wallet.Empty;

        foreach (Money money in _moneys)
            wallet += money;

        return wallet;
    }

    [Benchmark]
    public Wallet CreateWalletByBuilder()
    {
        Wallet.Builder builder = Wallet.Empty.ToBuilder();

        foreach (Money money in _moneys)
            builder.Add(money);

        return builder.ToWallet();
    }
}
