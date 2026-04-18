namespace CurrenCSharp.Exceptions;

/// <summary>
/// The exception that is thrown when an operation requires a default currency but none is currently defined.
/// </summary>
public sealed class NoDefaultCurrencyException() : Exception("No default currency is defined.");
