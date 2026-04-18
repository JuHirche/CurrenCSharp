namespace CurrenCSharp.Currencies;

public static class Iso4217
{
    private static readonly Iso4217Cache _cache = new();

    /// <summary>
    /// Finds a currency by its ISO 4217 alpha code.
    /// </summary>
    /// <param name="alphaCode">The three-letter ISO 4217 alpha code.</param>
    /// <returns>The currency that matches <paramref name="alphaCode"/>.</returns>
    public static Currency FindByAlphaCode(AlphaCode alphaCode)
    {
        ArgumentNullException.ThrowIfNull(alphaCode);
        return _cache.FindByAlphaCode(alphaCode)
            ?? throw new InvalidOperationException($"AlphaCode '{alphaCode}' does not exist in ISO 4217 currencies.");
    }

    /// <summary>
    /// Finds a currency by its ISO 4217 numeric code.
    /// </summary>
    /// <param name="numericCode">The ISO 4217 numeric code.</param>
    /// <returns>The currency that matches <paramref name="numericCode"/>.</returns>
    public static Currency FindByNumericCode(NumericCode numericCode)
    {
        ArgumentNullException.ThrowIfNull(numericCode);
        return _cache.FindByNumericCode(numericCode)
            ?? throw new InvalidOperationException($"NumericCode '{numericCode}' does not exist in ISO 4217 currencies.");
    }

