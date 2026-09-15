# Historische ISO-4217-Waehrungen: Datenabgleich

Stand: 2026-09-14

## Quellen und Methode

- Kandidaten: Wikipedia, Revision `269389849`, Tabelle "Fruehere Waehrungen".
- Historische Codes und Nummern: SIX List Three, Stand `2026-01-01`.
- Aktueller Bestand: SIX List One, Stand `2026-01-01`, und die bestehenden
  Felder in `Iso4217.cs`.
- Minor Units: OpenJDK `CurrencyData.properties`, ISO-4217-Datenversion 180.
  Die Datei bezeichnet ihre Eintraege als Liste aller gueltigen ISO-4217-Codes,
  nennt alle von zwei abweichenden Exponenten und legt fuer die uebrigen Codes
  zwei Dezimalstellen fest.

Veraenderliche Quellen wurden nur fuer den manuellen Abgleich verwendet. Build
und Tests greifen nicht auf das Netzwerk zu.

Quellen:

- <https://de.wikipedia.org/w/index.php?title=ISO_4217&oldid=269389849>
- <https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-three.xml>
- <https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml>
- <https://github.com/openjdk/jdk/blob/85f5bf3f415cc3d44d1618ec574e73f846bb91c4/src/java.base/share/data/currency/CurrencyData.properties>

Die Wikipedia/SIX-Schnittmenge umfasst 103 Alpha-Codes. Numerische Kollisionen
wurden gegen alle 103 Kandidaten und den aktuellen Katalog ermittelt, bevor
Eintraege wegen unbekannter Minor Units ausgeschlossen wurden.

## Freigegeben

Diese 42 Tupel sind numerisch eindeutig und mit einem ISO-Minor-Unit-Exponenten
im OpenJDK-Datenbestand belegt:

| Alpha | Numeric | Minor Units | Alpha | Numeric | Minor Units |
|---|---:|---:|---|---:|---:|
| ADP | 020 | 0 | AFA | 004 | 2 |
| ATS | 040 | 2 | AZM | 031 | 2 |
| BEF | 056 | 0 | BGN | 975 | 2 |
| BYB | 112 | 0 | BYR | 974 | 0 |
| CUC | 931 | 2 | CYP | 196 | 2 |
| DEM | 276 | 2 | EEK | 233 | 2 |
| ESP | 724 | 0 | FIM | 246 | 2 |
| FRF | 250 | 2 | GHC | 288 | 2 |
| GRD | 300 | 0 | GWP | 624 | 2 |
| IEP | 372 | 2 | ITL | 380 | 0 |
| LTL | 440 | 2 | LUF | 442 | 0 |
| LVL | 428 | 2 | MGF | 450 | 0 |
| MRO | 478 | 2 | MTL | 470 | 2 |
| MZM | 508 | 2 | NLG | 528 | 2 |
| PTE | 620 | 0 | ROL | 642 | 0 |
| SIT | 705 | 2 | SKK | 703 | 2 |
| SRG | 740 | 2 | STD | 678 | 2 |
| TMM | 795 | 2 | TPE | 626 | 0 |
| TRL | 792 | 0 | VEB | 862 | 2 |
| VEF | 937 | 2 | ZMK | 894 | 2 |
| ZWN | 942 | 2 | ZWR | 935 | 2 |

Diese 42 Tupel sind als oeffentliche Felder der Klasse `Iso4217.Historical`
verfuegbar. Der Laendername im XML-Kommentar jedes Feldes stammt aus der
SIX List Three (Spalte "Entity"); wo ein Land mehrere Codes hat, ergaenzt der
Kommentar den Waehrungsnamen oder den Zeitraum zur Unterscheidung.

Zwei Eintraege verdienen einen Hinweis, weil sie in aelteren Katalogen noch als
aktuell gefuehrt werden. Die SIX List Three (Stand `2026-01-01`) nennt fuer
`BGN` (Bulgaria, Bulgarian Lev) das Rueckzugsdatum `2026-01-01` und fuer `CUC`
(Cuba, Peso Convertible) das Rueckzugsdatum `2021-06`. Beide fehlen in der
List One desselben Stands.

Der ISO-Exponent kann von der nominellen Unterteilung abweichen. Beispielsweise
nennt Wikipedia fuer `ADP`, `BEF`, `ESP`, `ITL`, `LUF` und `PTE` Untereinheiten,
waehrend der ISO-Datenbestand einen Exponenten von 0 festlegt. Deshalb wurde die
nominelle Unterteilung nicht in einen Exponenten umgerechnet.

## Ausgeschlossen: numerische Kollision

Die folgenden 44 Codes sind ausgeschlossen. Codes einer Zeile teilen denselben
Numeric-Code; aktuelle Konfliktpartner sind ebenfalls angegeben:

| Numeric | Codes |
|---:|---|
| 008 | ALK, ALL |
| 024 | AOK, AON |
| 032 | ARA, ARP, ARS |
| 068 | BOP, BOB |
| 076 | BRB, BRC, BRE, BRN |
| 100 | BGJ, BGK, BGL |
| 191 | HRD, HRK |
| 203 | CSJ, CZK |
| 324 | GNE, GNF |
| 352 | ISJ, ISK |
| 376 | ILP, ILR, ILS |
| 418 | LAJ, LAK |
| 462 | MVQ, MVR |
| 484 | MXP, MXN |
| 558 | NIC, NIO |
| 604 | PEH, PEI, PES, PEN |
| 704 | VNC, VND |
| 716 | ZWC, ZWD |
| 736 | SDD, SDP |
| 800 | UGS, UGW, UGX |
| 810 | RUR, SUR |
| 858 | UYN, UYP, UYU |
| 890 | YUD, YUN |
| 891 | CSD, YUM |
| 180 | ZRN, ZRZ |

## Ausgeschlossen: nicht darstellbar oder unbelegt

- `XFO`: SIX weist keinen Numeric-Code aus.
- `AOR`, `BAD`, `BRR`, `CSK`, `DDM`, `ECS`, `ECV`, `ESA`, `ESB`, `GHP`,
  `GQE`, `MLF`, `PLZ`, `XEU`, `YDD`, `ZAL`: numerisch eindeutig, aber nicht
  in der verwendeten OpenJDK-Freigabeliste enthalten. Die nominellen Angaben
  aus Wikipedia reichen nicht als Beleg des ISO-Exponenten; diese 16 Codes
  bleiben bis zu einem belastbaren historischen ISO-Beleg zurueckgestellt.

Damit gilt: 42 freigegeben + 44 Kollisionen + 1 ohne Numeric-Code + 16 ohne
belegten Minor-Unit-Exponenten = 103 Kandidaten.
