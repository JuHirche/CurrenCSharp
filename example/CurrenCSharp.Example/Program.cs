using System.Globalization;
using CurrenCSharp;
using CurrenCSharp.Currencies;
using CurrenCSharp.Example;
// ReSharper disable All

// 0. Set CultureInfo (for demo purposes)
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// 1. Use a Currency as a default currency in this ambient context.
using var defaultCurrencyScope = CurrenC.UseDefaultCurrency(Iso4217.EUR);

// 2. Create Money objects
Money defaultZero = Money.Zero();            // EUR 0.00 (uses default currency)
Money usdZero     = Money.Zero(Iso4217.USD); // USD 0.00

Money eur47_11 = new(47.11m, Iso4217.EUR); // EUR 47.11
Money eur23_42 = new(23.42m, Iso4217.EUR); // EUR 23.42

Money usd47_11 = new(47.11m, Iso4217.USD); // USD 47.11
Money usd23_42 = new(23.42m, Iso4217.USD); // USD 23.42

Money chf47_11 = new(47.11m, Iso4217.CHF); // CHF 47.11
Money chf23_42 = new(23.42m, Iso4217.CHF); // CHF 23.42

// 3. Create Wallet objects
// 3.1. Empty wallet with no money objects
Wallet empty = Wallet.Empty;

// 3.2. Wallet with a single money object
Wallet simple = Wallet.Of(eur47_11);

// 3.3. Wallet with multiple money objects in different currencies
Wallet multiple = Wallet.Of(eur47_11, usd47_11, chf47_11);

// 3.4. Wallet created from a collection of money objects
Wallet collection = Wallet.Of([eur47_11, usd47_11, chf47_11]);

// 4. Create an exchange rate provider and load contexts.
IExchangeRateProvider provider = new ExampleExchangeRateProvider();
DateTimeOffset exchangeRateDate = new(new DateTime(2020, 1, 2), TimeSpan.Zero);

// 4.1. Get the latest exchange rates for the current date
ExchangeRateContext latestExchangeRates = await provider.GetLatestAsync();

// 4.2. Get the historical exchange rates for January 1st, 2020 
ExchangeRateContext historicalExchangeRates = await provider.GetHistoricalAsync(exchangeRateDate);

// 5. Bind money and wallet objects to an exchange-rate context to enable context-aware operations.
ContextedMoney eur4711WithLatestRates     = eur47_11.In(latestExchangeRates);
ContextedMoney eur4711WithHistoricalRates = eur47_11.In(historicalExchangeRates);

ContextedWallet walletWithLatestRates     = collection.In(latestExchangeRates);
ContextedWallet walletWithHistoricalRates = collection.In(historicalExchangeRates);

// 6. Convert a ContextedMoney to a different currency using the exchange rates from the context.
Money convertedEurInUsdWithLatestRates     = eur4711WithLatestRates.Convert(Iso4217.USD);     // USD 55.40
Money convertedEurInUsdWithHistoricalRates = eur4711WithHistoricalRates.Convert(Iso4217.USD); // USD 52.73

// 7. Calculate total wallet value in a specific currency using the exchange rates from the context.
Money totalOfWalletInDefaultCurrencyWithLatestRates  = walletWithLatestRates.Total();                        // EUR 138.44
Money totalOfWalletInUsdWithHistoricalRates          = walletWithHistoricalRates.Total(Iso4217.USD); // USD 162.80

// 8. Arithmetic operations
// 8.1. Operations on money objects (must be the same currency, otherwise the operation will throw an exception)
{
    Money   negate   = -eur47_11;           // EUR -47.11
    Money   sum      = eur47_11 + eur23_42; // EUR 70.53
    Money   diff     = eur47_11 - eur23_42; // EUR 23.69
    Money   multiply = eur47_11 * 2;        // EUR 94.22
    decimal quote    = eur47_11 / eur23_42; // 2.01
}

// 8.2. operations on wallet objects
{
    Wallet negate      = -collection;           // Wallet with EUR -47.11, USD -47.11, CHF -47.11
    Wallet addition    = collection + eur23_42; // Wallet with EUR 70.53, USD 47.11, CHF 47.11
    Wallet subtraction = collection - eur23_42; // Wallet with EUR 23.69, USD 47.11, CHF 47.11
    Wallet multiply    = collection * 3;        // Wallet with EUR 141.33, USD 141.33, CHF 141.33
    Wallet division    = collection / 2;        // Wallet with EUR 23.56, USD 23.56, CHF 23.56
}

