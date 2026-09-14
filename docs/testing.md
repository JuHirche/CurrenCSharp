# Testing conventions

Applies to `test/CurrenCSharp.Test` and `test/CurrenCSharp.Currencies.Test`.
Read this before adding or changing tests.

## Frameworks

- **xUnit v3**: `[Fact]`, `[Theory]` with `TheoryData<...>` exposed as a static property
  and consumed via `[MemberData(nameof(...))]`. Prefer `TheoryData` over `[InlineData]`
  when a case has more than two or three values or uses non-primitive types.
- **FsCheck** for property-based tests (`PropertyTests.cs`). Use it for invariants that
  hold for all inputs, e.g. `Distribute` preserving the total.
- **Verify.XunitV3** is referenced but not currently used. Introduce snapshot tests only
  for large formatted output where a hand-written assertion would be unreadable.
- Custom types used as theory data (`Currency`, `Money`, `Wallet`, `ConversionOptions`,
  `ExchangeRateContext`) are serialized through `XunitSerializer.cs`. Register any new
  type there before using it in `TheoryData`.

## Files and classes

- One test class per production type, named `<TypeName>Tests`, e.g. `CurrencyTests`.
- Split by concern exactly like the production partials: `MoneyTests.cs` holds
  construction and basics, `MoneyTests.Arithmetic.cs`, `MoneyTests.Comparison.cs`,
  `MoneyTests.Distribution.cs`, `MoneyTests.Formatting.cs` hold the rest. Declare the
  class `partial` in every file; the main file carries the base class.
- Equality and ordering tests (`==`, `!=`, `<`, `>`, `Equals`, `CompareTo`,
  `GetHashCode`) belong in the `*.Comparison.cs` file, never in a separate `*.Equality.cs`.
- Mark classes `sealed`; `sealed partial` is fine for the split classes.

## Fixture

- Inherit from `TestFixture` when a test needs the ambient default currency or exchange
  rates. The fixture sets `EUR` as default via `CurrenC.UseDefaultCurrency` and disposes
  the scope afterwards.
- Use the shared constants `EUR`, `USD`, `JPY` (JPY has zero minor units, useful for
  rounding cases) and the static `LatestExchangeRates` / `HistoricalExchangeRates`
  (USD 2 / 22, JPY 3 / 33 against EUR). Do not define new currencies inline unless the
  test is about currency construction itself.
- `ExchangeRateProvider` on the fixture returns those rates; use it instead of a new mock.
- Tests that do not touch currency defaults or rates (`AlphaCodeTests`, `RatioTests`,
  `ScaleTests`, ...) do not inherit the fixture.

## Test methods

- Name: `MethodName_StateUnderTest_ExpectedBehavior`, e.g.
  `Convert_WhenTargetCurrencyMatchesSource_ReturnsOriginalAmount`. For operators use the
  group name as method name (`EqualityOperators_...`, `OrderOperators_...`,
  `CrossTypeComparisonOperators_...`); `Equals_...` and `CompareTo_...` for the methods.
  Property tests end with `_Property`.
- Structure: strictly Arrange / Act / Assert with the comments `// Arrange`, `// Act`,
  `// Assert`. Keep the three blocks even when one is a single line.
- One observable behavior per test. Split instead of adding a second `Act`.
- Name the object under test `sut` and the outcome `result` where that reads naturally.
- Async: `async Task` and `await Assert.ThrowsAsync<T>(...)`.
- Exceptions: `Assert.Throws<T>(() => ...)` and assert on the relevant property of the
  exception when the message or data matters.
- Assert with xUnit primitives directly (`Assert.Equal`, `Assert.True`, `Assert.Throws`,
  `Assert.All`). Do not wrap them in helper methods or custom assertion classes.
- No magic values without context: derive expected values in the Arrange block or name
  them; comment the arithmetic when it is not obvious (e.g. `// 47.11 * 2 (EUR->USD)`).
- Tests must be deterministic. No time, randomness or environment dependencies except
  through FsCheck generators.

## Template

```csharp
[Fact]
public void Add_WhenCurrenciesMatch_ReturnsSumInSameCurrency()
{
    // Arrange
    var left = new Money(47.11m, EUR);
    var right = new Money(23.42m, EUR);

    // Act
    var result = left + right;

    // Assert
    Assert.Equal(new Money(70.53m, EUR), result);
}
```

## Running

`dotnet test` runs in Microsoft.Testing.Platform mode (see `global.json`), so filters use
the xUnit options, not `--filter`. Filter within one project; a project with zero matching
tests fails the run with exit code 8.

Every run writes one TRX report per assembly to `TestResults/` (git-ignored, overwritten
each run). When a run fails, read the matching `.trx` for the test name and assertion
message before re-running.

```bash
dotnet test                                                                       # everything
dotnet test --project test/CurrenCSharp.Test --filter-class "*.MoneyTests"        # one class
dotnet test --project test/CurrenCSharp.Test --filter-method "*.MoneyTests.Distribute_*" # method pattern
dotnet test --project test/CurrenCSharp.Test -f net10.0                           # one framework
```
