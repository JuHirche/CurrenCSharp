namespace CurrenCSharp.Currencies;

public static partial class Iso4217
{
    /// <summary>
    /// Historical ISO 4217 currencies that are no longer in circulation. They are kept apart
    /// from the current currencies on purpose. Each field is the same instance that
    /// <see cref="FindByAlphaCode"/> and <see cref="FindByNumericCode"/> return.
    /// See docs/historical-currencies-data.md for the data sources and decisions.
    /// </summary>
    public static class Historical
    {
        /// <summary> Andorra (Andorran peseta) </summary>
        public static readonly Currency ADP = new(nameof(ADP), 020, 0);
        /// <summary> Afghanistan (Afghan afghani, first series) </summary>
        public static readonly Currency AFA = new(nameof(AFA), 004, 2);
        /// <summary> Austria (Austrian schilling) </summary>
        public static readonly Currency ATS = new(nameof(ATS), 040, 2);
        /// <summary> Azerbaijan (Azerbaijani manat, first series) </summary>
        public static readonly Currency AZM = new(nameof(AZM), 031, 2);
        /// <summary> Belgium (Belgian franc) </summary>
        public static readonly Currency BEF = new(nameof(BEF), 056, 0);
        /// <summary> Bulgaria (Bulgarian lev) </summary>
        public static readonly Currency BGN = new(nameof(BGN), 975, 2);
        /// <summary> Belarus (Belarusian ruble, 1992-1999) </summary>
        public static readonly Currency BYB = new(nameof(BYB), 112, 0);
        /// <summary> Belarus (Belarusian ruble, 2000-2016) </summary>
        public static readonly Currency BYR = new(nameof(BYR), 974, 0);
        /// <summary> Cuba (Cuban convertible peso) </summary>
        public static readonly Currency CUC = new(nameof(CUC), 931, 2);
        /// <summary> Cyprus (Cypriot pound) </summary>
        public static readonly Currency CYP = new(nameof(CYP), 196, 2);
        /// <summary> Germany (Deutsche Mark) </summary>
        public static readonly Currency DEM = new(nameof(DEM), 276, 2);
        /// <summary> Estonia (Estonian kroon) </summary>
        public static readonly Currency EEK = new(nameof(EEK), 233, 2);
        /// <summary> Spain (Spanish peseta) </summary>
        public static readonly Currency ESP = new(nameof(ESP), 724, 0);
        /// <summary> Finland (Finnish markka) </summary>
        public static readonly Currency FIM = new(nameof(FIM), 246, 2);
        /// <summary> France (French franc) </summary>
        public static readonly Currency FRF = new(nameof(FRF), 250, 2);
        /// <summary> Ghana (Ghanaian cedi, first series) </summary>
        public static readonly Currency GHC = new(nameof(GHC), 288, 2);
        /// <summary> Greece (Greek drachma) </summary>
        public static readonly Currency GRD = new(nameof(GRD), 300, 0);
        /// <summary> Guinea-Bissau (Guinea-Bissau peso) </summary>
        public static readonly Currency GWP = new(nameof(GWP), 624, 2);
        /// <summary> Ireland (Irish pound) </summary>
        public static readonly Currency IEP = new(nameof(IEP), 372, 2);
        /// <summary> Italy (Italian lira) </summary>
        public static readonly Currency ITL = new(nameof(ITL), 380, 0);
        /// <summary> Lithuania (Lithuanian litas) </summary>
        public static readonly Currency LTL = new(nameof(LTL), 440, 2);
        /// <summary> Luxembourg (Luxembourg franc) </summary>
        public static readonly Currency LUF = new(nameof(LUF), 442, 0);
        /// <summary> Latvia (Latvian lats) </summary>
        public static readonly Currency LVL = new(nameof(LVL), 428, 2);
        /// <summary> Madagascar (Malagasy franc) </summary>
        public static readonly Currency MGF = new(nameof(MGF), 450, 0);
        /// <summary> Mauritania (Mauritanian ouguiya, first series) </summary>
        public static readonly Currency MRO = new(nameof(MRO), 478, 2);
        /// <summary> Malta (Maltese lira) </summary>
        public static readonly Currency MTL = new(nameof(MTL), 470, 2);
        /// <summary> Mozambique (Mozambican metical, first series) </summary>
        public static readonly Currency MZM = new(nameof(MZM), 508, 2);
        /// <summary> Netherlands (Dutch guilder) </summary>
        public static readonly Currency NLG = new(nameof(NLG), 528, 2);
        /// <summary> Portugal (Portuguese escudo) </summary>
        public static readonly Currency PTE = new(nameof(PTE), 620, 0);
        /// <summary> Romania (Romanian leu, first series) </summary>
        public static readonly Currency ROL = new(nameof(ROL), 642, 0);
        /// <summary> Slovenia (Slovenian tolar) </summary>
        public static readonly Currency SIT = new(nameof(SIT), 705, 2);
        /// <summary> Slovakia (Slovak koruna) </summary>
        public static readonly Currency SKK = new(nameof(SKK), 703, 2);
        /// <summary> Suriname (Surinamese guilder) </summary>
        public static readonly Currency SRG = new(nameof(SRG), 740, 2);
        /// <summary> Sao Tome and Principe (dobra, first series) </summary>
        public static readonly Currency STD = new(nameof(STD), 678, 2);
        /// <summary> Turkmenistan (Turkmenistani manat, first series) </summary>
        public static readonly Currency TMM = new(nameof(TMM), 795, 2);
        /// <summary> Timor-Leste (Timorese escudo) </summary>
        public static readonly Currency TPE = new(nameof(TPE), 626, 0);
        /// <summary> Turkey (Turkish lira, first series) </summary>
        public static readonly Currency TRL = new(nameof(TRL), 792, 0);
        /// <summary> Venezuela (Venezuelan bolivar) </summary>
        public static readonly Currency VEB = new(nameof(VEB), 862, 2);
        /// <summary> Venezuela (Venezuelan bolivar fuerte) </summary>
        public static readonly Currency VEF = new(nameof(VEF), 937, 2);
        /// <summary> Zambia (Zambian kwacha, first series) </summary>
        public static readonly Currency ZMK = new(nameof(ZMK), 894, 2);
        /// <summary> Zimbabwe (Zimbabwean dollar, 2006-2008) </summary>
        public static readonly Currency ZWN = new(nameof(ZWN), 942, 2);
        /// <summary> Zimbabwe (Zimbabwean dollar, 2008-2009) </summary>
        public static readonly Currency ZWR = new(nameof(ZWR), 935, 2);
    }
}