// 9. Comparison operations
// 9.1. Comparison of Money objects (currencies must be the same)
{
    bool isEqual          = eur47_11 == new Money(47.11m, Iso4217.EUR); // True
    bool isNotEqual       = eur47_11 != eur23_42; // True
    bool isGreater        = eur47_11 > eur23_42;  // True
    bool isGreaterOrEqual = eur47_11 >= eur23_42; // True
    bool isLess           = eur47_11 < eur23_42;  // False
    bool isLessOrEqual    = eur47_11 <= eur23_42; // False
}

// 9.2. Comparison of Money objects with ContextedMoney objects (currencies can be different)
{
    bool isEqual1          = usd47_11 == eur4711WithLatestRates; // False
    bool isEqual2          = eur4711WithLatestRates == usd47_11; // False
    bool isGreater1        = usd47_11 > eur4711WithLatestRates;  // False
    bool isGreater2        = eur4711WithLatestRates > usd47_11;  // True
    bool isGreaterOrEqual1 = usd47_11 >= eur4711WithLatestRates; // False
    bool isGreaterOrEqual2 = eur4711WithLatestRates >= usd47_11; // True
    bool isLess1           = usd47_11 < eur4711WithLatestRates;  // True
    bool isLess2           = eur4711WithLatestRates < usd47_11;  // False
    bool isLessOrEqual1    = usd47_11 <= eur4711WithLatestRates; // True
    bool isLessOrEqual2    = eur4711WithLatestRates <= usd47_11; // False
}

// 9.3. Comparison of Money objects with ContextedWallet objects (currencies must be the same)
{
    bool isEqual1          = eur47_11 == walletWithLatestRates; // False
    bool isEqual2          = walletWithLatestRates == eur47_11; // False
    bool isGreater1        = eur47_11 > walletWithLatestRates;  // False
    bool isGreater2        = walletWithLatestRates > eur47_11;  // True
    bool isGreaterOrEqual1 = eur47_11 >= walletWithLatestRates; // False
    bool isGreaterOrEqual2 = walletWithLatestRates >= eur47_11; // True
    bool isLess1           = eur47_11 < walletWithLatestRates;  // True
    bool isLess2           = walletWithLatestRates < eur47_11;  // False
    bool isLessOrEqual1    = eur47_11 <= walletWithLatestRates; // True
    bool isLessOrEqual2    = walletWithLatestRates <= eur47_11; // False
}

// 9.4. Comparison of Wallet objects with ContextedMoney objects (currencies can be different)
{
    bool isEqual1          = collection == eur4711WithLatestRates; // False
    bool isEqual2          = eur4711WithLatestRates == collection; // False
    bool isGreater1        = collection > eur4711WithLatestRates;  // True
    bool isGreater2        = eur4711WithLatestRates > collection;  // False
    bool isGreaterOrEqual1 = collection >= eur4711WithLatestRates; // True
    bool isGreaterOrEqual2 = eur4711WithLatestRates >= collection; // False
    bool isLess1           = collection < eur4711WithLatestRates;  // False
    bool isLess2           = eur4711WithLatestRates < collection;  // True
    bool isLessOrEqual1    = collection <= eur4711WithLatestRates; // False
    bool isLessOrEqual2    = eur4711WithLatestRates <= collection; // True
}

// 9.5. Comparison of Wallet objects with ContextedWallet objects (currencies can be different)
{
    bool isEqual1          = collection == walletWithLatestRates; // True
    bool isEqual2          = walletWithLatestRates == collection; // True
    bool isGreater1        = collection > walletWithLatestRates;  // False
    bool isGreater2        = walletWithLatestRates > collection;  // False
    bool isGreaterOrEqual1 = collection >= walletWithLatestRates; // True
    bool isGreaterOrEqual2 = walletWithLatestRates >= collection; // True
    bool isLess1           = collection < walletWithLatestRates;  // False
    bool isLess2           = walletWithLatestRates < collection;  // False
    bool isLessOrEqual1    = collection <= walletWithLatestRates; // True
    bool isLessOrEqual2    = walletWithLatestRates <= collection; // True
}

// 10. Distributing a money object into parts
// 10.1. Distributes a money object into equal parts
{
    var parts = eur47_11.Distribute(3); // EUR 15.71, EUR 15.70, EUR 15.70
}

// 10.2. Distributes a money object into parts with different ratios
{
    var parts = eur47_11.Distribute(3, 1, 2, 0); // EUR 23.56, EUR 7.85, EUR 15.70, 0.00 EUR
}