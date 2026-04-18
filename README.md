# CurrenCSharp

## Introduction

CurrenCSharp is a .NET library for handling monetary values.

It provides:
- a `Money` type (amount + currency),
- a `Wallet` type (a collection of monetary values, potentially across multiple currencies),
- a `ContextedMoney` / `ContextedWallet` pair that attaches an exchange-rate context for conversion and cross-currency comparison.

The library supports arithmetic, comparison, conversion, and distribution of amounts.
For exchange rates you implement `IExchangeRateProvider` and plug in your own source.

## Installation

> Note: Not done yet! The library is in early development and not yet published on NuGet. For now, clone the repository and reference the project directly. It should be available for installation via NuGet in the near future.

Install the core package:

```bash
dotnet add package CurrenCSharp
```

Install the optional ISO 4217 package (predefined currencies):

```bash
dotnet add package CurrenCSharp.Currencies
```

## Core Concepts

- `Currency`: an ISO 4217 currency definition (`AlphaCode`, `NumericCode`, `MinorUnits`).
- `Money`: one monetary amount in one currency (for example `EUR 47.11`).
- `Wallet`: a collection of `Money` values, potentially across multiple currencies.
- `ExchangeRateContext`: exchange rates for a base currency at a reference timestamp.
- `ContextedMoney` / `ContextedWallet`: a `Money` / `Wallet` bound to an `ExchangeRateContext` via `.In(context)`, required for conversion or cross-currency comparison.
- `IExchangeRateProvider`: asynchronous source for `ExchangeRateContext` values (latest or historical).

## Feature Examples

> The examples below mirror the end-to-end walkthrough in
> [`example/CurrenCSharp.Example/Program.cs`](example/CurrenCSharp.Example/Program.cs).
> They assume `using CurrenCSharp;` and `using CurrenCSharp.Currencies;`.

### 1. Ambient Default Currency

`CurrenC.UseDefaultCurrency` sets an ambient default currency for the current
async scope. The returned `IDisposable` restores the previous default on
dispose (scopes must be disposed in LIFO order).

```csharp
using var defaultCurrencyScope = CurrenC.UseDefaultCurrency(Iso4217.EUR);
```

While the scope is active, `Currency.Default` and `Money.Zero()` resolve to EUR.

### 2. Creating `Money`

```csharp
Money default_zero = Money.Zero();            // EUR 0.00 (uses default currency)
Money usd_zero    = Money.Zero(Iso4217.USD);  // USD 0.00

Money eur_47_11 = new(47.11m, Iso4217.EUR);
Money usd_47_11 = new(47.11m, Iso4217.USD);
Money chf_23_42 = new(23.42m, Iso4217.CHF);
```

`Money` is an immutable `record struct` and exposes convenience properties
`IsZero`, `IsPositive`, and `IsNegative`.

> **Note:** `Money` is a value type. `default(Money)` is intentionally
> invalid — accessing `Currency` throws `NoCurrencyException`.
> Always construct via `new Money(amount, currency)` or `Money.Zero(currency)`.

### 3. Creating `Wallet`

```csharp
Wallet empty      = Wallet.Empty;
Wallet simple     = Wallet.Of(eur_47_11);
Wallet multiple   = Wallet.Of(eur_47_11, usd_47_11, chf_23_42);
Wallet collection = Wallet.Of([eur_47_11, usd_47_11, chf_23_42]);
```

A `Wallet` aggregates money per currency automatically. It is enumerable
(`IEnumerable<Money>`) and provides structural equality.

### 4. Exchange-Rate Providers

Implement `IExchangeRateProvider` to plug in any rate source (REST API,
database, cache, ...):

```csharp
public interface IExchangeRateProvider
{
    Task<ExchangeRateContext> GetLatestAsync(CancellationToken cancellationToken = default);
    Task<ExchangeRateContext> GetHistoricalAsync(DateTimeOffset date, CancellationToken cancellationToken = default);
}
```

```csharp
IExchangeRateProvider provider = new ExampleExchangeRateProvider();
DateTimeOffset exchangeRateDate = new(new DateTime(2020, 1, 1), TimeSpan.Zero);

ExchangeRateContext latest     = await provider.GetLatestAsync();
ExchangeRateContext historical = await provider.GetHistoricalAsync(exchangeRateDate);
```

See
[`example/CurrenCSharp.Example/ExampleExchangeRateProvider.cs`](example/CurrenCSharp.Example/ExampleExchangeRateProvider.cs)
for a minimal reference implementation.

### 5. Binding Money and Wallet to a Context

Use `.In(context)` to attach an exchange-rate context and enable conversion
and cross-currency comparison.

```csharp
ContextedMoney  latest_money     = eur_47_11.In(latest);
ContextedMoney  historical_money = eur_47_11.In(historical);

ContextedWallet latest_wallet     = collection.In(latest);
ContextedWallet historical_wallet = collection.In(historical);
```

### 6. Currency Conversion

`ContextedMoney.Convert` converts the amount into another currency using
the bound context:

```csharp
Money latest_money_usd     = latest_money.Convert(Iso4217.USD);     // USD 51.41
Money historical_money_usd = historical_money.Convert(Iso4217.USD); // USD 42.08
```

Rounding is controlled via `ConversionOptions`:

