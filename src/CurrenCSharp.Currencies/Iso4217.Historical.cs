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
        /// <summary> Andorran peseta (Andorra) </summary>
        public static readonly Currency ADP = new(nameof(ADP), 020, 0);
        /// <summary> Afghan afghani (Afghanistan, first series) </summary>
        public static readonly Currency AFA = new(nameof(AFA), 004, 2);
        /// <summary> Austrian schilling (Austria) </summary>
        public static readonly Currency ATS = new(nameof(ATS), 040, 2);
        /// <summary> Azerbaijani manat (Azerbaijan, first series) </summary>
        public static readonly Currency AZM = new(nameof(AZM), 031, 2);
        /// <summary> Belgian franc (Belgium) </summary>
        public static readonly Currency BEF = new(nameof(BEF), 056, 0);
        /// <summary> Bulgarian lev (Bulgaria) </summary>
        public static readonly Currency BGN = new(nameof(BGN), 975, 2);
        /// <summary> Belarusian ruble (Belarus, 1992-1999) </summary>
        public static readonly Currency BYB = new(nameof(BYB), 112, 0);
        /// <summary> Belarusian ruble (Belarus, 2000-2016) </summary>
        public static readonly Currency BYR = new(nameof(BYR), 974, 0);
        /// <summary> Cuban convertible peso (Cuba) </summary>
        public static readonly Currency CUC = new(nameof(CUC), 931, 2);
        /// <summary> Cypriot pound (Cyprus) </summary>
        public static readonly Currency CYP = new(nameof(CYP), 196, 2);
        /// <summary> Deutsche Mark (Germany) </summary>
        public static readonly Currency DEM = new(nameof(DEM), 276, 2);
        /// <summary> Estonian kroon (Estonia) </summary>
        public static readonly Currency EEK = new(nameof(EEK), 233, 2);
        /// <summary> Spanish peseta (Spain) </summary>
        public static readonly Currency ESP = new(nameof(ESP), 724, 0);
        /// <summary> Finnish markka (Finland) </summary>
        public static readonly Currency FIM = new(nameof(FIM), 246, 2);
        /// <summary> French franc (France) </summary>
        public static readonly Currency FRF = new(nameof(FRF), 250, 2);
        /// <summary> Ghanaian cedi (Ghana, first series) </summary>
        public static readonly Currency GHC = new(nameof(GHC), 288, 2);
        /// <summary> Greek drachma (Greece) </summary>
        public static readonly Currency GRD = new(nameof(GRD), 300, 0);
        /// <summary> Guinea-Bissau peso (Guinea-Bissau) </summary>
        public static readonly Currency GWP = new(nameof(GWP), 624, 2);
        /// <summary> Irish pound (Ireland) </summary>
        public static readonly Currency IEP = new(nameof(IEP), 372, 2);
        /// <summary> Italian lira (Italy) </summary>
        public static readonly Currency ITL = new(nameof(ITL), 380, 0);
        /// <summary> Lithuanian litas (Lithuania) </summary>
        public static readonly Currency LTL = new(nameof(LTL), 440, 2);
        /// <summary> Luxembourg franc (Luxembourg) </summary>
        public static readonly Currency LUF = new(nameof(LUF), 442, 0);
        /// <summary> Latvian lats (Latvia) </summary>
        public static readonly Currency LVL = new(nameof(LVL), 428, 2);
        /// <summary> Malagasy franc (Madagascar) </summary>
        public static readonly Currency MGF = new(nameof(MGF), 450, 0);
        /// <summary> Mauritanian ouguiya (Mauritania, first series) </summary>
        public static readonly Currency MRO = new(nameof(MRO), 478, 2);
        /// <summary> Maltese lira (Malta) </summary>
        public static readonly Currency MTL = new(nameof(MTL), 470, 2);
        /// <summary> Mozambican metical (Mozambique, first series) </summary>
        public static readonly Currency MZM = new(nameof(MZM), 508, 2);
        /// <summary> Dutch guilder (Netherlands) </summary>
        public static readonly Currency NLG = new(nameof(NLG), 528, 2);
        /// <summary> Portuguese escudo (Portugal) </summary>
        public static readonly Currency PTE = new(nameof(PTE), 620, 0);
        /// <summary> Romanian leu (Romania, first series) </summary>
        public static readonly Currency ROL = new(nameof(ROL), 642, 0);
        /// <summary> Slovenian tolar (Slovenia) </summary>
        public static readonly Currency SIT = new(nameof(SIT), 705, 2);
        /// <summary> Slovak koruna (Slovakia) </summary>
        public static readonly Currency SKK = new(nameof(SKK), 703, 2);
        /// <summary> Surinamese guilder (Suriname) </summary>
        public static readonly Currency SRG = new(nameof(SRG), 740, 2);
        /// <summary> Sao Tome and Principe dobra (Sao Tome and Principe, first series) </summary>
        public static readonly Currency STD = new(nameof(STD), 678, 2);
        /// <summary> Turkmenistani manat (Turkmenistan, first series) </summary>
        public static readonly Currency TMM = new(nameof(TMM), 795, 2);
        /// <summary> Timorese escudo (Timor-Leste) </summary>
        public static readonly Currency TPE = new(nameof(TPE), 626, 0);
        /// <summary> Turkish lira (Turkey, first series) </summary>
        public static readonly Currency TRL = new(nameof(TRL), 792, 0);
        /// <summary> Venezuelan bolivar (Venezuela) </summary>
        public static readonly Currency VEB = new(nameof(VEB), 862, 2);
        /// <summary> Venezuelan bolivar fuerte (Venezuela) </summary>
        public static readonly Currency VEF = new(nameof(VEF), 937, 2);
        /// <summary> Zambian kwacha (Zambia, first series) </summary>
        public static readonly Currency ZMK = new(nameof(ZMK), 894, 2);
        /// <summary> Zimbabwean dollar (Zimbabwe, 2006-2008) </summary>
        public static readonly Currency ZWN = new(nameof(ZWN), 942, 2);
        /// <summary> Zimbabwean dollar (Zimbabwe, 2008-2009) </summary>
        public static readonly Currency ZWR = new(nameof(ZWR), 935, 2);
    }
}
