# Test Plan

Diese Datei enthaelt den aktuellen Test-Backlog als Tabellen fuer Unit-, Property-Based-, Performance-, Concurrency- und Security-Tests.

Die Spalte `Typ` benennt den xUnit-Attribut-Typ:
- `Fact` — einzelner Testfall, keine Parameter
- `Theory` — parametrisiert via `[InlineData]` oder `[MemberData]`; mehrere Eingaben teilen sich die gleiche Assertion-Logik

## Unit-Tests

| Klassenname | Methodenname | Test-Methodenname | Typ | Testdaten |
|---|---|---|---|---|
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenCurrencyIsNull_ThrowsArgumentNullException` | Fact | `currency = null` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenScopesAreNested_RestoresPreviousScope` | Fact | `outer=EUR`, `inner=USD`, nach `inner.Dispose()` wieder `EUR` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenDisposedOutOfOrder_ThrowsInvalidOperationException` | Fact | `outer=EUR`, `inner=USD`, `outer.Dispose()` vor `inner.Dispose()` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenScopeDisposedTwice_DoesNotThrow` | Fact | `scope.Dispose()` zweimal |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenAwaitBoundaryIsCrossed_PreservesAsyncLocalScope` | Fact | innerhalb Scope `await Task.Yield()`, danach weiterhin gleiche Default-Currency |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenUsedInParallelTasks_KeepsTaskLocalDefaults` | Fact | 2 parallele Tasks: Task A nutzt `EUR`, Task B nutzt `USD` |
| `Currency` | `Default` | `Default_WhenScopeExists_ReturnsScopedCurrency` | Fact | Scope mit `EUR`, erwartetes Ergebnis `EUR` |
| `Currency` | `Default` | `Default_WhenNoScopeExists_ThrowsNoDefaultCurrencyException` | Fact | Zugriff ohne Default-Scope |
| `Currency` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenAlphaCodeNumericCodeAndMinorUnitsMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `Currency(EUR,978,2)`-Instanzen |
| `Currency` | `Equals(object?)` | `Equals_WhenAnyFieldDiffers_ReturnsFalse` | Theory | `(EUR,978,2)` vs `(EUR,978,3)`; `(EUR,...)` vs `(USD,...)` |
| `AlphaCode` | `AlphaCode(string)` | `Constructor_WhenValueIsInvalid_ThrowsArgumentException` | Theory | `null`, `""`, `" "`, `"EU"`, `"EURO"`, `"eur"`, `"E1R"`, `"ЕUR"(Cyrillic E)` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsAlphaCode` | Theory | `"EUR"`, `"USD"`, `"CHF"`, `"JPY"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | Theory | `"eur"`, `"EU1"`, `"EURO"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | Fact | `s = null` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | Theory | `"JPY"`, `"EUR"`, `"USD"` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | Theory | `"jpy"`, `"JP"`, `"JP¥"`, `""`, `null` |
| `AlphaCode` | implizite Konvertierung / `ToString()` | `Conversions_WhenRoundTripped_PreserveValue` | Theory | `string -> AlphaCode -> string` fuer `"CHF"`, `"EUR"`, `"USD"` |
| `AlphaCode` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `AlphaCode("EUR")`-Instanzen |
| `AlphaCode` | `Equals(object?)` | `Equals_WhenComparedWithDifferentType_ReturnsFalse` | Fact | `AlphaCode("EUR")` vs `"EUR"` (string) |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenValueContainsNonAsciiLetters_ThrowsArgumentException` | Theory | Security: `"Çur"`, `"Ëur"`, `"𝐀BC"` (math variant) |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenValueIsExcessivelyLong_ThrowsArgumentException` | Fact | DoS-Hardening: 10_000 Zeichen |
| `AlphaCode` | `Parse(string)` | `Parse_WhenInputContainsControlCharacters_ThrowsFormatException` | Theory | `"E\0R"`, `"EU\nR"`, `"E\tR"` |
| `NumericCode` | `NumericCode(int)` | `Constructor_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException` | Theory | `-1`, `1000`, `int.MinValue`, `int.MaxValue` |
| `NumericCode` | `NumericCode(int)` | `Constructor_WhenValueIsAtBoundary_Succeeds` | Theory | `0`, `999` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsNumericCode` | Theory | `"978"`, `"840"`, `"007"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | Theory | `"ABC"`, `"1000"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | Fact | `s = null` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsNegative_ThrowsFormatException` | Theory | `"-1"`, `"-978"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsNonCanonical_RejectsAmbiguousFormats` | Theory | Security: `" 978"`, `"+978"`, `"٩٧٨"` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | Theory | `"007"`, `"840"`, `"000"`, `"999"` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | Theory | `"-1"`, `"1000"`, `"ABC"`, `""`, `null` |
| `NumericCode` | `ToString()` / implizite Konvertierung | `ToString_WhenValueHasLeadingZeros_ReturnsThreeDigits` | Theory | `7 -> "007"`, `42 -> "042"`, `999 -> "999"` |
| `NumericCode` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `NumericCode(978)`-Instanzen |
| `Scale` | `Scale(byte)` | `Constructor_WhenValueGreaterThan28_ThrowsArgumentOutOfRangeException` | Theory | `29`, `100`, `byte.MaxValue` |
| `Scale` | `Scale(byte)` | `Constructor_WhenValueIsAtBoundary_Succeeds` | Theory | `0`, `1`, `27`, `28` |
| `Scale` | Konvertierungsoperatoren | `Conversions_WhenScaleIsValid_ReturnExpectedValues` | Theory | `Scale(4) -> int(4)`, `Scale(6) -> byte(6)`, `byte(10) -> Scale(10)` |
| `Scale` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `Scale(4)`-Instanzen |
| `Ratio` | `Ratio(decimal)` | `Constructor_WhenValueIsNegative_ThrowsArgumentOutOfRangeException` | Theory | `-0.01m`, `-1m`, `decimal.MinValue` |
| `Ratio` | `Ratio(decimal)` | `Constructor_WhenValueIsNonNegative_Succeeds` | Theory | `0m`, `1.5m`, `decimal.MaxValue` |
| `Ratio` | `CompareTo(Ratio?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | Fact | `Ratio(1m).CompareTo(null)` |
| `Ratio` | `CompareTo(Ratio?)` | `CompareTo_WhenOtherIsEqualOrLessOrGreater_ReturnsExpectedSign` | Theory | `(1m,1m,0)`, `(1m,2m,<0)`, `(2m,1m,>0)` |
| `Ratio` | Konvertierungsoperatoren | `Conversions_WhenRoundTripped_PreserveValue` | Theory | `decimal -> Ratio -> decimal` fuer `2.25m`, `0m`, `100m` |
| `Ratio` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `Ratio(1.5m)`-Instanzen |
| `ExchangeRate` | `ExchangeRate(decimal)` | `Constructor_WhenValueIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException` | Theory | `0m`, `-1m`, `decimal.MinValue` |
| `ExchangeRate` | `ExchangeRate(decimal)` | `Constructor_WhenValueIsPositive_Succeeds` | Theory | Boundary: `0.00000001m`, `1m`, `1.25m` |
| `ExchangeRate` | expliziter Konvertierungsoperator | `ExplicitDecimalConversion_WhenExchangeRateIsValid_ReturnsUnderlyingValue` | Theory | `1.25m`, `2m`, `0.5m` |
| `ExchangeRate` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `ExchangeRate(1.25m)`-Instanzen |
| `Money` | `Money(decimal, Currency)` | `Constructor_WhenCurrencyIsNull_ThrowsArgumentNullException` | Fact | `amount=1m`, `currency=null` |
| `Money` | `Currency` | `Currency_WhenMoneyIsDefault_ThrowsNoCurrencyException` | Fact | `default(Money)` |
| `Money` | `default(Money)` | `DefaultMoney_WhenAmountAccessed_ReturnsZero` | Fact | `default(Money).Amount == 0m` |
| `Money` | `Zero()` | `Zero_WhenNoDefaultCurrencyConfigured_ThrowsNoDefaultCurrencyException` | Fact | kein Default-Scope |
| `Money` | `Zero(Currency)` | `Zero_WhenCurrencyIsNull_ThrowsArgumentNullException` | Fact | `currency=null` |
| `Money` | `In(ExchangeRateContext)` | `In_WhenContextIsNull_ThrowsArgumentNullException` | Fact | `context=null` |
| `Money` | `In(ExchangeRateContext)` | `In_WhenMoneyHasNoCurrency_ThrowsNoCurrencyException` | Fact | `default(Money)` |
| `Money` | `IsZero` / `IsPositive` / `IsNegative` | `SignProperties_WhenAmountVaries_ReturnExpectedFlags` | Theory | Betraege `-1`, `0`, `1` |
| `Money` | `Abs()` | `Abs_WhenCalled_ReturnsPositiveMoney` | Theory | `-10.25`, `0`, `10.25` -> erwartet `|amount|` |
| `Money` | `Round()` | `Round_WhenCurrencyHasMinorUnits_RoundsToCurrencyScale` | Fact | `Currency(TST, minorUnits=2)`, `1.235 -> 1.24` |
| `Money` | `Round()` | `Round_WhenCurrencyHasZeroMinorUnits_RoundsToInteger` | Fact | `Currency(JPY, minorUnits=0)`, `1.5 -> 2` (ToEven) |
| `Money` | `Round(int, MidpointRounding)` | `Round_WhenScaleAndModeProvided_UsesProvidedRounding` | Theory | `(1.005,2,ToEven,1.00)`, `(1.005,2,AwayFromZero,1.01)` |
| `Money` | `Round(int, MidpointRounding)` | `Round_WhenDecimalsOutOfRange_ThrowsArgumentOutOfRangeException` | Theory | `decimals=-1`, `decimals=29` |
| `Money` | `Min(Money, Money)` | `Min_WhenCurrenciesMatch_ReturnsSmallerAmount` | Theory | `(10 EUR,5 EUR,5 EUR)`, `(5 EUR,5 EUR,left)` |
| `Money` | `Min(Money, Money)` | `Min_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR`, `10 USD` |
| `Money` | `Max(Money, Money)` | `Max_WhenCurrenciesMatch_ReturnsGreaterAmount` | Theory | `(10 EUR,5 EUR,10 EUR)`, `(5 EUR,5 EUR,left)` |
| `Money` | `Max(Money, Money)` | `Max_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR`, `10 USD` |
| `Money` | `Distribute(int)` | `Distribute_WhenCountIsLessThanOrEqualZero_ThrowsArgumentOutOfRangeException` | Theory | `count=0`, `count=-1`, `count=int.MinValue` |
| `Money` | `Distribute(int)` | `Distribute_WhenCountIsValid_PreservesSumAndCount` | Fact | `47.11 EUR`, `count=3`, erwartet `[15.71,15.70,15.70]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatiosAreNullOrEmpty_ThrowsArgumentException` | Theory | `ratios=null`, `ratios=[]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatioSumIsZero_ThrowsArgumentException` | Fact | `ratios=[0,0,0]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatiosHaveTie_DistributesRemainderByIndex` | Fact | `0.02 EUR`, `ratios=[1,1,1]`, erwartet `[0.01,0.01,0.00]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenAmountIsNegative_DistributesNegativeRemainderByIndex` | Fact | `-0.02 EUR`, `ratios=[1,1,1]`, erwartet `[-0.01,-0.01,0.00]` |
| `Money` | `operator +(Money, Money)` | `Addition_WhenCurrenciesMatch_ReturnsSum` | Theory | `(10 EUR,5 EUR,15 EUR)`, `(0 EUR,0 EUR,0 EUR)`, `(10 EUR,-5 EUR,5 EUR)` |
| `Money` | `operator +(Money, Money)` | `Addition_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR + 5 USD` |
| `Money` | `operator -(Money, Money)` | `Subtraction_WhenCurrenciesMatch_ReturnsDifference` | Theory | `(10 EUR,5 EUR,5 EUR)`, `(0 EUR,0 EUR,0 EUR)`, `(5 EUR,10 EUR,-5 EUR)` |
| `Money` | `operator -(Money, Money)` | `Subtraction_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR - 5 USD` |
| `Money` | `operator +(Money)` (unary) | `UnaryPlus_WhenCalled_ReturnsSameAmount` | Theory | `+(10 EUR)=10 EUR`, `+(0 EUR)=0 EUR`, `+(-5 EUR)=-5 EUR` |
| `Money` | `operator -(Money)` (unary) | `UnaryNegation_WhenCalled_ReturnsNegatedAmount` | Theory | `-(10 EUR)=-10 EUR`, `-(0 EUR)=0 EUR`, `-(-5 EUR)=5 EUR` |
| `Money` | `operator *(Money, decimal)` | `Multiplication_WhenFactorIsScalar_ReturnsScaledMoney` | Theory | `(10 EUR,2m,20 EUR)`, `(10 EUR,0m,0 EUR)`, `(10 EUR,-1m,-10 EUR)`, `(10 EUR,1m,10 EUR)` |
| `Money` | `operator *(decimal, Money)` | `Multiplication_WhenOrderIsReversed_ReturnsSameResult` | Theory | `(2m,10 EUR,20 EUR)`, `(0m,10 EUR,0 EUR)` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenCurrenciesMatch_ReturnsRatio` | Theory | `(10 EUR,5 EUR,2m)`, `(10 EUR,4 EUR,2.5m)` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR / 5 USD` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenDivisorIsZero_ThrowsDivideByZeroException` | Fact | `10 EUR / 0 EUR` |
| `Money` | `operator /(Money, decimal)` | `DivisionByDecimal_WhenDivisorIsScalar_ReturnsScaledMoney` | Theory | `(10 EUR,2m,5 EUR)`, `(10 EUR,4m,2.5 EUR)` |
| `Money` | `operator /(Money, decimal)` | `DivisionByDecimal_WhenDivisorIsZero_ThrowsDivideByZeroException` | Fact | `10 EUR / 0m` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenCurrenciesMatch_ReturnsExpectedSign` | Theory | `(5 EUR,10 EUR,<0)`, `(10 EUR,10 EUR,0)`, `(10 EUR,5 EUR,>0)` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | Fact | `10 EUR`, `10 USD` |
| `Money` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | Fact | `other=null` |
| `Money` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenCurrenciesDiffer_ConvertsBeforeComparing` | Fact | `100 USD.CompareTo((100 EUR).In(ctx))` mit Rate `EUR->USD=1.1` |
| `Money` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | Fact | `other=null` |
| `Money` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenCurrenciesDiffer_UsesTotalInOwnCurrency` | Fact | `100 USD` vs `Wallet[50 EUR,50 USD].In(ctx)` |
| `Money` | Cross-Type `==`/`!=`/`<`/`<=`/`>`/`>=` mit `ContextedMoney` | `CrossTypeComparisonOperators_WithContextedMoney_ReturnExpectedResult` | Theory | Operatoren-Matrix mit gleichen/abweichenden Betraegen, beide Reihenfolgen |
| `Money` | Cross-Type `==`/`!=`/`<`/`<=`/`>`/`>=` mit `ContextedWallet` | `CrossTypeComparisonOperators_WithContextedWallet_ReturnExpectedResult` | Theory | Operatoren-Matrix mit Wallet-Total groesser/kleiner/gleich, beide Reihenfolgen |
| `Money` | `Equals(Money)` / `GetHashCode()` | `Equals_WhenAmountAndCurrencyMatch_ReturnsTrueAndSameHashCode` | Fact | zwei `Money(10m, EUR)`-Werte |
| `Money` | `Equals(Money)` | `Equals_WhenAnyFieldDiffers_ReturnsFalse` | Theory | `(10 EUR,11 EUR)`, `(10 EUR,10 USD)` |
| `Money` | `ToString()` | `ToString_WhenCalled_UsesAlphaCodeAndMinorUnits` | Theory | `(12.34,EUR,en-US)`, `(1.2345,TST3,en-US)`, `(1234,JPY,en-US)`, `(-12.34,EUR,en-US)` |
| `Wallet` | `Empty` | `Empty_WhenAccessed_ReturnsWalletWithoutEntries` | Fact | `Wallet.Empty`, keine Enumerierung |
| `Wallet` | `Of(params Money[])` / `Of(IReadOnlyCollection<Money>)` | `Of_WhenMoneysIsNull_ThrowsArgumentNullException` | Theory | beide Overloads mit `moneys=null` |
| `Wallet` | `Of(params Money[])` / `Of(IReadOnlyCollection<Money>)` | `Of_WhenInputIsEmpty_ReturnsEmptyWallet` | Theory | `Wallet.Of()`, `Wallet.Of(new List<Money>())` |
| `Wallet` | `Of(...)` | `Of_WhenSameCurrencyAppearsMultipleTimes_AggregatesAmounts` | Fact | `[1 EUR,2 EUR,3 EUR]` -> `6 EUR` |
| `Wallet` | `Of(...)` | `Of_WhenAmountsCancelOut_ReturnsEmptyWallet` | Fact | `[10 EUR,-10 EUR]` -> empty |
| `Wallet` | `operator +(Wallet)` (unary) | `UnaryPlus_WhenCalled_ReturnsSameWallet` | Fact | `+wallet` identisch zu `wallet` |
| `Wallet` | `operator -(Wallet)` (unary) | `UnaryNegation_WhenCalled_NegatesAllBuckets` | Fact | `[10 EUR,-5 USD]` -> `[-10 EUR,5 USD]` |
| `Wallet` | unaere Operatoren `+`/`-` | `UnaryOperators_WhenWalletIsNull_ThrowArgumentNullException` | Theory | `+(Wallet)null`, `-(Wallet)null` |
| `Wallet` | `operator +(Wallet, Wallet)` | `Addition_WhenWalletsContainSameCurrencies_AggregatesPerCurrency` | Fact | `[10 EUR,2 USD] + [5 EUR,3 USD]` -> `[15 EUR,5 USD]` |
| `Wallet` | `operator +(Wallet, Money)` | `Addition_WhenMoneyCurrencyExists_UpdatesBucketAmount` | Fact | `wallet=[1 EUR,2 USD]`, `money=3 EUR` -> `EUR=4` |
| `Wallet` | `operator -(Wallet, Wallet)` | `Subtraction_WhenWalletsContainSameCurrencies_SubtractsPerCurrency` | Fact | `[10 EUR,5 USD] - [3 EUR,2 USD]` -> `[7 EUR,3 USD]` |
| `Wallet` | `operator -(Wallet, Money)` | `Subtraction_WhenResultBecomesZero_RemovesBucket` | Fact | `wallet=[1 EUR]`, `money=1 EUR` -> `empty` |
| `Wallet` | binaere Operatoren `+`/`-` | `BinaryOperators_WhenEitherOperandIsNull_ThrowArgumentNullException` | Theory | `(null,wallet,+)`, `(wallet,null,+)`, `(null,wallet,-)`, `(wallet,null,-)`, `(null,money,+)`, `(null,money,-)` |
| `Wallet` | `operator *(Wallet, decimal)` / `operator *(decimal, Wallet)` | `Multiplication_WhenFactorIsScalar_ScalesAllBuckets` | Theory | `(wallet,2m)`, `(2m,wallet)`, `(wallet,0m -> empty)`, `(wallet,1m -> same)` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenDivisorIsScalar_ScalesAllBuckets` | Theory | `(wallet,2m)`, `(wallet,1m)`, `(wallet,-1m)` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenDivisorIsZero_ThrowsDivideByZeroException` | Fact | `wallet=[1 EUR]`, `divisor=0m` |
| `Wallet` | skalare Operatoren `*`/`/` | `ScalarOperators_WhenWalletIsNull_ThrowArgumentNullException` | Theory | `null * 2m`, `2m * null`, `null / 2m` |
| `Wallet` | `GetEnumerator()` | `GetEnumerator_WhenWalletHasEntries_YieldsAllMoneys` | Fact | `wallet=[1 EUR,2 USD]` -> 2 Elemente |
| `Wallet` | `GetEnumerator()` | `GetEnumerator_WhenWalletIsEmpty_YieldsNothing` | Fact | `Wallet.Empty` -> 0 Elemente |
| `Wallet` | `IEnumerable.GetEnumerator()` (nicht-generisch) | `NonGenericEnumerator_WhenIterated_ReturnsSameResultsAsGenericEnumerator` | Fact | `((IEnumerable)wallet).GetEnumerator()` |
| `Wallet` | `In(ExchangeRateContext)` | `In_WhenContextIsNull_ThrowsArgumentNullException` | Fact | `context=null` |
| `Wallet` | `ToBuilder()` | `ToBuilder_WhenWalletIsMutatedThroughBuilder_DoesNotMutateOriginalWallet` | Fact | Original-Wallet mit Builder aendern, Original bleibt unveraendert |
| `Wallet` | `Equals(Wallet)` / `GetHashCode()` | `Equals_WhenInsertionOrderDiffers_ReturnsTrueAndSameHashCode` | Fact | `Wallet.Of(1 EUR,2 USD)` vs `Wallet.Of(2 USD,1 EUR)` |
| `Wallet` | `Equals(Wallet?)` / `Equals(object?)` | `Equals_WhenComparedWithNullOrDifferentType_ReturnsFalse` | Theory | `(Wallet?)null`, `(object?)null`, `"some string"` |
| `Wallet` | `CompareTo(ContextedMoney?)` / `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | Theory | `other=(ContextedMoney?)null`, `other=(ContextedWallet?)null` |
| `Wallet` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenWalletTotalEqualsContextedMoney_ReturnsZero` | Fact | `wallet=[50 EUR,50 USD]`, `ctx EUR->USD=1`, `other=100 USD` |
| `Wallet` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenBothTotalsConvertedToSameCurrency_ComparesTotals` | Fact | `left=[10 EUR].In(ctx)`, `right=[9 EUR,1 USD].In(ctx)` |
| `Wallet` | Cross-Type `==`/`!=` mit `ContextedMoney`/`ContextedWallet` | `EqualityOperators_WithContextTypes_ReturnExpectedResult` | Theory | Matrix aus Typen und Reihenfolgen mit gleichen/unterschiedlichen Totals |
| `Wallet` | Cross-Type `<`/`<=`/`>`/`>=` mit `ContextedMoney`/`ContextedWallet` | `OrderOperators_WithContextTypes_ReturnExpectedResult` | Theory | Matrix: Totals groesser/kleiner/gleich, beide Reihenfolgen |
| `Wallet` | Cross-Type-Operatoren mit `ContextedMoney`/`ContextedWallet` | `ComparisonOperators_WhenAnyReferenceOperandIsNull_ReturnExpectedResult` | Theory | `wallet=null`, `contextedMoney=null`, `contextedWallet=null` in allen Operatoren |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenSingleCurrencyWallet_ReturnsThatCurrency` | Fact | `wallet=[1 USD,2 USD]` -> `USD` |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenMultiCurrencyWallet_UsesDefaultCurrency` | Fact | `wallet=[1 EUR,1 USD]`, Default=`EUR` |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException` | Fact | `wallet=[1 EUR,1 USD]`, kein Default-Scope |
| `Wallet.Builder` | `Add(Money)` | `Add_WhenCurrencyExists_AggregatesAmount` | Fact | Start: `EUR=1`, add `EUR=2` -> `EUR=3` |
| `Wallet.Builder` | `Add(Money)` | `Add_WhenCurrencyIsNew_CreatesBucket` | Fact | Start: `EUR=1`, add `USD=2` -> `EUR=1,USD=2` |
| `Wallet.Builder` | `AddRange(IEnumerable<Money>)` | `AddRange_WhenMoneysProvided_AggregatesPerCurrency` | Fact | `[EUR1,USD2,EUR3]` -> `EUR4,USD2` |
| `Wallet.Builder` | `AddRange(IEnumerable<Money>)` | `AddRange_WhenMoneysIsNull_ThrowsArgumentNullException` | Fact | `moneys=null` |
| `Wallet.Builder` | `this[Currency] set` | `IndexerSet_WhenMoneyCurrencyDiffers_ThrowsDifferentCurrencyException` | Fact | `key=EUR`, `value=1 USD` |
| `Wallet.Builder` | `this[Currency] set` | `IndexerSet_WhenKeyIsNull_ThrowsArgumentNullException` | Fact | `key=null`, `value=1 EUR` |
| `Wallet.Builder` | `this[Currency] get` | `IndexerGet_WhenKeyMissing_ThrowsKeyNotFoundException` | Fact | `missingKey=JPY` |
| `Wallet.Builder` | `Remove(Currency)` | `Remove_WhenKeyExistsOrMissing_ReturnsExpectedResult` | Theory | `(keyExists,true)`, `(keyMissing,false)` |
| `Wallet.Builder` | `Clear()` | `Clear_WhenCalled_RemovesAllEntries` | Fact | vor: `[EUR1,USD2]`, nach `Clear()`: Count=0 |
| `Wallet.Builder` | `TryGetValue(Currency, out Money)` | `TryGetValue_WhenKeyExistsOrMissing_ReturnsExpectedResult` | Theory | `(EUR,true,value)`, `(JPY,false,default)` |
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderContainsEntries_ReturnsEquivalentWallet` | Fact | Builder-Inhalt gegen `Wallet.Of(...)` vergleichen |
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderChangesAfterCreation_DoesNotMutateReturnedWallet` | Fact | Snapshot-Test: `wallet1=ToWallet()`, danach Builder mutieren |
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderIsEmpty_ReturnsEmptyWallet` | Fact | keine Entries |
| `Wallet.Builder` | `GetEnumerator()` | `GetEnumerator_WhenBuilderHasEntries_YieldsAllKeyValuePairs` | Fact | mehrere Currencies, Rueckgabe als `KeyValuePair<Currency,Money>` |
| `Wallet.Builder` | `Count` | `Count_WhenItemsAddedAndRemoved_ReflectsCurrentSize` | Fact | Start=0, add 3, remove 1 -> 2 |
| `Wallet.Builder` | `Keys` / `Values` | `KeysAndValues_WhenEnumerated_ReturnCurrentBuilderState` | Fact | nach mehreren Add/Remove-Ops |
| `Wallet.Builder` | `ContainsKey(Currency)` | `ContainsKey_WhenKeyExistsOrMissing_ReturnsExpectedResult` | Theory | `(EUR,true)`, `(JPY,false)` |
| `ExchangeRateContext` | Constructor | `Constructor_WhenRequiredArgumentIsNull_ThrowsArgumentNullException` | Theory | `baseCurrency=null`, `exchangeRates=null` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenEitherCurrencyIsNull_ThrowsArgumentNullException` | Theory | `(null,USD)`, `(USD,null)` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenSourceAndTargetCurrencyMatch_ReturnsOne` | Theory | `EUR->EUR`, `USD->USD`, `JPY->JPY` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenFromIsBase_ReturnsDirectRate` | Fact | Base=`EUR`, Rates: `USD=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenToIsBase_ReturnsInverseRate` | Fact | `USD->EUR` erwartet `0.5` bei `EUR->USD=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenNeitherIsBase_ReturnsCrossRate` | Fact | Base=`EUR`, `USD=2`, `JPY=4`, `USD->JPY=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenPairMissing_ThrowsInvalidOperationException` | Fact | Nur `USD` vorhanden, Anfrage `EUR->JPY` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenCalledConcurrently_ReturnsConsistentValues` | Fact | 1000 parallele Reads auf direkt/invers/cross |
| `ExchangeRateContext` | `GetEnumerator()` | `GetEnumerator_WhenRatesExist_ReturnsAllConfiguredRates` | Fact | Rates: `USD`, `JPY`, erwartete Anzahl `2` |
| `ExchangeRateContext` | `GetEnumerator()` | `GetEnumerator_WhenEnumeratedMultipleTimes_ReturnsConsistentResults` | Fact | zweimaliger `foreach`-Durchlauf |
| `ExchangeRateContext` | Properties | `Properties_WhenAccessed_ReturnsConstructorValues` | Fact | Ctor-Werte unveraendert gespiegelt |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenTargetCurrencyIsNull_ThrowsArgumentNullException` | Fact | `to=null` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRateIsMissing_ThrowsInvalidOperationException` | Fact | Context ohne benoetigte Rate |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenSourceEqualsTarget_ReturnsSameAmountAndCurrency` | Fact | `12.34 USD` nach `USD` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenAmountIsZero_ReturnsZeroInTargetCurrency` | Fact | `0 EUR -> USD = 0 USD` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRoundResultIsFalse_DoesNotRound` | Fact | `EUR->JPY rate=1.5`, `amount=1`, `RoundResult=false` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenScaleProvided_UsesScaleAndMode` | Theory | `(rate=1.234567,scale=4,ToEven,1.2346)`, `(rate=1.234567,scale=4,AwayFromZero,1.2346)` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenOptionsAreNull_UsesDefaultRoundingByTargetMinorUnits` | Fact | `to.MinorUnits=2`, `amount=1.005` |
| `ContextedMoney` | `ToString()` | `ToString_WhenCalled_DelegatesToUnderlyingMoney` | Fact | `12.34 EUR` |
| `ContextedMoney` | `Amount` / `Currency` Properties | `Properties_WhenAccessed_DelegateToUnderlyingMoney` | Fact | `(12.34 EUR).In(ctx)` -> Amount/Currency |
| `ContextedMoney` | `Equals`/`GetHashCode` (record) | `Equals_WhenSameMoneyAndContext_ReturnsTrueAndSameHashCode` | Fact | zwei identische `ContextedMoney`-Instanzen |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenCurrencyIsNull_ThrowsArgumentNullException` | Fact | `currency=null` |
| `ContextedWallet` | `Total()` | `Total_WhenSingleCurrencyWallet_ReturnsTotalInSingleCurrency` | Fact | `wallet=[1 USD,2 USD]` -> `3 USD` |
| `ContextedWallet` | `Total()` | `Total_WhenMultiCurrencyWallet_UsesResolvedDefaultCurrency` | Fact | `wallet=[1 EUR,1 USD]`, `EUR->USD=2`, Ergebnis `1.50 EUR` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenRoundResultIsFalse_ReturnsUnroundedTotal` | Fact | `wallet=[1 EUR,1 USD]`, `RoundResult=false` -> `1 + 1/2` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenScaleAndModeProvided_UsesFinalRoundingOptions` | Fact | gemischte Wallet, `scale=4`, `AwayFromZero` |
| `ContextedWallet` | `Total()` | `Total_WhenWalletIsMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException` | Fact | `wallet=[1 EUR,1 USD]`, kein Default-Scope |
| `ContextedWallet` | `Total()` | `Total_WhenWalletIsEmpty_ReturnsZeroInResolvedCurrency` | Fact | `Wallet.Empty` mit Default=`EUR` |
| `ContextedWallet` | `Wallet` / `Context` Properties | `Properties_WhenAccessed_ReturnOriginalBoundInstances` | Fact | Identitaetspruefung der injizierten Instanzen |
| `ConversionOptions` | `Default` | `Default_WhenAccessed_ReturnsExpectedDefaultValues` | Fact | `RoundResult=true`, `RoundingMode=ToEven`, `Scale=null` |
| `ConversionOptions` | record `Equals` / `GetHashCode` | `Equals_WhenAllFieldsMatch_ReturnsTrueAndSameHashCode` | Fact | zwei identische Konfigurationen |
| `ConversionOptions` | record `Equals` | `Equals_WhenAnyFieldDiffers_ReturnsFalse` | Theory | unterschiedliches `RoundResult`, `RoundingMode`, `Scale` |
| `ConversionOptions` | `with`-Expression | `WithExpression_WhenFieldChanged_ReturnsModifiedCopy` | Fact | `Default with { RoundResult = false }` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeExists_ReturnsCurrency` | Theory | `"EUR"`, `"USD"`, `"JPY"`, `"CHF"` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | Theory | `"ZZZ"`, `"XXX"` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeIsNull_ThrowsArgumentNullException` | Fact | `alphaCode=null` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeExists_ReturnsCurrency` | Theory | `978`, `840`, `392` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | Theory | `001`, `002` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeIsNull_ThrowsArgumentNullException` | Fact | `numericCode=null` |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_HasUniqueAlphaAndNumericCodes` | Fact | Reflection ueber alle `Currency`-Felder |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_ContainsExpectedMinorUnitsSamples` | Theory | `(JPY,0)`, `(EUR,2)`, `(KWD,3)` |
| `Iso4217` | statischer Katalog | `Catalog_WhenInspected_AllCurrenciesAreNonNull` | Fact | Reflection: kein `Currency`-Feld ist `null` |
| `Iso4217` | `FindByAlphaCode` | `FindByAlphaCode_WhenRoundTrippedThroughNumericLookup_ReturnsSameCurrencyInstance` | Fact | `EUR` via Alpha, anschliessend numerisch `978` -> gleiche Referenz |

## Property-Based-Tests (FsCheck)

Alle Property-Tests sind inhaerent parametrisiert ueber FsCheck-Generatoren (`[Property]` / `[FsCheckTheory]`), daher ist die Typ-Spalte nicht noetig.

| Klassenname | Methodenname | Test-Methodenname | Testdaten |
|---|---|---|---|
| `Money` | `Distribute(int)` | `Distribute_WhenCalled_PreservesTotalCurrencyAndCount_Property` | Generator: `amountCents [-1_000_000..1_000_000]`, `count [1..100]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenCalled_DeviationIsAtMostOneMinorUnit_Property` | Generator: Ratios `0..100`, Laenge `1..20`, Summe `>0` |
| `ExchangeRateContext` | `GetExchangeRate(Currency,Currency)` | `GetExchangeRate_WhenRatesAreValid_IsInverseAndCrossConsistent_Property` | Generator: positive Raten `0.0001..5` (skaliert) |
| `Wallet` | `Of(...)` / Aggregation | `Of_WhenInputOrderChanges_RemainsEqual_Property` | Zufaellige Money-Liste + zufaellige Permutation |
| `ContextedMoney` | `Convert(Currency,ConversionOptions?)` | `Convert_WhenRoundResultIsFalse_RoundTripAtoBtoA_IsWithinTolerance_Property` | Zufaellige Betraege + positive Raten + Toleranz `1e-18` |
| `Money` | `operator +` | `Addition_WhenSameCurrency_IsCommutative_Property` | `leftCents/rightCents [-1_000_000..1_000_000]` |
| `Money` | `operator +` | `Addition_WhenSameCurrency_IsAssociative_Property` | Tripel `a/b/c` aus `[-1_000_000..1_000_000]` |
| `Money` | `operator +` / unary `-` | `AdditionWithNegation_WhenSameCurrency_ReturnsZero_Property` | `amountCents [-1_000_000..1_000_000]` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenSameCurrency_IsAntisymmetricAndTransitive_Property` | Tripel `left/mid/right` aus Bereich `[-1_000_000..1_000_000]` |
| `Wallet` | `operator +` / unary `-` | `WalletPlusNegatedWallet_WhenComputed_CancelsToEmptyWallet_Property` | Zufaellige Liste von EUR-Betraegen |
| `AlphaCode` | `Parse` / `ToString` | `ParseToString_WhenCodeIsValid_RoundTrips_Property` | Generator fuer 3 Grossbuchstaben `A-Z` |
| `NumericCode` | `Parse` / `ToString` | `ParseToString_WhenCodeIsValid_RoundTrips_Property` | Generator fuer `000..999` |
| `Wallet.Builder` | `Add/Remove/Clear/ToWallet` | `Builder_WhenRandomOperationsApplied_MatchesReferenceModel_Property` | Zufaellige Ops-Sequenz Laenge `1..200` gegen Dictionary-Modell |
| `Money` | `operator -(Money, Money)` | `Subtraction_AddingBackRightOperand_EqualsLeftOperand_Property` | `(a - b) + b == a` fuer gleiche Waehrung |
| `Money` | `operator *(Money, decimal)` | `Multiplication_ByOne_IsIdentity_Property` | `money * 1m == money` |
| `Money` | `operator *(Money, decimal)` | `Multiplication_ByMinusOne_EqualsNegation_Property` | `money * -1m == -money` |
| `Money` | `Abs()` | `Abs_WhenAppliedTwice_IsIdempotent_Property` | `money.Abs().Abs() == money.Abs()` |
| `Money` | `Round` / `Round(int, MidpointRounding)` | `Round_WhenAppliedTwiceWithSameArgs_IsIdempotent_Property` | `m.Round(n,mode).Round(n,mode) == m.Round(n,mode)` |
| `Wallet` | `operator +` / unary `-` | `WalletPlusOwnNegation_AlwaysEqualsEmpty_Property` | Zufaellige Multi-Currency-Wallet |
| `Wallet` | `operator *` / `operator /` | `Multiplication_DivisionByNonZeroScalar_RoundTrips_Property` | `(w * k) / k == w` fuer `k != 0` (Toleranz fuer Rundung) |
| `ExchangeRate` | Inverse | `ExchangeRate_InverseOfInverse_EqualsOriginal_Property` | `1 / (1 / rate)` innerhalb Toleranz |

## Performance-Tests (BenchmarkDotNet)

BenchmarkDotNet nutzt eigene Attribute (`[Benchmark]`, `[Params]`), daher ist die Typ-Spalte nicht noetig. Parameter-Permutationen werden ueber `[Params]` abgebildet.

| Klassenname | Methodenname | Test-Methodenname | Testdaten |
|---|---|---|---|
| `Wallet` | `Of(IReadOnlyCollection<Money>)` | `CreateWalletFromCollection_Benchmark` | `MoneyCount = 1_000, 10_000, 100_000, 1_000_000` |
| `Wallet` | `operator +(Wallet, Money)` iterativ | `CreateWalletByIterativeAdd_Benchmark` | `MoneyCount = 1_000, 10_000, 100_000, 1_000_000` |
| `Wallet.Builder` | `Add(Money)` + `ToWallet()` | `CreateWalletByBuilder_Benchmark` | `MoneyCount = 1_000, 10_000, 100_000, 1_000_000` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WithMixedCurrencies_Benchmark` | `WalletSize = 10, 1_000, 100_000`; `CurrencyCount = 1, 5, 20` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_DirectInverseCross_WithAndWithoutRounding_Benchmark` | `Mode=direct/inverse/cross`; `RoundResult=true/false`; `Scale=null/2/4` |
| `ExchangeRateContext` | `GetExchangeRate(Currency,Currency)` | `GetExchangeRate_DirectInverseCross_Benchmark` | `RateCount = 10, 100, 1_000`; Modi: direkt/invers/cross |
| `Money` | `Distribute(int)` | `DistributeByCount_Benchmark` | `count = 2, 3, 10, 100, 1_000`; Betraege klein/gross/negativ |
| `Money` | `Distribute(params Ratio[])` | `DistributeByRatios_Benchmark` | `ratioLength = 2, 3, 10, 100`; dichte vs. sparse Ratios |
| `Iso4217` | `FindByAlphaCode` / `FindByNumericCode` | `LookupByAlphaAndNumeric_WarmCold_Benchmark` | `N = 1_000_000` Lookups, warm cache vs. erster Zugriff |
| `Iso4217` | `FindByAlphaCode` / `FindByNumericCode` parallel | `LookupByAlphaAndNumeric_Parallel_Benchmark` | `Parallelism = 2, 4, 8, 16`; gemischte Alpha/Numeric Lookups |
| `Money` | `operator +(Money,Money)` / `operator *(Money,decimal)` | `MoneyArithmetic_Benchmark` | `OperationCount = 100, 10_000, 1_000_000`; gleiche Waehrung |
| `Wallet` | `Equals(Wallet)` / `GetHashCode()` | `WalletEquality_Benchmark` | `WalletSize = 10, 1_000, 100_000`; identische vs. abweichende Wallets |
| `AlphaCode` / `NumericCode` | `Parse(string)` | `CodeParsing_Benchmark` | `ParseCount = 1_000_000`; gueltige und ungueltige Eingaben |

## Thread-Safety- / Concurrency-Tests

Concurrency-Tests sind jeweils Einzelszenarien mit festem Orchestrierungs-Setup (`[Fact]`), daher keine Typ-Spalte.

| Klassenname | Methodenname | Test-Methodenname | Testdaten |
|---|---|---|---|
| `CurrenC` | `UseDefaultCurrency(Currency)` | `DefaultCurrency_WhenUsedAcrossAsyncAwaitBoundaries_RemainsIsolatedPerFlow` | 50 parallele Tasks, jede mit eigener Default-Waehrung, zufaelliges `await Task.Yield()` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `DefaultCurrency_WhenScopeIsDisposedInOtherThread_DoesNotAffectOtherAsyncLocalStacks` | Scope in Task A, Dispose in Task B |
| `Iso4217Cache` | Lazy-Initialisierung | `Iso4217Cache_WhenAccessedConcurrentlyOnColdStart_InitializesExactlyOnce` | Process-neuer Cold-Start, 32 Threads rufen gleichzeitig `FindByAlphaCode` auf; Verifikation ueber Zaehler-Sentinel in Reflection |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenReadConcurrentlyOnSharedInstance_IsRaceFree` | 1 Instanz, 16 Threads, Mischung aus direkt/invers/cross Lookups |
| `Wallet` | Immutability | `Wallet_WhenSharedReadOnlyAcrossThreads_IsSafeForParallelEnumeration` | eine Wallet-Instanz, 16 Threads iterieren parallel, kein Locking noetig |
| `Wallet.Builder` | Mutation | `Builder_WhenMutatedConcurrently_IsDocumentedAsNotThreadSafe` | Negativ-Test: parallele `Add`-Aufrufe in 8 Threads, belegt dokumentierte Nicht-Thread-Safety |

## Security- / Input-Validation-Tests

| Klassenname | Methodenname | Test-Methodenname | Typ | Testdaten |
|---|---|---|---|---|
| `AlphaCode` | `Parse(string)` / `Constructor(string)` | `Parse_WhenInputContainsUnicodeHomoglyphs_Rejected` | Theory | kyrillisches `"ЕUR"`, griechisches `"ΕUR"`, Fullwidth `"ＥＵＲ"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenInputContainsZeroWidthCharacters_Rejected` | Theory | `"E\u200BUR"`, `"EUR\uFEFF"`, `"\u200BEUR"` |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenInputIsExtremelyLarge_DoesNotHang` | Fact | DoS-Resistenz: `new string('A', 1_000_000)` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputContainsNonAsciiDigits_Rejected` | Theory | Arabic-Indic `"٩٧٨"`, Thai `"๙๗๘"`, Devanagari `"९७८"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputContainsSignOrWhitespace_Rejected` | Theory | `"+978"`, `" 978"`, `"978 "`, `"\t978"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsExtremelyLarge_DoesNotHang` | Fact | DoS: `new string('9', 1_000_000)` |
| `Currency` | Hash-Verhalten | `GetHashCode_WhenManyDistinctCurrenciesHashed_HasNegligibleCollisionRate` | Fact | alle Iso4217-Waehrungen, Kollisionsrate `< 2 %` |
| `Wallet` | `Equals` / `GetHashCode` | `Equals_WhenContentsAreSameButConstructedDifferently_RemainConstantTime` | Fact | Timing-Test: gleiche und ungleiche Wallets, `stddev` innerhalb Toleranz |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenRateIsVerySmallOrLarge_DoesNotOverflowOrHang` | Theory | Raten `decimal.MaxValue / 2`, `1e-20m`, Cross-Rate-Berechnung |