    /// <summary> United Arab Emirates </summary>
    public static readonly Currency AED = new(nameof(AED), 784, 2);
    /// <summary> Afghanistan </summary>
    public static readonly Currency AFN = new(nameof(AFN), 971, 2);
    /// <summary> Albania </summary>
    public static readonly Currency ALL = new(nameof(ALL), 008, 2);
    /// <summary> Armenia </summary>
    public static readonly Currency AMD = new(nameof(AMD), 051, 2);
    /// <summary> Angola </summary>
    public static readonly Currency AOA = new(nameof(AOA), 973, 2);
    /// <summary> Argentina </summary>
    public static readonly Currency ARS = new(nameof(ARS), 032, 2);
    /// <summary> Australia </summary>
    public static readonly Currency AUD = new(nameof(AUD), 036, 2);
    /// <summary> Aruba </summary>
    public static readonly Currency AWG = new(nameof(AWG), 533, 2);
    /// <summary> Azerbaijan </summary>
    public static readonly Currency AZN = new(nameof(AZN), 944, 2);
    /// <summary> Bosnia and Herzegovina </summary>
    public static readonly Currency BAM = new(nameof(BAM), 977, 2);
    /// <summary> Barbados </summary>
    public static readonly Currency BBD = new(nameof(BBD), 052, 2);
    /// <summary> Bangladesh </summary>
    public static readonly Currency BDT = new(nameof(BDT), 050, 2);
    /// <summary> Bahrain </summary>
    public static readonly Currency BHD = new(nameof(BHD), 048, 3);
    /// <summary> Burundi </summary>
    public static readonly Currency BIF = new(nameof(BIF), 108, 0);
    /// <summary> Bermuda </summary>
    public static readonly Currency BMD = new(nameof(BMD), 060, 2);
    /// <summary> Brunei </summary>
    public static readonly Currency BND = new(nameof(BND), 096, 2);
    /// <summary> Bolivia </summary>
    public static readonly Currency BOB = new(nameof(BOB), 068, 2);
    /// <summary> Bolivia </summary>
    public static readonly Currency BOV = new(nameof(BOV), 984, 2);
    /// <summary> Brazil </summary>
    public static readonly Currency BRL = new(nameof(BRL), 986, 2);
    /// <summary> Bahamas </summary>
    public static readonly Currency BSD = new(nameof(BSD), 044, 2);
    /// <summary> Bhutan </summary>
    public static readonly Currency BTN = new(nameof(BTN), 064, 2);
    /// <summary> Botswana </summary>
    public static readonly Currency BWP = new(nameof(BWP), 072, 2);
    /// <summary> Belarus </summary>
    public static readonly Currency BYN = new(nameof(BYN), 933, 2);
    /// <summary> Belize </summary>
    public static readonly Currency BZD = new(nameof(BZD), 084, 2);
    /// <summary> Canada </summary>
    public static readonly Currency CAD = new(nameof(CAD), 124, 2);
    /// <summary> Democratic Republic of the Congo </summary>
    public static readonly Currency CDF = new(nameof(CDF), 976, 2);
    /// <summary> Switzerland </summary>
    public static readonly Currency CHE = new(nameof(CHE), 947, 2);
    /// <summary> Switzerland </summary>
    public static readonly Currency CHF = new(nameof(CHF), 756, 2);
    /// <summary> Switzerland </summary>
    public static readonly Currency CHW = new(nameof(CHW), 948, 2);
    /// <summary> Chile </summary>
    public static readonly Currency CLF = new(nameof(CLF), 990, 4);
    /// <summary> Chile </summary>
    public static readonly Currency CLP = new(nameof(CLP), 152, 0);
    /// <summary> China </summary>
    public static readonly Currency CNY = new(nameof(CNY), 156, 2);
    /// <summary> Colombia </summary>
    public static readonly Currency COP = new(nameof(COP), 170, 2);
    /// <summary> Colombia </summary>
    public static readonly Currency COU = new(nameof(COU), 970, 2);
    /// <summary> Costa Rica </summary>
    public static readonly Currency CRC = new(nameof(CRC), 188, 2);
    /// <summary> Cuba </summary>
    public static readonly Currency CUP = new(nameof(CUP), 192, 2);
    /// <summary> Cape Verde </summary>
    public static readonly Currency CVE = new(nameof(CVE), 132, 2);
    /// <summary> Czech Republic </summary>
    public static readonly Currency CZK = new(nameof(CZK), 203, 2);
    /// <summary> Djibouti </summary>
    public static readonly Currency DJF = new(nameof(DJF), 262, 0);
    /// <summary> Denmark </summary>
    public static readonly Currency DKK = new(nameof(DKK), 208, 2);
    /// <summary> Dominican Republic </summary>
    public static readonly Currency DOP = new(nameof(DOP), 214, 2);
    /// <summary> Algeria </summary>
    public static readonly Currency DZD = new(nameof(DZD), 012, 2);
    /// <summary> Egypt </summary>
    public static readonly Currency EGP = new(nameof(EGP), 818, 2);
    /// <summary> Eritrea </summary>
    public static readonly Currency ERN = new(nameof(ERN), 232, 2);
    /// <summary> Ethiopia </summary>
    public static readonly Currency ETB = new(nameof(ETB), 230, 2);
    /// <summary> Eurozone </summary>
    public static readonly Currency EUR = new(nameof(EUR), 978, 2);
    /// <summary> Fiji </summary>
    public static readonly Currency FJD = new(nameof(FJD), 242, 2);
    /// <summary> Falkland Islands </summary>
    public static readonly Currency FKP = new(nameof(FKP), 238, 2);
    /// <summary> United Kingdom </summary>
    public static readonly Currency GBP = new(nameof(GBP), 826, 2);
    /// <summary> Georgia </summary>
    public static readonly Currency GEL = new(nameof(GEL), 981, 2);
    /// <summary> Ghana </summary>
    public static readonly Currency GHS = new(nameof(GHS), 936, 2);
    /// <summary> Gibraltar </summary>
    public static readonly Currency GIP = new(nameof(GIP), 292, 2);
    /// <summary> Gambia </summary>
    public static readonly Currency GMD = new(nameof(GMD), 270, 2);
    /// <summary> Guinea </summary>
    public static readonly Currency GNF = new(nameof(GNF), 324, 0);
    /// <summary> Guatemala </summary>
    public static readonly Currency GTQ = new(nameof(GTQ), 320, 2);
    /// <summary> Guyana </summary>
    public static readonly Currency GYD = new(nameof(GYD), 328, 2);
    /// <summary> Hong Kong </summary>
    public static readonly Currency HKD = new(nameof(HKD), 344, 2);
    /// <summary> Honduras </summary>
    public static readonly Currency HNL = new(nameof(HNL), 340, 2);
    /// <summary> Haiti </summary>
    public static readonly Currency HTG = new(nameof(HTG), 332, 2);
    /// <summary> Hungary </summary>
    public static readonly Currency HUF = new(nameof(HUF), 348, 2);
    /// <summary> Indonesia </summary>
    public static readonly Currency IDR = new(nameof(IDR), 360, 2);
    /// <summary> Israel </summary>
    public static readonly Currency ILS = new(nameof(ILS), 376, 2);
    /// <summary> India </summary>
    public static readonly Currency INR = new(nameof(INR), 356, 2);
    /// <summary> Iraq </summary>
    public static readonly Currency IQD = new(nameof(IQD), 368, 3);
    /// <summary> Iran </summary>
    public static readonly Currency IRR = new(nameof(IRR), 364, 2);
    /// <summary> Iceland </summary>
    public static readonly Currency ISK = new(nameof(ISK), 352, 0);
    /// <summary> Jamaica </summary>
    public static readonly Currency JMD = new(nameof(JMD), 388, 2);
    /// <summary> Jordan </summary>
    public static readonly Currency JOD = new(nameof(JOD), 400, 3);
    /// <summary> Japan </summary>
    public static readonly Currency JPY = new(nameof(JPY), 392, 0);
    /// <summary> Kenya </summary>
    public static readonly Currency KES = new(nameof(KES), 404, 2);
    /// <summary> Kyrgyzstan </summary>
    public static readonly Currency KGS = new(nameof(KGS), 417, 2);
    /// <summary> Cambodia </summary>
    public static readonly Currency KHR = new(nameof(KHR), 116, 2);
    /// <summary> Comoros </summary>
    public static readonly Currency KMF = new(nameof(KMF), 174, 0);
    /// <summary> North Korea </summary>
    public static readonly Currency KPW = new(nameof(KPW), 408, 2);
    /// <summary> South Korea </summary>
    public static readonly Currency KRW = new(nameof(KRW), 410, 0);
    /// <summary> Kuwait </summary>
    public static readonly Currency KWD = new(nameof(KWD), 414, 3);
    /// <summary> Cayman Islands </summary>
    public static readonly Currency KYD = new(nameof(KYD), 136, 2);
    /// <summary> Kazakhstan </summary>
    public static readonly Currency KZT = new(nameof(KZT), 398, 2);
    /// <summary> Laos </summary>
    public static readonly Currency LAK = new(nameof(LAK), 418, 2);
    /// <summary> Lebanon </summary>
    public static readonly Currency LBP = new(nameof(LBP), 422, 2);
    /// <summary> Sri Lanka </summary>
    public static readonly Currency LKR = new(nameof(LKR), 144, 2);
    /// <summary> Liberia </summary>
    public static readonly Currency LRD = new(nameof(LRD), 430, 2);
    /// <summary> Lesotho </summary>
    public static readonly Currency LSL = new(nameof(LSL), 426, 2);
    /// <summary> Libya </summary>
    public static readonly Currency LYD = new(nameof(LYD), 434, 3);
    /// <summary> Morocco </summary>
    public static readonly Currency MAD = new(nameof(MAD), 504, 2);
    /// <summary> Moldova </summary>
    public static readonly Currency MDL = new(nameof(MDL), 498, 2);
    /// <summary> Madagascar </summary>
    public static readonly Currency MGA = new(nameof(MGA), 969, 2);
    /// <summary> North Macedonia </summary>
    public static readonly Currency MKD = new(nameof(MKD), 807, 2);
    /// <summary> Myanmar </summary>
    public static readonly Currency MMK = new(nameof(MMK), 104, 2);
    /// <summary> Mongolia </summary>
    public static readonly Currency MNT = new(nameof(MNT), 496, 2);
    /// <summary> Macau </summary>
    public static readonly Currency MOP = new(nameof(MOP), 446, 2);
    /// <summary> Mauritania </summary>
    public static readonly Currency MRU = new(nameof(MRU), 929, 2);
    /// <summary> Mauritius </summary>
    public static readonly Currency MUR = new(nameof(MUR), 480, 2);
    /// <summary> Maldives </summary>
    public static readonly Currency MVR = new(nameof(MVR), 462, 2);
    /// <summary> Malawi </summary>
    public static readonly Currency MWK = new(nameof(MWK), 454, 2);
    /// <summary> Mexico </summary>
    public static readonly Currency MXN = new(nameof(MXN), 484, 2);
    /// <summary> Mexico </summary>
    public static readonly Currency MXV = new(nameof(MXV), 979, 2);
    /// <summary> Malaysia </summary>
    public static readonly Currency MYR = new(nameof(MYR), 458, 2);
    /// <summary> Mozambique </summary>
    public static readonly Currency MZN = new(nameof(MZN), 943, 2);
    /// <summary> Namibia </summary>
    public static readonly Currency NAD = new(nameof(NAD), 516, 2);
    /// <summary> Nigeria </summary>
    public static readonly Currency NGN = new(nameof(NGN), 566, 2);
    /// <summary> Nicaragua </summary>
    public static readonly Currency NIO = new(nameof(NIO), 558, 2);
    /// <summary> Norway </summary>
    public static readonly Currency NOK = new(nameof(NOK), 578, 2);
    /// <summary> Nepal </summary>
    public static readonly Currency NPR = new(nameof(NPR), 524, 2);
    /// <summary> New Zealand </summary>
    public static readonly Currency NZD = new(nameof(NZD), 554, 2);
    /// <summary> Oman </summary>
    public static readonly Currency OMR = new(nameof(OMR), 512, 3);
    /// <summary> Panama </summary>
    public static readonly Currency PAB = new(nameof(PAB), 590, 2);
    /// <summary> Peru </summary>
    public static readonly Currency PEN = new(nameof(PEN), 604, 2);
    /// <summary> Papua New Guinea </summary>
    public static readonly Currency PGK = new(nameof(PGK), 598, 2);
    /// <summary> Philippines </summary>
    public static readonly Currency PHP = new(nameof(PHP), 608, 2);
    /// <summary> Pakistan </summary>
    public static readonly Currency PKR = new(nameof(PKR), 586, 2);
    /// <summary> Poland </summary>
    public static readonly Currency PLN = new(nameof(PLN), 985, 2);
    /// <summary> Paraguay </summary>
    public static readonly Currency PYG = new(nameof(PYG), 600, 0);
    /// <summary> Qatar </summary>
    public static readonly Currency QAR = new(nameof(QAR), 634, 2);
    /// <summary> Romania </summary>
    public static readonly Currency RON = new(nameof(RON), 946, 2);
    /// <summary> Serbia </summary>
    public static readonly Currency RSD = new(nameof(RSD), 941, 2);
    /// <summary> Russia </summary>
    public static readonly Currency RUB = new(nameof(RUB), 643, 2);
    /// <summary> Rwanda </summary>
    public static readonly Currency RWF = new(nameof(RWF), 646, 0);
    /// <summary> Saudi Arabia </summary>
    public static readonly Currency SAR = new(nameof(SAR), 682, 2);
    /// <summary> Solomon Islands </summary>
    public static readonly Currency SBD = new(nameof(SBD), 090, 2);
    /// <summary> Seychelles </summary>
    public static readonly Currency SCR = new(nameof(SCR), 690, 2);
    /// <summary> Sudan </summary>
    public static readonly Currency SDG = new(nameof(SDG), 938, 2);
    /// <summary> Sweden </summary>
    public static readonly Currency SEK = new(nameof(SEK), 752, 2);
    /// <summary> Singapore </summary>
    public static readonly Currency SGD = new(nameof(SGD), 702, 2);
    /// <summary> Saint Helena </summary>
    public static readonly Currency SHP = new(nameof(SHP), 654, 2);
    /// <summary> Sierra Leone </summary>
    public static readonly Currency SLE = new(nameof(SLE), 925, 2);
    /// <summary> Somalia </summary>
    public static readonly Currency SOS = new(nameof(SOS), 706, 2);
    /// <summary> Suriname </summary>
    public static readonly Currency SRD = new(nameof(SRD), 968, 2);
    /// <summary> South Sudan </summary>
    public static readonly Currency SSP = new(nameof(SSP), 728, 2);
    /// <summary> Sao Tome and Principe </summary>
    public static readonly Currency STN = new(nameof(STN), 930, 2);
    /// <summary> El Salvador </summary>
    public static readonly Currency SVC = new(nameof(SVC), 222, 2);
    /// <summary> Syria </summary>
    public static readonly Currency SYP = new(nameof(SYP), 760, 2);
    /// <summary> Eswatini </summary>
    public static readonly Currency SZL = new(nameof(SZL), 748, 2);
    /// <summary> Thailand </summary>
    public static readonly Currency THB = new(nameof(THB), 764, 2);
    /// <summary> Tajikistan </summary>
    public static readonly Currency TJS = new(nameof(TJS), 972, 2);
    /// <summary> Turkmenistan </summary>
    public static readonly Currency TMT = new(nameof(TMT), 934, 2);
    /// <summary> Tunisia </summary>
    public static readonly Currency TND = new(nameof(TND), 788, 3);
    /// <summary> Tonga </summary>
    public static readonly Currency TOP = new(nameof(TOP), 776, 2);
    /// <summary> Turkey </summary>
    public static readonly Currency TRY = new(nameof(TRY), 949, 2);
    /// <summary> Trinidad and Tobago </summary>
    public static readonly Currency TTD = new(nameof(TTD), 780, 2);
    /// <summary> Taiwan </summary>
    public static readonly Currency TWD = new(nameof(TWD), 901, 2);
    /// <summary> Tanzania </summary>
    public static readonly Currency TZS = new(nameof(TZS), 834, 2);
    /// <summary> Ukraine </summary>
    public static readonly Currency UAH = new(nameof(UAH), 980, 2);
    /// <summary> Uganda </summary>
    public static readonly Currency UGX = new(nameof(UGX), 800, 0);
    /// <summary> United States </summary>
    public static readonly Currency USD = new(nameof(USD), 840, 2);
    /// <summary> United States </summary>
    public static readonly Currency USN = new(nameof(USN), 997, 2);
    /// <summary> Uruguay </summary>
    public static readonly Currency UYI = new(nameof(UYI), 940, 0);
    /// <summary> Uruguay </summary>
    public static readonly Currency UYU = new(nameof(UYU), 858, 2);
    /// <summary> Uruguay </summary>
    public static readonly Currency UYW = new(nameof(UYW), 927, 4);
    /// <summary> Uzbekistan </summary>
    public static readonly Currency UZS = new(nameof(UZS), 860, 2);
    /// <summary> Venezuela </summary>
    public static readonly Currency VED = new(nameof(VED), 926, 2);
    /// <summary> Venezuela </summary>
    public static readonly Currency VES = new(nameof(VES), 928, 2);
    /// <summary> Vietnam </summary>
    public static readonly Currency VND = new(nameof(VND), 704, 0);
    /// <summary> Vanuatu </summary>
    public static readonly Currency VUV = new(nameof(VUV), 548, 0);
    /// <summary> Samoa </summary>
    public static readonly Currency WST = new(nameof(WST), 882, 2);
    /// <summary> Arab Monetary Fund </summary>
    public static readonly Currency XAD = new(nameof(XAD), 396, 2);
    /// <summary> Cameroon </summary>
    public static readonly Currency XAF = new(nameof(XAF), 950, 0);
    /// <summary> Anguilla </summary>
    public static readonly Currency XCD = new(nameof(XCD), 951, 2);
    /// <summary> Curacao </summary>
    public static readonly Currency XCG = new(nameof(XCG), 532, 2);
    /// <summary> Benin </summary>
    public static readonly Currency XOF = new(nameof(XOF), 952, 0);
    /// <summary> French Polynesia </summary>
    public static readonly Currency XPF = new(nameof(XPF), 953, 0);
    /// <summary> Yemen </summary>
    public static readonly Currency YER = new(nameof(YER), 886, 2);
    /// <summary> South Africa </summary>
    public static readonly Currency ZAR = new(nameof(ZAR), 710, 2);
    /// <summary> Zambia </summary>
    public static readonly Currency ZMW = new(nameof(ZMW), 967, 2);
    /// <summary> Zimbabwe </summary>
    public static readonly Currency ZWG = new(nameof(ZWG), 924, 2);
}
