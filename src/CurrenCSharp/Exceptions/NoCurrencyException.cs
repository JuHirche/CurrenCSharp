namespace CurrenCSharp.Exceptions;

public sealed class NoCurrencyException() : Exception("Currency is not set in Money.")
{
    internal static void ThrowIfNoCurrency(Currency? currency)
    {
        if (currency is null)
            throw new NoCurrencyException();
    }
}
