using System.Globalization;
using CurrenCSharp;
using CurrenCSharp.Currencies;
using CurrenCSharp.Example;

// Set CultureInfo (for demo purposes)
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// 1. Use EUR as default currency in this ambient context.
using var defaultCurrencyScope = CurrenC.UseDefaultCurrency(Iso4217.EUR);

// 2. Create Money objects
Money default_zero = Money.Zero();        // EUR 0.00 (uses default currency)
Money usd_zero = Money.Zero(Iso4217.USD); // USD 0.00

Money eur_47_11 = new(47.11m, Iso4217.EUR);
Money eur_23_42 = new(23.42m, Iso4217.EUR);

Money usd_47_11 = new(47.11m, Iso4217.USD);
Money usd_23_42 = new(23.42m, Iso4217.USD);

Money chf_47_11 = new(47.11m, Iso4217.CHF);
Money chf_23_42 = new(23.42m, Iso4217.CHF);

// 3. Create Wallet objects
Wallet empty      = Wallet.Empty;          // Empty wallet with no money objects
Wallet simple     = Wallet.Of(eur_47_11);  // Wallet with a single money object
Wallet multiple   = Wallet.Of(eur_47_11, usd_47_11, chf_47_11);   // Wallet with multiple money objects in different currencies
Wallet collection = Wallet.Of([eur_47_11, usd_47_11, chf_47_11]); // Wallet created from a collection of money objects

// 4. Create an exchange rate provider and load contexts.
IExchangeRateProvider provider = new ExampleExchangeRateProvider();
DateTimeOffset exchangeRateDate = new(new DateTime(2020, 1, 2), TimeSpan.Zero);

ExchangeRateContext latest     = await provider.GetLatestAsync();                     // latest exchange rates for the current date
ExchangeRateContext historical = await provider.GetHistoricalAsync(exchangeRateDate); // historical exchange rates for January 1st, 2020

// 5. Bind money and wallet objects to an exchange-rate context to enable context-aware operations.
ContextedMoney latest_money     = eur_47_11.In(latest);
ContextedMoney historical_money = eur_47_11.In(historical);

ContextedWallet latest_wallet     = collection.In(latest);
ContextedWallet historical_wallet = collection.In(historical);

// 6. Convert a ContextedMoney to a different currency using the exchange rates from the context.
Money latest_money_usd     = latest_money.Convert(Iso4217.USD);     // USD 55.40
Money historical_money_usd = historical_money.Convert(Iso4217.USD); // USD 52.73

// 7. Calculate total wallet value in a specific currency using the exchange rates from the context.
Money latest_total_KeyCurrency = latest_wallet.Total();            // EUR 138.44
Money latest_total_usd         = latest_wallet.Total(Iso4217.USD); // USD 162.80

// 8. Arithmetic operations
// 8.1. Operations on money objects (must be the same currency, otherwise the operation will throw an exception)
{
    Money   negate   = -eur_47_11;            // EUR -47.11
    Money   sum      = eur_47_11 + eur_23_42; // EUR 70.53
    Money   diff     = eur_47_11 - eur_23_42; // EUR 23.69
    Money   multiply = eur_47_11 * 2;         // EUR 94.22
    decimal quote    = eur_47_11 / eur_23_42; // 2.01
}

// 8.2. operations on wallet objects
{
    Wallet negate      = -collection;            // Wallet with EUR -47.11, USD -47.11, CHF -47.11
    Wallet addition    = collection + eur_23_42; // Wallet with EUR 70.53, USD 47.11, CHF 47.11
    Wallet subtraction = collection - eur_23_42; // Wallet with EUR 23.69, USD 47.11, CHF 47.11
    Wallet multiply    = collection * 3;         // Wallet with EUR 141.33, USD 141.33, CHF 141.33
    Wallet division    = collection / 2;         // Wallet with EUR 23.56, USD 23.56, CHF 23.56
}

