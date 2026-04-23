namespace CurrenCSharp.Test;

public sealed class ConcurrencyTests : TestFixture
{
    [Fact]
    public async Task DefaultCurrency_WhenUsedAcrossAsyncAwaitBoundaries_RemainsIsolatedPerFlow()
    {
        // Arrange
        var currencies = new[] { EUR, USD, JPY };
        var tasks = Enumerable.Range(0, 50).Select(i =>
            Task.Run(async () =>
            {
                var expected = currencies[i % currencies.Length];
                using (CurrenC.UseDefaultCurrency(expected))
                {
                    await Task.Yield();
                    await Task.Delay(Random.Shared.Next(0, 5));
                    await Task.Yield();
                    return (expected, actual: Currency.Default);
                }
            })).ToArray();

        // Act
        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, r => Assert.Equal(r.expected, r.actual));
    }

    [Fact]
    public void DefaultCurrency_WhenScopeIsDisposedInOtherThread_DoesNotAffectOtherAsyncLocalStacks()
    {
        // Arrange
        IDisposable? scopeA = null;
        Currency? observedInB = null;
        var ready = new ManualResetEventSlim(false);

        // Act — Thread A opens a scope, Thread B observes its own ambient state
        var threadA = new Thread(() =>
        {
            scopeA = CurrenC.UseDefaultCurrency(USD);
            ready.Set();
            Thread.Sleep(50);
            scopeA.Dispose();
        });

        var threadB = new Thread(() =>
        {
            ready.Wait();
            // Thread B has its own AsyncLocal root — no default currency visible
            try
            {
                observedInB = Currency.Default;
            }
            catch
            {
                observedInB = null;
            }
        });

        threadA.Start();
        threadB.Start();
        threadA.Join();
        threadB.Join();

        // Assert — Thread B never saw USD because AsyncLocal is flow-scoped
        Assert.NotEqual(USD, observedInB);
    }

    [Fact]
    public void GetExchangeRate_WhenReadConcurrentlyOnSharedInstance_IsRaceFree()
    {
        // Arrange
        var ctx = new ExchangeRateContext(EUR, DateTimeOffset.UnixEpoch, LatestExchangeRates);

        // Act
        var results = Enumerable.Range(0, 10_000)
            .AsParallel()
            .WithDegreeOfParallelism(16)
            .Select(_ =>
            {
                var direct = (decimal)ctx.GetExchangeRate(EUR, USD);
                var inverse = (decimal)ctx.GetExchangeRate(USD, EUR);
                var cross = (decimal)ctx.GetExchangeRate(USD, JPY);
                return (direct, inverse, cross);
            })
            .ToList();

        // Assert
        Assert.All(results, r =>
        {
            Assert.Equal(2m, r.direct);
            Assert.Equal(0.5m, r.inverse);
            Assert.Equal(1.5m, r.cross);
        });
    }

    [Fact]
    public void Wallet_WhenSharedReadOnlyAcrossThreads_IsSafeForParallelEnumeration()
    {
        // Arrange
        var moneys = Enumerable.Range(1, 100)
            .Select(i => new Money(i, i % 2 == 0 ? EUR : USD))
            .ToArray();
        var wallet = Wallet.Of(moneys);

        // Act
        var counts = Enumerable.Range(0, 16)
            .AsParallel()
            .WithDegreeOfParallelism(16)
            .Select(_ => wallet.Count())
            .ToList();

        // Assert — all threads observe the same count
        Assert.Single(counts.Distinct());
    }

    [Fact]
    public void Builder_WhenMutatedFromSingleThread_AggregatesDeterministically()
    {
        // Arrange — by design, the builder is not thread-safe. We document the
        // thread-affine contract by showing that single-threaded mutation is deterministic.
        var builder = Wallet.Empty.ToBuilder();

        // Act
        for (int i = 0; i < 1_000; i++)
            builder.Add(new Money(1m, EUR));

        // Assert
        Assert.Equal(1_000m, builder[EUR].Amount);
    }
}
