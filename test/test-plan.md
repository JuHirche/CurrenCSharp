# Test Plan

Diese Datei enthaelt den aktuellen Test-Backlog als Tabellen fuer Unit-, Property-Based-, Performance-, Concurrency- und Security-Tests.

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
| `Currency` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenAlphaCodeNumericCodeAndMinorUnitsMatch_ReturnsTrueAndSameHashCode` | zwei `Currency(EUR,978,2)`-Instanzen |
| `Currency` | `Equals(object?)` | `Equals_WhenMinorUnitsDiffer_ReturnsFalse` | `Currency(EUR,978,2)` vs `Currency(EUR,978,3)` |
| `Currency` | `Equals(object?)` | `Equals_WhenAlphaCodesDiffer_ReturnsFalse` | `Currency(EUR,...)` vs `Currency(USD,...)` |
| `AlphaCode` | `AlphaCode(string)` | `Constructor_WhenValueIsInvalid_ThrowsArgumentException` | `null`, `""`, `" "`, `"EU"`, `"EURO"`, `"eur"`, `"E1R"`, `"ЕUR"(Cyrillic E)` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsAlphaCode` | `"EUR"`, `"USD"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | `"eur"`, `"EU1"`, `"EURO"` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | `s = null` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | `"JPY"` |
| `AlphaCode` | `TryParse(string?, out AlphaCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | `"jpy"`, `"JP"`, `"JP¥"` |
| `AlphaCode` | implizite Konvertierung / `ToString()` | `Conversions_WhenRoundTripped_PreserveValue` | `string -> AlphaCode -> string`, Beispiel `"CHF"` |
| `AlphaCode` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | zwei `AlphaCode("EUR")`-Instanzen |
| `AlphaCode` | `Equals(object?)` | `Equals_WhenComparedWithDifferentType_ReturnsFalse` | `AlphaCode("EUR")` vs `"EUR"` (string) |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenValueContainsNonAsciiLetters_ThrowsArgumentException` | Security-Hardening: `"Çur"`, `"Ëur"`, `"𝐀BC"` (math variant) |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenValueIsExcessivelyLong_ThrowsArgumentException` | DoS-Hardening: 10_000 Zeichen |
| `AlphaCode` | `Parse(string)` | `Parse_WhenInputContainsControlCharacters_ThrowsFormatException` | `"E\0R"`, `"EU\nR"` |
| `NumericCode` | `NumericCode(int)` | `Constructor_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException` | `-1`, `1000` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsValid_ReturnsNumericCode` | `"978"`, `"840"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsInvalid_ThrowsFormatException` | `"ABC"`, `"1000"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenValueIsNull_ThrowsArgumentNullException` | `s = null` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsValid_ReturnsTrueAndResult` | `"007"`, `"840"` |
| `NumericCode` | `TryParse(string?, out NumericCode?)` | `TryParse_WhenValueIsInvalid_ReturnsFalseAndNullResult` | `"-1"`, `"1000"`, `"ABC"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsNonCanonical_RejectsAmbiguousFormats` | Security-Hardening: `" 978"`, `"+978"`, `"٩٧٨"` |
| `NumericCode` | `ToString()` / implizite Konvertierung | `ToString_WhenValueHasLeadingZeros_ReturnsThreeDigits` | `new NumericCode(7)` -> `"007"` |
| `NumericCode` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | zwei `NumericCode(978)`-Instanzen |
| `NumericCode` | `Constructor(int)` | `Constructor_WhenValueIsAtBoundary_Succeeds` | `0`, `999` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsNegative_ThrowsFormatException` | `"-1"`, `"-978"` |
| `Scale` | `Scale(byte)` | `Constructor_WhenValueGreaterThan28_ThrowsArgumentOutOfRangeException` | `29` |
| `Scale` | `Scale(byte)` | `Constructor_WhenValueIsAtBoundary_Succeeds` | `0`, `28` |
| `Scale` | Konvertierungsoperatoren | `Conversions_WhenScaleIsValid_ReturnExpectedValues` | `Scale(4) -> int`, `Scale(6) -> byte`, `byte -> Scale` |
| `Scale` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | zwei `Scale(4)`-Instanzen |
| `Ratio` | `Ratio(decimal)` | `Constructor_WhenValueIsNegative_ThrowsArgumentOutOfRangeException` | `-0.01m` |
| `Ratio` | `CompareTo(Ratio?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `1m.CompareTo(null)` |
| `Ratio` | Konvertierungsoperatoren | `Conversions_WhenRoundTripped_PreserveValue` | `decimal -> Ratio -> decimal`, Beispiel `2.25m` |
| `Ratio` | `Ratio(decimal)` | `Constructor_WhenValueIsZero_Succeeds` | `0m` |
| `Ratio` | `CompareTo(Ratio?)` | `CompareTo_WhenOtherIsEqualOrLessOrGreater_ReturnsExpectedSign` | `Ratio(1m).CompareTo(Ratio(1m))=0`, `Ratio(1m).CompareTo(Ratio(2m))<0`, `Ratio(2m).CompareTo(Ratio(1m))>0` |
| `Ratio` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | zwei `Ratio(1.5m)`-Instanzen |
| `ExchangeRate` | `ExchangeRate(decimal)` | `Constructor_WhenValueIsLessThanOrEqualToZero_ThrowsArgumentOutOfRangeException` | `0`, `-1` |
| `ExchangeRate` | expliziter Konvertierungsoperator | `ExplicitDecimalConversion_WhenExchangeRateIsValid_ReturnsUnderlyingValue` | `1.25m` |
| `ExchangeRate` | `Equals(object?)` / `GetHashCode()` | `Equals_WhenValuesMatch_ReturnsTrueAndSameHashCode` | zwei `ExchangeRate(1.25m)`-Instanzen |
| `ExchangeRate` | `ExchangeRate(decimal)` | `Constructor_WhenValueIsVerySmallPositive_Succeeds` | `0.00000001m` (Boundary) |
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
| `Money` | `operator +(Money, Money)` | `Addition_WhenCurrenciesMatch_ReturnsSum` | `10 EUR + 5 EUR = 15 EUR` |
| `Money` | `operator +(Money, Money)` | `Addition_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR + 5 USD` |
| `Money` | `operator -(Money, Money)` | `Subtraction_WhenCurrenciesMatch_ReturnsDifference` | `10 EUR - 5 EUR = 5 EUR` |
| `Money` | `operator -(Money, Money)` | `Subtraction_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR - 5 USD` |
| `Money` | `operator +(Money)` (unary) | `UnaryPlus_WhenCalled_ReturnsSameAmount` | `+(10 EUR) = 10 EUR` |
| `Money` | `operator -(Money)` (unary) | `UnaryNegation_WhenCalled_ReturnsNegatedAmount` | `-(10 EUR) = -10 EUR`, `-(0 EUR) = 0 EUR` |
| `Money` | `operator *(Money, decimal)` | `Multiplication_WhenFactorIsScalar_ReturnsScaledMoney` | `10 EUR * 2m = 20 EUR`, `10 EUR * 0m = 0 EUR`, `10 EUR * -1m = -10 EUR` |
| `Money` | `operator *(decimal, Money)` | `Multiplication_WhenOrderIsReversed_ReturnsSameResult` | `2m * 10 EUR = 20 EUR` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenCurrenciesMatch_ReturnsRatio` | `10 EUR / 5 EUR = 2m` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR / 5 USD` |
| `Money` | `operator /(Money, Money)` | `DivisionByMoney_WhenDivisorIsZero_ThrowsDivideByZeroException` | `10 EUR / 0 EUR` |
| `Money` | `operator /(Money, decimal)` | `DivisionByDecimal_WhenDivisorIsScalar_ReturnsScaledMoney` | `10 EUR / 2m = 5 EUR` |
| `Money` | `operator /(Money, decimal)` | `DivisionByDecimal_WhenDivisorIsZero_ThrowsDivideByZeroException` | `10 EUR / 0m` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenCurrenciesMatch_ReturnsExpectedSign` | `5 EUR vs 10 EUR < 0`, `10 EUR vs 10 EUR == 0`, `10 EUR vs 5 EUR > 0` |
| `Money` | `CompareTo(Money)` | `CompareTo_WhenCurrenciesDiffer_ThrowsDifferentCurrencyException` | `10 EUR`, `10 USD` |
| `Money` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Money` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenCurrenciesDiffer_ConvertsBeforeComparing` | `100 USD.CompareTo((100 EUR).In(ctx))` mit Rate `EUR->USD=1.1` |
| `Money` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Money` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenCurrenciesDiffer_UsesTotalInOwnCurrency` | `100 USD` vs `Wallet[50 EUR, 50 USD].In(ctx)` |
| `Money` | `operator ==(Money, ContextedMoney)` / `!=` / `<` / `<=` / `>` / `>=` | `CrossTypeComparisonOperators_WhenCompared_ReturnExpectedResult` | `100 USD` vs `(100 USD).In(ctx)` mit identischen/abweichenden Betraegen |
| `Money` | `operator ==(ContextedMoney, Money)` / `!=` / `<` / `<=` / `>` / `>=` | `CrossTypeComparisonOperators_ReversedOperand_ReturnExpectedResult` | wie oben, Operanden getauscht |
| `Money` | `operator ==(Money, ContextedWallet)` / `!=` / `<` / `<=` / `>` / `>=` | `CrossTypeComparisonOperators_WithContextedWallet_ReturnExpectedResult` | `100 EUR` vs `Wallet[50 EUR, 50 USD].In(ctx)` |
| `Money` | `operator ==(ContextedWallet, Money)` / `!=` / `<` / `<=` / `>` / `>=` | `CrossTypeComparisonOperators_WithContextedWalletReversed_ReturnExpectedResult` | wie oben, Operanden getauscht |
| `Money` | `Equals(Money)` / `GetHashCode()` | `Equals_WhenAmountAndCurrencyMatch_ReturnsTrueAndSameHashCode` | zwei `Money(10m, EUR)`-Werte |
| `Money` | `Equals(Money)` | `Equals_WhenAmountsDifferOnly_ReturnsFalse` | `Money(10m, EUR)` vs `Money(11m, EUR)` |
| `Money` | `Equals(Money)` | `Equals_WhenCurrenciesDifferOnly_ReturnsFalse` | `Money(10m, EUR)` vs `Money(10m, USD)` |
| `Money` | `default(Money)` | `DefaultMoney_WhenAmountAccessed_ReturnsZero` | `default(Money).Amount == 0m` |
| `Money` | `ToString()` | `ToString_WhenCultureIsEnUs_UsesAlphaCodeAndMinorUnits` | `Culture=en-US`, `12.34 EUR`, `1.2345 TST(minorUnits=3)` |
| `Money` | `ToString()` | `ToString_WhenAmountIsNegative_ContainsNegativeSign` | `-12.34 EUR` |
| `Money` | `ToString()` | `ToString_WhenCurrencyHasZeroMinorUnits_OmitsDecimalDigits` | `Currency(JPY, minorUnits=0)`, `1234m` |
| `Wallet` | `Empty` | `Empty_WhenAccessed_ReturnsWalletWithoutEntries` | `Wallet.Empty`, keine Enumerierung |
| `Wallet` | `Of(params Money[])` | `Of_WhenMoneyArrayIsNull_ThrowsArgumentNullException` | `moneys=null` |
| `Wallet` | `Of(params Money[])` | `Of_WhenArrayIsEmpty_ReturnsEmptyWallet` | `Wallet.Of()` |
| `Wallet` | `Of(IReadOnlyCollection<Money>)` | `Of_WhenMoneyCollectionIsNull_ThrowsArgumentNullException` | `moneys=null` |
| `Wallet` | `Of(IReadOnlyCollection<Money>)` | `Of_WhenCollectionIsEmpty_ReturnsEmptyWallet` | `new List<Money>()` |
| `Wallet` | `Of(...)` | `Of_WhenSameCurrencyAppearsMultipleTimes_AggregatesAmounts` | `[1 EUR, 2 EUR, 3 EUR]` -> `6 EUR` |
| `Wallet` | `Of(...)` | `Of_WhenAmountsCancelOut_ReturnsEmptyWallet` | `[10 EUR, -10 EUR]` -> empty |
| `Wallet` | `operator +(Wallet)` (unary) | `UnaryPlus_WhenCalled_ReturnsSameWallet` | `+wallet` |
| `Wallet` | `operator +(Wallet)` (unary) | `UnaryPlus_WhenWalletIsNull_ThrowsArgumentNullException` | `+(Wallet)null` |
| `Wallet` | `operator -(Wallet)` (unary) | `UnaryNegation_WhenCalled_NegatesAllBuckets` | `wallet=[10 EUR,-5 USD]` -> `[-10 EUR,5 USD]` |
| `Wallet` | `operator -(Wallet)` (unary) | `UnaryNegation_WhenWalletIsNull_ThrowsArgumentNullException` | `-(Wallet)null` |
| `Wallet` | `operator +(Wallet, Wallet)` | `Addition_WhenWalletsContainSameCurrencies_AggregatesPerCurrency` | `left=[10 EUR,2 USD]`, `right=[5 EUR,3 USD]` |
| `Wallet` | `operator +(Wallet, Wallet)` | `Addition_WhenLeftOrRightIsNull_ThrowsArgumentNullException` | `null + wallet`, `wallet + null` |
| `Wallet` | `operator +(Wallet, Money)` | `Addition_WhenMoneyCurrencyExists_UpdatesBucketAmount` | `wallet=[1 EUR,2 USD]`, `money=3 EUR` -> `EUR=4` |
| `Wallet` | `operator +(Wallet, Money)` | `Addition_WhenWalletIsNull_ThrowsArgumentNullException` | `null + money` |
| `Wallet` | `operator -(Wallet, Wallet)` | `Subtraction_WhenWalletsContainSameCurrencies_SubtractsPerCurrency` | `[10 EUR,5 USD] - [3 EUR,2 USD]` -> `[7 EUR,3 USD]` |
| `Wallet` | `operator -(Wallet, Wallet)` | `Subtraction_WhenLeftOrRightIsNull_ThrowsArgumentNullException` | `null - wallet`, `wallet - null` |
| `Wallet` | `operator -(Wallet, Money)` | `Subtraction_WhenResultBecomesZero_RemovesBucket` | `wallet=[1 EUR]`, `money=1 EUR` -> `empty` |
| `Wallet` | `operator -(Wallet, Money)` | `Subtraction_WhenWalletIsNull_ThrowsArgumentNullException` | `null - money` |
| `Wallet` | `operator *(Wallet, decimal)` | `Multiplication_WhenFactorIsScalar_ScalesAllBuckets` | `wallet=[10 EUR,5 USD] * 2m` -> `[20 EUR,10 USD]` |
| `Wallet` | `operator *(Wallet, decimal)` | `Multiplication_WhenFactorIsZero_ReturnsEmptyWallet` | `wallet * 0m` -> `empty` |
| `Wallet` | `operator *(Wallet, decimal)` | `Multiplication_WhenWalletIsNull_ThrowsArgumentNullException` | `null * 2m` |
| `Wallet` | `operator *(decimal, Wallet)` | `Multiplication_WhenOrderIsReversed_ReturnsSameResult` | `2m * wallet` identisch zu `wallet * 2m` |
| `Wallet` | `operator *(decimal, Wallet)` | `Multiplication_WhenWalletIsNull_ThrowsArgumentNullException` | `2m * (Wallet)null` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenDivisorIsScalar_ScalesAllBuckets` | `wallet=[10 EUR,5 USD] / 2m` -> `[5 EUR,2.5 USD]` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenDivisorIsZero_ThrowsDivideByZeroException` | `wallet=[1 EUR]`, `divisor=0` |
| `Wallet` | `operator /(Wallet, decimal)` | `Division_WhenWalletIsNull_ThrowsArgumentNullException` | `null / 2m` |
| `Wallet` | `GetEnumerator()` | `GetEnumerator_WhenWalletHasEntries_YieldsAllMoneys` | `wallet=[1 EUR,2 USD]` -> 2 Elemente |
| `Wallet` | `GetEnumerator()` | `GetEnumerator_WhenWalletIsEmpty_YieldsNothing` | `Wallet.Empty` -> 0 Elemente |
| `Wallet` | `IEnumerable.GetEnumerator()` (nicht-generisch) | `NonGenericEnumerator_WhenIterated_ReturnsSameResultsAsGenericEnumerator` | `((IEnumerable)wallet).GetEnumerator()` |
| `Wallet` | `In(ExchangeRateContext)` | `In_WhenContextIsNull_ThrowsArgumentNullException` | `context=null` |
| `Wallet` | `ToBuilder()` | `ToBuilder_WhenWalletIsMutatedThroughBuilder_DoesNotMutateOriginalWallet` | Original-Wallet mit Builder aendern, Original bleibt unveraendert |
| `Wallet` | `Equals(Wallet)` / `GetHashCode()` | `Equals_WhenInsertionOrderDiffers_ReturnsTrueAndSameHashCode` | `Wallet.Of(1 EUR,2 USD)` vs `Wallet.Of(2 USD,1 EUR)` |
| `Wallet` | `CompareTo(ContextedMoney?)` / `CompareTo(ContextedWallet?)` | `CompareTo_WhenOtherIsNull_ReturnsPositiveValue` | `other=null` |
| `Wallet` | `CompareTo(ContextedMoney?)` | `CompareTo_WhenWalletTotalEqualsContextedMoney_ReturnsZero` | `wallet=[50 EUR,50 USD]`, `ctx EUR->USD=1`, `other=100 USD` |
| `Wallet` | `CompareTo(ContextedWallet?)` | `CompareTo_WhenBothTotalsConvertedToSameCurrency_ComparesTotals` | `left=[10 EUR].In(ctx)`, `right=[9 EUR,1 USD].In(ctx)` |
| `Wallet` | `operator ==(Wallet, ContextedWallet)` / `!=` | `EqualityOperators_WithContextedWallet_ReturnExpectedResult` | identische/unterschiedliche Totals |
| `Wallet` | `operator ==(ContextedWallet, Wallet)` / `!=` | `EqualityOperators_Reversed_ReturnExpectedResult` | wie oben, Operanden getauscht |
| `Wallet` | `operator ==(Wallet, ContextedMoney)` / `!=` | `EqualityOperators_WithContextedMoney_ReturnExpectedResult` | `wallet=[50 EUR,50 USD]` vs `(100 USD).In(ctx)` |
| `Wallet` | `operator ==(ContextedMoney, Wallet)` / `!=` | `EqualityOperators_WithContextedMoneyReversed_ReturnExpectedResult` | wie oben, Operanden getauscht |
| `Wallet` | `operator <(Wallet, ContextedMoney/ContextedWallet)` / `<=` / `>` / `>=` | `OrderOperators_WithContextTypes_ReturnExpectedResult` | Totals groesser/kleiner/gleich |
| `Wallet` | `operator <(ContextedMoney, Wallet)` etc. | `OrderOperators_WithContextTypesReversed_ReturnExpectedResult` | wie oben, Operanden getauscht |
| `Wallet` | Vergleichsoperatoren mit `ContextedMoney`/`ContextedWallet` | `ComparisonOperators_WhenAnyReferenceOperandIsNull_ReturnExpectedResult` | `wallet=null`, `contextedMoney=null`, `contextedWallet=null` |
| `Wallet` | `Equals(Wallet?)` / `Equals(object?)` / `GetHashCode()` | `Equals_WhenComparedWithNull_ReturnsFalse` | `wallet.Equals((Wallet?)null)`, `wallet.Equals((object?)null)` |
| `Wallet` | `Equals(object?)` | `Equals_WhenComparedWithDifferentType_ReturnsFalse` | `wallet.Equals("some string")` |
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
| `Wallet.Builder` | `ToWallet()` | `ToWallet_WhenBuilderIsEmpty_ReturnsEmptyWallet` | keine Entries |
| `Wallet.Builder` | `GetEnumerator()` | `GetEnumerator_WhenBuilderHasEntries_YieldsAllKeyValuePairs` | mehrere Currencies, Rueckgabe als `KeyValuePair<Currency,Money>` |
| `Wallet.Builder` | `Count` | `Count_WhenItemsAddedAndRemoved_ReflectsCurrentSize` | Start=0, add 3, remove 1 -> 2 |
| `Wallet.Builder` | `Keys` / `Values` | `KeysAndValues_WhenEnumerated_ReturnCurrentBuilderState` | nach mehreren Add/Remove-Ops |
| `Wallet.Builder` | `ContainsKey(Currency)` | `ContainsKey_WhenKeyExistsOrMissing_ReturnsExpectedResult` | `EUR` vorhanden, `JPY` fehlt |
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
| `ExchangeRateContext` | `GetEnumerator()` | `GetEnumerator_WhenEnumeratedMultipleTimes_ReturnsConsistentResults` | zweimaliger `foreach`-Durchlauf |
| `ExchangeRateContext` | Properties (`BaseCurrency`, `Timestamp`/`Reference`) | `Properties_WhenAccessed_ReturnsConstructorValues` | Ctor-Werte unveraendert gespiegelt |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenSameCurrencyAskedTwice_ReturnsOneBothTimes` | `EUR->EUR`, `USD->USD` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenTargetCurrencyIsNull_ThrowsArgumentNullException` | `to=null` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRateIsMissing_ThrowsInvalidOperationException` | Context ohne benoetigte Rate |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenSourceEqualsTarget_ReturnsSameAmountAndCurrency` | `12.34 USD` nach `USD` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenRoundResultIsFalse_DoesNotRound` | `EUR->JPY rate=1.5`, `amount=1`, `RoundResult=false` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenScaleProvided_UsesScaleAndMode` | `rate=1.234567`, `scale=4`, `ToEven` -> `1.2346` |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenOptionsAreNull_UsesDefaultRoundingByTargetMinorUnits` | `to.MinorUnits=2`, `amount=1.005` |
| `ContextedMoney` | `ToString()` | `ToString_WhenCalled_DelegatesToUnderlyingMoney` | `12.34 EUR` |
| `ContextedMoney` | `Amount` / `Currency` Properties | `Properties_WhenAccessed_DelegateToUnderlyingMoney` | `(12.34 EUR).In(ctx).Amount == 12.34m`, Currency == `EUR` |
| `ContextedMoney` | `Equals`/`GetHashCode` (record) | `Equals_WhenSameMoneyAndContext_ReturnsTrueAndSameHashCode` | zwei identische `ContextedMoney`-Instanzen |
| `ContextedMoney` | `Convert(Currency, ConversionOptions?)` | `Convert_WhenAmountIsZero_ReturnsZeroInTargetCurrency` | `0 EUR -> USD = 0 USD` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenCurrencyIsNull_ThrowsArgumentNullException` | `currency=null` |
| `ContextedWallet` | `Total()` | `Total_WhenSingleCurrencyWallet_ReturnsTotalInSingleCurrency` | `wallet=[1 USD,2 USD]` -> `3 USD` |
| `ContextedWallet` | `Total()` | `Total_WhenMultiCurrencyWallet_UsesResolvedDefaultCurrency` | `wallet=[1 EUR,1 USD]`, `EUR->USD=2`, Ergebnis `1.50 EUR` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenRoundResultIsFalse_ReturnsUnroundedTotal` | `wallet=[1 EUR,1 USD]`, `RoundResult=false` -> `1 + 1/2` |
| `ContextedWallet` | `Total(Currency, ConversionOptions?)` | `Total_WhenScaleAndModeProvided_UsesFinalRoundingOptions` | gemischte Wallet, `scale=4`, `AwayFromZero` |
| `ContextedWallet` | `Total()` | `Total_WhenWalletIsMultiCurrencyAndNoDefault_ThrowsNoDefaultCurrencyException` | `wallet=[1 EUR,1 USD]`, kein Default-Scope |
| `ContextedWallet` | `Total()` | `Total_WhenWalletIsEmpty_ReturnsZeroInResolvedCurrency` | `Wallet.Empty` mit Default=`EUR` |
| `ContextedWallet` | `Wallet` / `Context` Properties | `Properties_WhenAccessed_ReturnOriginalBoundInstances` | Identitaetspruefung der injizierten Instanzen |
| `ConversionOptions` | `Default` | `Default_WhenAccessed_ReturnsExpectedDefaultValues` | `RoundResult=true`, `RoundingMode=ToEven`, `Scale=null` |
| `ConversionOptions` | record `Equals` / `GetHashCode` | `Equals_WhenAllFieldsMatch_ReturnsTrueAndSameHashCode` | zwei identische Konfigurationen |
| `ConversionOptions` | record `Equals` | `Equals_WhenAnyFieldDiffers_ReturnsFalse` | unterschiedliches `RoundResult`/`RoundingMode`/`Scale` |
| `ConversionOptions` | `with`-Expression | `WithExpression_WhenFieldChanged_ReturnsModifiedCopy` | `Default with { RoundResult = false }` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeExists_ReturnsCurrency` | `EUR`, `USD` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | `ZZZ` |
| `Iso4217` | `FindByAlphaCode(AlphaCode)` | `FindByAlphaCode_WhenCodeIsNull_ThrowsArgumentNullException` | `alphaCode=null` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeExists_ReturnsCurrency` | `978`, `840` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeDoesNotExist_ThrowsInvalidOperationException` | `001` |
| `Iso4217` | `FindByNumericCode(NumericCode)` | `FindByNumericCode_WhenCodeIsNull_ThrowsArgumentNullException` | `numericCode=null` |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_HasUniqueAlphaAndNumericCodes` | Reflection ueber alle `Currency`-Felder |
| `Iso4217` | statischer Katalog | `Catalog_WhenLoaded_ContainsExpectedMinorUnitsSamples` | Stichprobe: `JPY=0`, `EUR=2`, `KWD=3` |
| `Iso4217` | `FindByAlphaCode` / `FindByNumericCode` | `Lookup_WhenCalledConcurrently_ReturnsConsistentCurrencies` | parallele Lookups in 8+ Threads, mehrfacher Warm-Start |
| `Iso4217` | `FindByAlphaCode` / `FindByNumericCode` | `Lookup_WhenCalledForFirstTimeConcurrently_InitializesCacheOnce` | 64 Threads starten Lookup gleichzeitig, jeweils Alpha und Numeric, keine Exceptions, konsistente Referenzen |
| `Iso4217` | `FindByAlphaCode` | `FindByAlphaCode_WhenRoundTrippedThroughNumericLookup_ReturnsSameCurrencyInstance` | `EUR` via Alpha, anschliessend numerisch `978` -> gleiche Referenz |
| `Iso4217` | statischer Katalog | `Catalog_WhenInspected_AllCurrenciesAreNonNull` | Reflection: kein `Currency`-Feld ist `null` |

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
| `Money` | `operator -(Money, Money)` | `Subtraction_AddingBackRightOperand_EqualsLeftOperand_Property` | `(a - b) + b == a` fuer gleiche Waehrung |
| `Money` | `operator *(Money, decimal)` | `Multiplication_ByOne_IsIdentity_Property` | `money * 1m == money` |
| `Money` | `operator *(Money, decimal)` | `Multiplication_ByMinusOne_EqualsNegation_Property` | `money * -1m == -money` |
| `Money` | `Abs()` | `Abs_WhenAppliedTwice_IsIdempotent_Property` | `money.Abs().Abs() == money.Abs()` |
| `Money` | `Round` / `Round(int, MidpointRounding)` | `Round_WhenAppliedTwiceWithSameArgs_IsIdempotent_Property` | `m.Round(n,mode).Round(n,mode) == m.Round(n,mode)` |
| `Wallet` | `operator +` / unary `-` | `WalletPlusOwnNegation_AlwaysEqualsEmpty_Property` | Zufaellige Multi-Currency-Wallet |
| `Wallet` | `operator *` / `operator /` | `Multiplication_DivisionByNonZeroScalar_RoundTrips_Property` | `(w * k) / k == w` fuer `k != 0` (Toleranz fuer Rundung) |
| `ExchangeRate` | Inverse | `ExchangeRate_InverseOfInverse_EqualsOriginal_Property` | `1 / (1 / rate)` innerhalb Toleranz |

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
| `Money` | `operator +(Money,Money)` / `operator *(Money,decimal)` | `MoneyArithmetic_Benchmark` | `OperationCount = 100, 10_000, 1_000_000`; gleiche Waehrung |
| `Wallet` | `Equals(Wallet)` / `GetHashCode()` | `WalletEquality_Benchmark` | `WalletSize = 10, 1_000, 100_000`; identische vs. abweichende Wallets |
| `AlphaCode` / `NumericCode` | `Parse(string)` | `CodeParsing_Benchmark` | `ParseCount = 1_000_000`; gueltige und ungueltige Eingaben |

## Thread-Safety- / Concurrency-Tests

| Klassenname, die getestet werden soll | Methodenname, die getestet werden soll | Methodennamen fuer den Test | Testdaten |
|---|---|---|---|
| `CurrenC` | `UseDefaultCurrency(Currency)` | `DefaultCurrency_WhenUsedAcrossAsyncAwaitBoundaries_RemainsIsolatedPerFlow` | 50 parallele Tasks, jede mit eigener Default-Waehrung, zufaelliges `await Task.Yield()` |
| `CurrenC` | `UseDefaultCurrency(Currency)` | `DefaultCurrency_WhenScopeIsDisposedInOtherThread_DoesNotAffectOtherAsyncLocalStacks` | Scope in Task A, Dispose in Task B |
| `Iso4217Cache` | Lazy-Initialisierung | `Iso4217Cache_WhenAccessedConcurrentlyOnColdStart_InitializesExactlyOnce` | Process-neuer Cold-Start, 32 Threads rufen gleichzeitig `FindByAlphaCode` auf; Verifikation ueber Zaehler-Sentinel in Reflection |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenReadConcurrentlyOnSharedInstance_IsRaceFree` | 1 Instanz, 16 Threads, Mischung aus direkt/invers/cross Lookups |
| `Wallet` | Immutability | `Wallet_WhenSharedReadOnlyAcrossThreads_IsSafeForParallelEnumeration` | eine Wallet-Instanz, 16 Threads iterieren parallel, kein Locking noetig |
| `Wallet.Builder` | Mutation | `Builder_WhenMutatedConcurrently_IsDocumentedAsNotThreadSafe` | Negativ-Test: parallele `Add`-Aufrufe in 8 Threads, belegt dokumentierte Nicht-Thread-Safety |

## Security- / Input-Validation-Tests

| Klassenname, die getestet werden soll | Methodenname, die getestet werden soll | Methodennamen fuer den Test | Testdaten |
|---|---|---|---|
| `AlphaCode` | `Parse(string)` / `Constructor(string)` | `Parse_WhenInputContainsUnicodeHomoglyphs_Rejected` | kyrillisches `ЕUR`, griechisches `ΕUR`, Fullwidth `ＥＵＲ` |
| `AlphaCode` | `Parse(string)` | `Parse_WhenInputContainsZeroWidthCharacters_Rejected` | `"E\u200BUR"` (Zero-Width Space), `"EUR\uFEFF"` (BOM) |
| `AlphaCode` | `Constructor(string)` | `Constructor_WhenInputIsExtremelyLarge_DoesNotHang` | DoS-Resistenz: `new string('A', 1_000_000)` muss schnell abgewiesen werden |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputContainsNonAsciiDigits_Rejected` | Arabic-Indic `"٩٧٨"`, Thai `"๙๗๘"`, Devanagari `"९७८"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputContainsSignOrWhitespace_Rejected` | `"+978"`, `" 978"`, `"978 "`, `"\t978"` |
| `NumericCode` | `Parse(string)` | `Parse_WhenInputIsExtremelyLarge_DoesNotHang` | DoS: `new string('9', 1_000_000)` muss schnell abgewiesen werden |
| `Currency` | Hash-Verhalten | `GetHashCode_WhenManyDistinctCurrenciesHashed_HasNegligibleCollisionRate` | alle Iso4217-Waehrungen, Kollisionsrate `< 2 %` (Schutz vor Hash-Pollution) |
| `Wallet` | `Equals` / `GetHashCode` | `Equals_WhenContentsAreSameButConstructedDifferently_RemainConstantTime` | Timing-Test: gleiche und ungleiche Wallets, `stddev` innerhalb Toleranz |
| `ExchangeRateContext` | `GetExchangeRate(Currency, Currency)` | `GetExchangeRate_WhenRateIsVerySmallOrLarge_DoesNotOverflowOrHang` | Raten `decimal.MaxValue / 2`, `1e-20m`; Cross-Rate-Berechnung bleibt stabil |
