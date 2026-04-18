namespace CurrenCSharp;

/// <summary>
/// Defines a provider for obtaining exchange-rate contexts.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Gets the latest available exchange-rate context.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the asynchronous operation.</param>
    /// <returns>A task that resolves to the latest <see cref="ExchangeRateContext"/>.</returns>
    Task<ExchangeRateContext> GetLatestAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a historical exchange-rate context for the specified date.
    /// </summary>
    /// <param name="date">The point in time for which exchange rates are requested.</param>
    /// <param name="cancellationToken">A token used to cancel the asynchronous operation.</param>
    /// <returns>A task that resolves to the requested <see cref="ExchangeRateContext"/>.</returns>
    Task<ExchangeRateContext> GetHistoricalAsync(DateTimeOffset date, CancellationToken cancellationToken = default);
}
