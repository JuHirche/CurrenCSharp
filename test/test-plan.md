# Test Plan

Diese Datei enthaelt den aktuellen Test-Backlog als Tabelle fuer Unit-, Property-Based- und Performance-Tests.

## Unit-Tests

| Klassenname, die getestet werden soll | Methodenname, die getestet werden soll | Methodennamen fuer den Test | Testdaten |
|---|---|---|---|
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenCurrencyIsNull_ThrowsArgumentNullException` | `currency = null` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenScopesAreNested_RestoresPreviousScope` | `outer=EUR`, `inner=USD`, nach `inner.Dispose()` wieder `EUR` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenDisposedOutOfOrder_ThrowsInvalidOperationException` | `outer=EUR`, `inner=USD`, `outer.Dispose()` vor `inner.Dispose()` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenScopeDisposedTwice_DoesNotThrow` | `scope.Dispose()` zweimal |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenAwaitBoundaryIsCrossed_PreservesAsyncLocalScope` | innerhalb Scope `await Task.Yield()`, danach weiterhin gleiche Default-Currency |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `UseDefaultCurrency_WhenUsedInParallelTasks_KeepsTaskLocalDefaults` | 2 parallele Tasks: Task A nutzt `EUR`, Task B nutzt `USD` |
| `Currency` | `Default` | `Default_WhenScopeExists_ReturnsScopedCurrency` | Scope mit `EUR`, erwartetes Ergebnis `EUR` |
| `Currency` | `Default` | `Default_WhenNoScopeExists_ThrowsNoDefaultCurrencyException` | Zugriff ohne Default-Scope |
| `AlphaCode` | `AlphaCode(string)` | `Constructor_WhenValueIsInvalid_ThrowsArgumentException` | `null`, `""`, `" "`, `"EU"`, `"EURO"`, `"eur"`, `"E1R"`, `"ЕUR"(Cyrillic E)` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsAlphaCode` | `"EUR"`, `"USD"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | `"eur"`, `"EU1"`, `"EURO"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | `s = null` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | `"JPY"` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | `"jpy"`, `"JP"`, `"JP¥"` |
| `AlphaCode` | implizite Konvertierung / `ToString()` | `Conversions_WhenRoundTripped_PreserveValue` | `string -> AlphaCode -> string`, Beispiel `"CHF"` |
| `NumericCode` | `NumericCode(int)` | `Constructor_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException` | `-1`, `1000` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsNumericCode` | `"978"`, `"840"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | `"ABC"`, `"1000"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | `s = null` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | `"007"`, `"840"` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | `"-1"`, `"1000"`, `"ABC"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsNonCanonical_RejectsAmbiguousFormats` | Security-Hardening: `" 978"`, `"+978"`, `"٩٧٨"` |
| `NumericCode` | `ToString()` / implizite Konvertierung | `ToString_WhenValueHasLeadingZeros_ReturnsThreeDigits` | `new NumericCode(7)` -> `"007"` |
| `Scale` | `Scale(byte)` | `Constructor_WhenValueGreaterThan28_ThrowsArgumentOutOfRangeException` | `29` |
| `Scale` | Konvertierungsoperatoren | `Conversions_WhenScaleIsValid_ReturnExpectedValues` | `Scale(4) -> int`, `Scale(6) -> byte`, `byte -> Scale` |
| `Ratio` | `Ratio(decimal)` | `Constructor_WhenValueIsNegative_ThrowsArgumentOutOfRangeException` | `-0.01m` |
| `Ratio` | `CompareTo(Ratio?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `1m.CompareTo(null)` |
| `Ratio` | Konvertierungsoperatoren | `Conversions_WhenRoundTripped_PreserveValue` | `decimal -> Ratio -> decimal`, Beispiel `2.25m` |
| `ExchangeRate` | `ExchangeRate(decimal)` | `Constructor_WhenValueIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException` | `0`, `-1` |
| `ExchangeRate` | expliziter Konvertierungsoperator | `ExplicitDecimalConversion_WhenExchangeRateIsValid_ReturnsUnderlyingValue` | `1.25m` |
| `Money` | `Money(decimal, Currency)` | `Constructor_WhenCurrencyIsNull_ThrowsArgumentNullException` | `amount=1m`, `currency=null` |
| `Money` | `Currency` | `Currency_WhenMoneyIsDefault_ThrowsNoCurrencyException` | `default(Money)` |
| `Money` | `Zero()` | `Zero_WhenNoDefaultCurrencyConfigured_ThrowsNoDefaultCurrencyException` | kein Default-Scope |
| `Money` | `Zero(Currency)` | `Zero_WhenCurrencyIsNull_ThrowsArgumentNullException` | `currency=null` |
| `Money` | `In(ExchangeRateContext)` | `In_WhenContextIsNull_ThrowsArgumentNullException` | `context=null` |
| `Money` | `In(ExchangeRateContext)` | `In_WhenMoneyHasNoCurrency_ThrowsNoCurrencyException` | `default(Money)` |
| `Money` | `IsZero` / `IsPositive` / `IsNegative` | `SignProperties_WhenAmountVaries_ReturnExpectedFlags` | `-1`, `0`, `1` |
| `Money` | `Abs()` | `Abs_WhenAmountIsNegative_ReturnsPositiveMoney` | `-10.25 EUR -> 10.25 EUR`, `0 EUR -> 0 EUR`, `10.25 EUR -> 10.25 EUR` |
| `Money` | `Round()` | `Round_WhenCurrencyHasMinorUnits_RoundsToCurrencyScale` | `Currency(TST, minorUnits=2)`, `1.235 -> 1.24` |
| `Money` | `Round(int, MidpointRounding)` | `Round_WhenScaleAndModeProvided_UsesProvidedRounding` | `(1.005,2,ToEven)=>1.00`, `(1.005,2,AwayFromZero)=>1.01` |
| `Money` | `Round(int, MidpointRounding)` | `Round_WhenDecimalsOutOfRange_ThrowsArgumentOutOfRangeException` | `decimals=-1`, `decimals=29` |
| `Money` | `Min(Money, Money)` | `Min_WhenCurrenciesMatch_ReturnsSmallerAmount` | `(10 EUR, 5 EUR)=>5 EUR`, `(5 EUR, 5 EUR)=>left` |
| `Money` | `Min(Money, Money)` | `Min_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR`, `10 USD` |
| `Money` | `Max(Money, Money)` | `Max_WhenCurrenciesMatch_ReturnsGreaterAmount` | `(10 EUR, 5 EUR)=>10 EUR`, `(5 EUR, 5 EUR)=>left` |
| `Money` | `Max(Money, Money)` | `Max_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR`, `10 USD` |
| `Money` | `Distribute(int)` | `Distribute_WhenCountIsLessThanOrEqualZero_ThrowsArgumentOutOfRangeException` | `count=0`, `count=-1` |
| `Money` | `Distribute(int)` | `Distribute_WhenCountIsValid_PreservesSumAndCount` | `47.11 EUR`, `count=3`, erwartet `[15.71,15.70,15.70]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatiosAreNullOrEmpty_ThrowsArgumentException` | `ratios=null`, `ratios=[]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatioSumIsZero_ThrowsArgumentException` | `ratios=[0,0,0]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenRatiosHaveTie_DistributesRemainderByIndex` | `0.02 EUR`, `ratios=[1,1,1]`, erwartet `[0.01,0.01,0.00]` |
| `Money` | `Distribute(params Ratio[])` | `Distribute_WhenAmountIsNegative_DistributesNegativeRemainderByIndex` | `-0.02 EUR`, `ratios=[1,1,1]`, erwartet `[-0.01,-0.01,0.00]` |
| `Money` | `operator +(Money, Money)` | `Addition_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR + 5 USD` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenDivisorIsZero_ThrowsDivideByZeroException` | `10 EUR / 0 EUR` |
| `Money` | `operator /(Money, decimal)` | `DivisionByDecimal_WhenDivisorIsZero_ThrowsDivideByZeroException` | `10 EUR / 0m` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR`, `10 USD` |
| `Money` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Money` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Money` | `ToString()` | `ToString_WhenCultureIsEnUs_UsesAlphaCodeAndMinorUnits` | `Culture=en-US`, `12.34 EUR`, `1.2345 TST(minorUnits=3)` |
| `Wallet` | `Of(params Money[])` | `Of_WhenMoneyArrayIsNull_ThrowsArgumentNullException` | `moneys=null` |
| `Wallet` | `Of(IReadOnlyCollection<Money>)` | `Of_WhenMoneyCollectionIsNull_ThrowsArgumentNullException` | `moneys=null` |
| `Wallet` | `operator +(Wallet, Wallet)` | `Addition_WhenWalletsContainSameCurrencies_AggregatesPerCurrency` | `left=[10 EUR,2 USD]`, `right=[5 EUR,3 USD]` |
| `Wallet` | `operator +(Wallet, Money)` | `Addition_WhenMoneyCurrencyExists_UpdatesBucketAmount` | `wallet=[1 EUR,2 USD]`, `money=3 EUR` -> `EUR=4` |
| `Wallet` | `operator -(Wallet, Money)` | `Subtraction_WhenResultBecomesZero_RemovesBucket` | `wallet=[1 EUR]`, `money=1 EUR` -> `empty` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenDivisorIsZero_ThrowsDivideByZeroException` | `wallet=[1 EUR]`, `divisor=0` |
| `Wallet` | `In(ExchangeRateContext)` | `In_WhenContextIsNull_ThrowsArgumentNullException` | `context=null` |
| `Wallet` | `ToBuilder()` | `ToBuilder_WhenWalletIsMutatedThroughBuilder_DoesNotMutateOriginalWallet` | Original-Wallet mit Builder aendern, Original bleibt unveraendert |
| `Wallet` | `Equals(Wallet)` / `GetHashCode()` | `Equals_WhenInsertionOrderDiffers_ReturnsTrueAndSameHashCode` | `Wallet.Of(1 EUR,2 USD)` vs `Wallet.Of(2 USD,1 EUR)` |
| `Wallet` | `CompareTo(ContextedMoney?)` / `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Wallet` | Vergleichsoperatoren mit `ContextedMoney`/`ContextedWallet` | `ComparisonOperators_WhenAnyReferenceOperandIsNull_ReturnExpectedResult` | `wallet=null`, `contextedMoney=null`, `contextedWallet=null` |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenSingleCurrencyWallet_ReturnsThatCurrency` | `wallet=[1 USD,2 USD]` -> `USD` |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenMultiCurrencyWallet_UsesDefaultCurrency` | `wallet=[1 EUR,1 USD]`, Default=`EUR` |
| `Wallet` | `ResolveCurrency()` | `ResolveCurrency_WhenMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException` | `wallet=[1 EUR,1 USD]`, kein Default-Scope |
| `Wallet.Builder` | `Add(Money)` | `Add_WhenCurrencyExists_AggregatesAmount` | Start: `EUR=1`, add `EUR=2` -> `EUR=3` |
| `Wallet.Builder` | `AddRange(IEnumerable<Money>)` | `AddRange_WhenMoneysProvided_AggregatesPerCurrency` | `[EUR1,USD2,EUR3]` -> `EUR4,USD2` |
| `Wallet.Builder` | `AddRange(IEnumerable<Money>)` | `AddRange_WhenMoneysIsNull_ThrowsArgumentNullException` | `moneys=null` |
| `Wallet.Builder` | `this[Currency] set` | `IndexerSet_WhenMoneyCurrencyDiffers_ThrowsDifferentCurrencyException` | `key=EUR`, `value=1 USD` |
| `Wallet.Builder` | `this[Currency] set` | `IndexerSet_WhenKeyIsNull_ThrowsArgumentNullException` | `key=null`, `value=1 EUR` |
| `Wallet.Builder` | `this[Currency] get` | `IndexerGet_WhenKeyMissing_ThrowsKeyNotFoundException` | `missingKey=JPY` |
| `Wallet.Builder` | `Remove(Currency)` / `Clear()` | `RemoveAndClear_WhenCalled_UpdatesCount` | remove existent/nicht existent, danach `Clear()` |
| `Wallet.Builder` | `TryGetValue(Currency, out Money)` | `TryGetValue_WhenKeyExists_ReturnsTrueAndValue` | Keys: `EUR`, `JPY` |
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderContainsEntries_ReturnsEquivalentWallet` | Builder-Inhalt gegen `Wallet.Of(...)` vergleichen |
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderChangesAfterCreation_DoesNotMutateReturnedWallet` | Snapshot-Test: `wallet1=ToWallet()`, danach Builder mutieren |
| `ExchangeRateContext` | `ExchangeRateContext(Currency, DateTimeOffset, IImmutableDictionary<Currency, ExchangeRate>)` | `Constructor_WhenBaseCurrencyIsNull_ThrowsArgumentNullException` | `base=null` |
| `ExchangeRateContext` | `ExchangeRateContext(Currency, DateTimeOffset, IImmutableDictionary<Currency, ExchangeRate>)` | `Constructor_WhenExchangeRatesAreNull_ThrowsArgumentNullException` | `exchangeRates=null` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenFromCurrencyIsNull_ThrowsArgumentNullException` | `from=null`, `to=USD` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenToCurrencyIsNull_ThrowsArgumentNullException` | `from=USD`, `to=null` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenSourceAndTargetCurrencyMatch_ReturnsOne` | `USD->USD = 1` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenFromIsBase_ReturnsDirectRate` | Base=`EUR`, Rates: `USD=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenToIsBase_ReturnsInverseRate` | `USD->EUR` erwartet `0.5` bei `EUR->USD=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenNeitherIsBase_ReturnsCrossRate` | Base=`EUR`, `USD=2`, `JPY=4`, `USD->JPY=2` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenPairMissing_ThrowsInvalidOperationException` | Nur `USD` vorhanden, Anfrage `EUR->JPY` |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenCalledConcurrently_ReturnsConsistentValues` | 1000 parallele Reads auf direkt/invers/cross |
| `ExchangeRateContext` | `GetEnumerator()` | `GetEnumerator_WhenRatesExist_ReturnsAllConfiguredRates` | Rates: `USD`, `JPY`, erwartete Anzahl `2` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenTargetCurrencyIsNull_ThrowsArgumentNullException` | `to=null` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRateIsMissing_ThrowsInvalidOperationException` | Context ohne benoetigte Rate |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenSourceEqualsTarget_ReturnsSameAmountAndCurrency` | `12.34 USD` nach `USD` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRoundResultIsFalse_DoesNotRound` | `EUR->JPY rate=1.5`, `amount=1`, `RoundResult=false` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenScaleProvided_UsesScaleAndMode` | `rate=1.234567`, `scale=4`, `ToEven` -> `1.2346` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenOptionsAreNull_UsesDefaultRoundingByTargetMinorUnits` | `to.MinorUnits=2`, `amount=1.005` |
| `ContextedMoney` | `ToString()` | `ToString_WhenCalled_DelegatesToUnderlyingMoney` | `12.34 EUR` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenCurrencyIsNull_ThrowsArgumentNullException` | `currency=null` |
| `ContextedWallet` | `Total()` | `Total_WhenSingleCurrencyWallet_ReturnsTotalInSingleCurrency` | `wallet=[1 USD,2 USD]` -> `3 USD` |
| `ContextedWallet` | `Total()` | `Total_WhenMultiCurrencyWallet_UsesResolvedDefaultCurrency` | `wallet=[1 EUR,1 USD]`, `EUR->USD=2`, Ergebnis `1.50 EUR` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenRoundResultIsFalse_ReturnsUnroundedTotal` | `wallet=[1 EUR,1 USD]`, `RoundResult=false` -> `1 + 1/2` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenScaleAndModeProvided_UsesFinalRoundingOptions` | gemischte Wallet, `scale=4`, `AwayFromZero` |
| `ContextedWallet` | `Total()` | `Total_WhenWalletIsMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException` | `wallet=[1 EUR,1 USD]`, kein Default-Scope |
| `ConversionOptions` | `Default` | `Default_WhenAccessed_ReturnsExpectedDefaultValues` | `RoundResult=true`, `RoundingMode=ToEven`, `Scale=null` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeExists_ReturnsCurrency` | `EUR`, `USD` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | `ZZZ` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeIsNull_ThrowsArgumentNullException` | `alphaCode=null` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeExists_ReturnsCurrency` | `978`, `840` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | `001` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeIsNull_ThrowsArgumentNullException` | `numericCode=null` |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_HasUniqueAlphaAndNumericCodes` | Reflection ueber alle `Currency`-Felder |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_ContainsExpectedMinorUnitsSamples` | Stichprobe: `JPY=0`, `EUR=2`, `KWD=3` |
| `Iso4217` | `FindByAlphaCode` / `FindByNumericCode` | `Lookup_WhenCalledConcurrently_ReturnsConsistentCurrencies` | parallele Lookups in 8+ Threads, mehrfacher Warm-Start |

## Property-Based-Tests (FsCheck)

| Klassenname, die getestet werden soll | Methodenname, die getestet werden soll | Methodennamen fuer den Test | Testdaten |
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

## Performance-Tests (BenchmarkDotNet)

| Klassenname, die getestet werden soll | Methodenname, die getestet werden soll | Methodennamen fuer den Test | Testdaten |
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
