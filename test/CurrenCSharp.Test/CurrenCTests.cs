using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed class CurrenCTests
{
    private static readonly Currency Eur = new("EUR", 978, 2);
    private static readonly Currency Usd = new("USD", 840, 2);

    [Fact]
    public void UseDefaultCurrency_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CurrenC.UseDefaultCurrency(null!));
    }

    [Fact]
    public void UseDefaultCurrency_WhenScopesAreNested_RestoresPreviousScope()
    {
        // Arrange
        Exception? captured = null;
        Currency? innerDefault = null;
        Currency? outerDefault = null;

        // Act — run on a dedicated thread to isolate AsyncLocal
        var thread = new Thread(() =>
        {
            using (CurrenC.UseDefaultCurrency(Eur))
            {
                using (CurrenC.UseDefaultCurrency(Usd))
                    innerDefault = Currency.Default;

                outerDefault = Currency.Default;
            }

            captured = Record.Exception(() => _ = Currency.Default);
        });
        thread.Start();
        thread.Join();

        // Assert
        Assert.Equal(Usd, innerDefault);
        Assert.Equal(Eur, outerDefault);
        Assert.IsType<NoDefaultCurrencyException>(captured);
    }

    [Fact]
    public void UseDefaultCurrency_WhenDisposedOutOfOrder_ThrowsInvalidOperationException()
    {
        // Arrange
        Exception? captured = null;

        // Act
        var thread = new Thread(() =>
        {
            var outer = CurrenC.UseDefaultCurrency(Eur);
            var inner = CurrenC.UseDefaultCurrency(Usd);

            captured = Record.Exception(() => outer.Dispose());

            inner.Dispose();
            outer.Dispose();
        });
        thread.Start();
        thread.Join();

        // Assert
        Assert.IsType<InvalidOperationException>(captured);
    }

    [Fact]
    public void UseDefaultCurrency_WhenScopeDisposedTwice_DoesNotThrow()
    {
        // Arrange
        Exception? captured = null;

        // Act
        var thread = new Thread(() =>
        {
            var scope = CurrenC.UseDefaultCurrency(Eur);
            scope.Dispose();
            captured = Record.Exception(() => scope.Dispose());
        });
        thread.Start();
        thread.Join();

        // Assert
        Assert.Null(captured);
    }

    [Fact]
    public async Task UseDefaultCurrency_WhenAwaitBoundaryIsCrossed_PreservesAsyncLocalScope()
    {
        // Arrange & Act — run inside an isolated Task so we don't taint ambient AsyncLocal
        var observed = await Task.Run(async () =>
        {
            using (CurrenC.UseDefaultCurrency(Usd))
            {
                await Task.Yield();
                return Currency.Default;
            }
        });

        // Assert
        Assert.Equal(Usd, observed);
    }

    [Fact]
    public async Task UseDefaultCurrency_WhenUsedInParallelTasks_KeepsTaskLocalDefaults()
    {
        // Arrange
        var taskA = Task.Run(async () =>
        {
            using (CurrenC.UseDefaultCurrency(Eur))
            {
                await Task.Delay(10);
                return Currency.Default;
            }
        });

        var taskB = Task.Run(async () =>
        {
            using (CurrenC.UseDefaultCurrency(Usd))
            {
                await Task.Delay(10);
                return Currency.Default;
            }
        });

        // Act
        var results = await Task.WhenAll(taskA, taskB);

        // Assert
        Assert.Equal(Eur, results[0]);
        Assert.Equal(Usd, results[1]);
    }

    [Fact]
    public void CurrencyDefault_WhenNoScopeExists_ThrowsNoDefaultCurrencyException()
    {
        // Arrange
        Exception? captured = null;

        // Act
        var thread = new Thread(() =>
        {
            captured = Record.Exception(() => _ = Currency.Default);
        });
        thread.Start();
        thread.Join();

        // Assert
        Assert.IsType<NoDefaultCurrencyException>(captured);
    }
}