```csharp
Money custom = latest_money.Convert(
    Iso4217.USD,
    new ConversionOptions(
        RoundResult: true,
        RoundingMode: MidpointRounding.AwayFromZero,
        Scale: new Scale(4)));
```

`ExchangeRateContext` automatically handles inverse rates and cross rates
via the base currency.

### 7. Wallet Totals

`ContextedWallet.Total()` sums the wallet into a single `Money`. Without a
target currency it uses the wallet's resolved currency (the single currency
in the wallet, or the ambient default).

```csharp
Money latest_total_KeyCurrency = latest_wallet.Total();            // EUR 140.10
Money latest_total_usd         = latest_wallet.Total(Iso4217.USD); // USD 152.87
```

### 8. Arithmetic Operators

#### 8.1 On `Money` (same currency required)

```csharp
Money   negate   = -eur_47_11;            // EUR -47.11
Money   sum      = eur_47_11 + eur_23_42; // EUR 70.53
Money   diff     = eur_47_11 - eur_23_42; // EUR 23.69
Money   multiply = eur_47_11 * 2;         // EUR 94.22
decimal quote    = eur_47_11 / eur_23_42; // 2.01
```

Mixing currencies in `+`, `-`, or `/` throws `DifferentCurrencyException`.

#### 8.2 On `Wallet`

```csharp
Wallet negate      = -collection;            // EUR -47.11, USD -47.11, CHF -47.11
Wallet addition    = collection + eur_23_42; // EUR 70.53, USD 47.11, CHF 47.11
Wallet subtraction = collection - eur_23_42; // EUR 23.69, USD 47.11, CHF 47.11
Wallet multiply    = collection * 3;         // EUR 141.33, USD 141.33, CHF 141.33
Wallet division    = collection / 2;         // EUR 23.56, USD 23.56, CHF 23.56
```

Adding or subtracting `Money` updates the matching currency bucket; adding
two wallets merges them per currency. Scalar `*` / `/` apply to every entry.

### 9. Comparison Operators

Equality and ordering operators are defined for `Money`, `Wallet`, and their
context-aware counterparts. Operators that cross currencies require a
context-bound operand.

#### 9.1 `Money` vs. `Money` (same currency)

```csharp
bool isEqual          = eur_47_11 == eur_47_11; // True
bool isNotEqual       = eur_47_11 != eur_23_42; // True
bool isGreater        = eur_47_11 >  eur_23_42; // True
bool isGreaterOrEqual = eur_47_11 >= eur_23_42; // True
bool isLess           = eur_47_11 <  eur_23_42; // False
bool isLessOrEqual    = eur_47_11 <= eur_23_42; // False
```

#### 9.2 `Money` vs. `ContextedMoney` (currencies may differ)

```csharp
bool a = usd_47_11     == latest_money; // False
bool b = latest_money  >  usd_47_11;    // True
bool c = usd_47_11     <= latest_money; // True
```

#### 9.3 `Money` vs. `ContextedWallet` (currencies may differ)

```csharp
bool a = eur_47_11     == latest_wallet; // False
bool b = latest_wallet >  eur_47_11;     // True
bool c = eur_47_11     <= latest_wallet; // True
```

#### 9.4 `Wallet` vs. `ContextedMoney` (currencies may differ)

```csharp
bool a = collection    == latest_money;  // False
bool b = collection    >  latest_money;  // True
bool c = latest_money  <= collection;    // True
```

#### 9.5 `Wallet` vs. `ContextedWallet` (currencies may differ)

```csharp
bool a = collection    == latest_wallet; // True
bool b = collection    >= latest_wallet; // True
bool c = latest_wallet <= collection;    // True
```

### 10. Distributing Amounts

`Money.Distribute` splits an amount into parts without losing minor units.
Remaining units are allocated to the largest ratios (ties broken by index).

#### 10.1 Equal parts

```csharp
var parts = eur_47_11.Distribute(3);
// EUR 15.71, EUR 15.70, EUR 15.70
```

#### 10.2 Weighted parts via `Ratio`

```csharp
var parts = eur_47_11.Distribute(3, 1, 2, 0);
// EUR 23.56, EUR 7.85, EUR 15.70, EUR 0.00
```

`Ratio` is a non-negative value type with implicit conversion from
`decimal`. The sum of ratios must be greater than zero.

## Strongly Typed Currency Codes

`AlphaCode` and `NumericCode` validate ISO 4217 codes and offer
`Parse` / `TryParse`:

```csharp
AlphaCode   alpha   = AlphaCode.Parse("EUR");
NumericCode numeric = NumericCode.Parse("978");

Currency eur = new(alpha, numeric, 2);
```

## Optional ISO 4217 Package

`CurrenCSharp.Currencies` ships predefined `Currency` instances for all
ISO 4217 codes and a lookup cache:

```csharp
using CurrenCSharp.Currencies;

Currency eur            = Iso4217.EUR;
Currency foundByAlpha   = Iso4217.FindByAlphaCode("USD");
Currency foundByNumeric = Iso4217.FindByNumericCode(840);
```

`FindByAlphaCode` and `FindByNumericCode` throw
`InvalidOperationException` when the code is not defined in ISO 4217.

## License

See [LICENSE](LICENSE).