// 9. Comparison operations
// 9.1. Comparison of Money objects (currencies must be the same)
{
    bool isEqual          = eur_47_11 == eur_47_11; // True
    bool isNotEqual       = eur_47_11 != eur_23_42; // True
    bool isGreater        = eur_47_11 > eur_23_42;  // True
    bool isGreaterOrEqual = eur_47_11 >= eur_23_42; // True
    bool isLess           = eur_47_11 < eur_23_42;  // False
    bool isLessOrEqual    = eur_47_11 <= eur_23_42; // False
}

// 9.2. Comparison of Money objects with ContextedMoney objects (currencies can be different)
{
    bool isEqual1          = usd_47_11 == latest_money; // False
    bool isEqual2          = latest_money == usd_47_11; // False
    bool isGreater1        = usd_47_11 > latest_money;  // False
    bool isGreater2        = latest_money > usd_47_11;  // True
    bool isGreaterOrEqual1 = usd_47_11 >= latest_money; // False
    bool isGreaterOrEqual2 = latest_money >= usd_47_11; // True
    bool isLess1           = usd_47_11 < latest_money;  // True
    bool isLess2           = latest_money < usd_47_11;  // False
    bool isLessOrEqual1    = usd_47_11 <= latest_money; // True
    bool isLessOrEqual2    = latest_money <= usd_47_11; // False
}

// 9.3. Comparison of Money objects with ContextedWallet objects (currencies must be the same)
{
    bool isEqual1          = eur_47_11 == latest_wallet; // False
    bool isEqual2          = latest_wallet == eur_47_11; // False
    bool isGreater1        = eur_47_11 > latest_wallet;  // False
    bool isGreater2        = latest_wallet > eur_47_11;  // True
    bool isGreaterOrEqual1 = eur_47_11 >= latest_wallet; // False
    bool isGreaterOrEqual2 = latest_wallet >= eur_47_11; // True
    bool isLess1           = eur_47_11 < latest_wallet;  // True
    bool isLess2           = latest_wallet < eur_47_11;  // False
    bool isLessOrEqual1    = eur_47_11 <= latest_wallet; // True
    bool isLessOrEqual2    = latest_wallet <= eur_47_11; // False
}

// 9.4. Comparison of Wallet objects with ContextedMoney objects (currencies can be different)
{
    bool isEqual1          = collection == latest_money; // False
    bool isEqual2          = latest_money == collection; // False
    bool isGreater1        = collection > latest_money;  // True
    bool isGreater2        = latest_money > collection;  // False
    bool isGreaterOrEqual1 = collection >= latest_money; // True
    bool isGreaterOrEqual2 = latest_money >= collection; // False
    bool isLess1           = collection < latest_money;  // False
    bool isLess2           = latest_money < collection;  // True
    bool isLessOrEqual1    = collection <= latest_money; // False
    bool isLessOrEqual2    = latest_money <= collection; // True
}

// 9.5. Comparison of Wallet objects with ContextedWallet objects (currencies can be different)
{
    bool isEqual1          = collection == latest_wallet; // True
    bool isEqual2          = latest_wallet == collection; // True
    bool isGreater1        = collection > latest_wallet;  // False
    bool isGreater2        = latest_wallet > collection;  // False
    bool isGreaterOrEqual1 = collection >= latest_wallet; // True
    bool isGreaterOrEqual2 = latest_wallet >= collection; // True
    bool isLess1           = collection < latest_wallet;  // False
    bool isLess2           = latest_wallet < collection;  // False
    bool isLessOrEqual1    = collection <= latest_wallet; // True
    bool isLessOrEqual2    = latest_wallet <= collection; // True
}

// 10. Distributing a money object into parts
// 10.1. Distributes a money object into equal parts
{
    var parts = eur_47_11.Distribute(3); // EUR 15.71, EUR 15.70, EUR 15.70
}

// 10.2. Distributes a money object into parts with different ratios
{
    var parts = eur_47_11.Distribute(3, 1, 2, 0); // EUR 23.56, EUR 7.85, EUR 15.70, 0.00 EUR
}