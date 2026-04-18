using CurrenCSharp.Exceptions;

namespace CurrenCSharp.Test;

public sealed class CurrenCTests
{
    private static readonly Currency Eur = new("EUR", 978, 2);
    private static readonly Currency Usd = new("USD", 840, 2);

    [Fact]
    public void UseDefaultCurrency_WhenCurrencyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Currency currency = null!;

        // Act
        var exception = Record.Exception(() => _ = CurrenC.UseDefaultCurrency(currency));

        // Assert
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void UseDefaultCurrency_WhenScopesAreNested_RestoresPreviousScope()
    {
        // Arrange
        Currency? innerDefault = null;
        Currency? outerDefault = null;

        // Act
        using (CurrenC.UseDefaultCurrency(Eur))
        {
            using (CurrenC.UseDefaultCurrency(Usd))
                innerDefault = Currency.Default;

            outerDefault = Currency.Default;
        }

        var exception = Record.Exception(() => _ = Currency.Default);

        // Assert
        Assert.Equal(Usd, innerDefault);
        Assert.Equal(Eur, outerDefault);
        Assert.IsType<NoDefaultCurrencyException>(exception);
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
        });
        thread.Start();
        thread.Join();

        // Assert
        Assert.IsType<InvalidOperationException>(captured);
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
