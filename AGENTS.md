# AGENTS.md

.NET library for monetary values: `Money`, `Wallet`, and their context-bound counterparts
`ContextedMoney` / `ContextedWallet` for conversion and cross-currency comparison via a
user-supplied `IExchangeRateProvider`. Packages: `CurrenCSharp`, `CurrenCSharp.Currencies`
(ISO 4217). `example/CurrenCSharp.Example/Program.cs` mirrors the README and is the API reference.

## Build & test

```bash
dotnet build                                                          # net8.0/9.0/10.0, warnings are errors
dotnet test                                                           # all projects, all frameworks
dotnet test --project test/CurrenCSharp.Test --filter-class "*.MoneyTests"   # xUnit filter options, not --filter
```

`dotnet test` runs in Microsoft.Testing.Platform mode (`global.json`, .NET 10 SDK). A filter
that matches nothing in a project fails with exit code 8, so filter with `--project`.

## Conventions

- C# 14, nullable. Types are immutable (`Money` is a `readonly record struct`, the rest `sealed`).
- One concern per partial file (`Money.Arithmetic.cs`, `Money.Comparison.cs`, `Wallet.Builder.cs`).
- XML docs on every public member. Package versions only in `Directory.Packages.props`.
- Tests: read [`docs/testing.md`](docs/testing.md) first.

## Domain rules

- `==`, `!=`, `<`, `>`, `<=`, `>=`, `Equals`, `CompareTo`, `GetHashCode` must be mutually
  consistent and must not throw. Sole exception: `DifferentCurrencyException` for operands
  with different currencies and no bound context.
- `default(Money)` is invalid by design (`NoCurrencyException`); use `Money.Zero(...)`.
- Cross-currency operations require `.In(context)`.
- `Money.Distribute` preserves total, count and currency; no minor unit is lost or invented.
- `CurrenC.UseDefaultCurrency` is async-scoped and restored LIFO; no global mutable defaults.

## Commits & releases

- Conventional Commits 1.0.0; imperative, ≤ 72 chars, no trailing period.
  Scopes in use: `money`, `wallet`, `currency`, `currencies`, `example`, `deps`, `release`, `agents`.
- Breaking changes: `!` plus `BREAKING CHANGE:` footer. Versions come from tags `v*.*.*` (MinVer).
- Do not commit, tag or push unless asked.

## Done means

Build without warnings, all tests green on every framework, new behavior tested (incl. edge
and negative cases), README and example updated on public API changes, no unrelated changes.
Run `.claude/agents/code-reviewer` on the diff before asking for a commit.
