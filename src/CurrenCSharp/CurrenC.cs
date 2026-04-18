using CurrenCSharp.Exceptions;

namespace CurrenCSharp;

/// <summary>
/// Provides ambient library-level configuration for currency handling.
/// </summary>
public static class CurrenC
{
    private static readonly AsyncLocal<DefaultCurrencyScope?> _defaultCurrencyScope = new();

    internal static Currency DefaultCurrency =>
        _defaultCurrencyScope.Value?.Currency ?? throw new NoDefaultCurrencyException();

    /// <summary>
    /// Sets the default currency for the current asynchronous scope.
    /// </summary>
    /// <param name="defaultCurrency">The currency to use as the default inside the created scope.</param>
    /// <returns>An <see cref="IDisposable"/> that restores the previous default currency when disposed.</returns>
    public static IDisposable UseDefaultCurrency(Currency defaultCurrency)
    {
        ArgumentNullException.ThrowIfNull(defaultCurrency);

        var previousScope = _defaultCurrencyScope.Value;
        var scope = new DefaultCurrencyScope(defaultCurrency, previousScope);
        _defaultCurrencyScope.Value = scope;
        return scope;
    }

    private sealed class DefaultCurrencyScope(Currency currency, DefaultCurrencyScope? previousScope) : IDisposable
    {
        private bool _disposed;

        public Currency Currency { get; } = currency;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (!ReferenceEquals(_defaultCurrencyScope.Value, this))
                throw new InvalidOperationException("Default currency scopes must be disposed in LIFO order.");

            _defaultCurrencyScope.Value = previousScope;
        }
    }
}
