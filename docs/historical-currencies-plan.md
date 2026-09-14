# Plan: Historische ISO-4217-Waehrungen

Status: Umgesetzt. Der verifizierte Datenbestand ist in
[`historical-currencies-data.md`](historical-currencies-data.md) dokumentiert.

## Ziel

Historische Geldbetraege, beispielsweise Restanten mit DEM-Waehrung, sollen
ueber dieselbe Schnittstelle wie aktuelle Waehrungen verarbeitet werden.
Aufrufer muessen nicht wissen, ob eine Waehrung aktuell oder historisch ist.

```csharp
Currency deutscheMarkByAlpha = Iso4217.FindByAlphaCode("DEM");
Currency deutscheMarkByNumeric = Iso4217.FindByNumericCode(276);
Money restant = new(100m, deutscheMarkByAlpha);
```

Beide Lookups liefern dieselbe Currency-Instanz.

## Festgelegte Entscheidungen

- Die bestehenden Methoden `Iso4217.FindByAlphaCode(...)` und
  `Iso4217.FindByNumericCode(...)` finden aktuelle und aufgenommene historische
  Waehrungen.
- Es gibt keinen separaten oeffentlichen historischen Katalog und keine neue
  Methode `FindByIsoCode(...)`.
- Historische Waehrungen werden nur ueber Lookup angeboten, nicht als neue
  oeffentliche Felder wie `Iso4217.DEM`.
- Historische Waehrungen mit mehrdeutigen numerischen Codes werden vorerst
  nicht aufgenommen, auch nicht ausschliesslich ueber den Alpha-Lookup.
- Der Datenumfang ist die Schnittmenge aus dem Wikipedia-Abschnitt
  "Fruehere Waehrungen" und der offiziellen historischen SIX-Liste.
- Minor Units werden nicht geraten oder pauschal mit 0 beziehungsweise 2
  vorbelegt.
- Das bestehende Currency-Modell und die Geldarithmetik bleiben unveraendert.

## Quellen und Datenstand

- Wikipedia, ISO 4217, Abschnitt "Fruehere Waehrungen":
  <https://de.wikipedia.org/w/index.php?title=ISO_4217&oldid=269389849>
- SIX List Three, historische Waehrungen und Fonds, untersuchter
  Veroeffentlichungsstand `2026-01-01`:
  <https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-three.xml>
- SIX List One, aktuelle Waehrungen und Fonds, untersuchter
  Veroeffentlichungsstand `2026-01-01`:
  <https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml>
- SIX Maintenance Agency und historische Amendments:
  <https://www.six-group.com/en/products-services/financial-information/market-reference-data/data-standards.html>

Die SIX-URLs liefern veraenderliche Daten. Fuer den spaeteren Abgleich sind
Veroeffentlichungsstand und verwendete Quellen festzuhalten. Tests duerfen
keinen Live-Abruf voraussetzen.

Wikipedia ist eine Kandidatenquelle, nicht die verbindliche Quelle fuer
numerische Codes oder Minor Units. Die untersuchte List Three enthaelt keine
Minor-Units-Angaben; diese muessen aus historischen Amendments oder anderen
belastbaren Quellen ergaenzt werden.

Die bisherigen Arbeitszahlen (103 Codes in der Schnittmenge, davon 58
grundsaetzlich numerisch eindeutige Kandidaten und 21 Kandidaten mit weiterem
Minor-Units-Pruefbedarf) sind vorlaeufig. Die exakte Freigabeliste und ihre
Anzahl werden im Datenabgleich erneut ermittelt, nicht aus diesen Zahlen
abgeleitet.

## Umsetzung

### 1. Datenbestand verifizieren

- Alpha-Codes aus beiden Quellen extrahieren und ihre Schnittmenge bilden.
- Wiederholte Laender- oder Namenseintraege derselben Waehrung zusammenfassen;
  widerspruechliche Eintraege separat pruefen.
- Fuer jeden Kandidaten Alpha-Code, numerischen Code, Minor Units und
  Quellenbelege erfassen.
- Den Bestand mit den vorhandenen Deklarationen in `Iso4217.cs` abgleichen.
- Historische List-Three-Eintraege nicht automatisch als weltweit ungueltige
  Codes behandeln: Die Liste enthaelt auch fruehere Laenderzuordnungen und
  Waehrungsnamen noch aktueller Codes.
- `BGN` ist im untersuchten Checkout nicht als Feld vorhanden und muss als
  historischer Kandidat geprueft werden. Die fruehere Aussage im Chat, es sei
  bereits vorhanden, war falsch.

### 2. Eindeutigkeit und Darstellbarkeit pruefen

- Numerische Kollisionen vor dem Herausfiltern ungeklaerter Minor Units
  ermitteln. Das Weglassen eines Konfliktpartners darf keine scheinbare
  Eindeutigkeit erzeugen.
- Fuer die Pruefung den aktuellen Katalog und alle historischen Kandidaten
  der festgelegten Schnittmenge betrachten.
- Nur historische Kandidaten aufnehmen, deren Alpha- und Numeric-Code im
  kombinierten Bestand eindeutig zuordenbar sind.
- Historische Kandidaten mit fehlendem Numeric-Code ausschliessen, etwa
  `XFO`. Keine Ersatznummer erfinden.
- Numerisch mehrdeutige historische Kandidaten vorerst ausschliessen,
  beispielsweise `PEH`/`PEI`/`PES` gegenueber `PEN` (604), `ALK` gegenueber
  `ALL` (008), `AOK`/`AON` (024) und `RUR`/`SUR` (810).
- Bei numerischer Kollision bestehende aktuelle Definitionen und ihr
  Lookup-Verhalten unveraendert lassen.
- Minor Units anhand belastbarer Quellen belegen. Die nominelle Unterteilung
  einer Waehrung ist nicht automatisch ihr ISO-Dezimalexponent.
- Unbekannte, widerspruechliche oder zeitlich wechselnde Minor Units vorerst
  zurueckstellen. Nicht dezimale Unterteilungen nicht durch einen geratenen
  Dezimalexponenten ersetzen.
- Eine nachvollziehbare Freigabe- und Ausschlussliste mit Gruenden erstellen.

### 3. Historische Daten intern hinterlegen

- Eine interne Datendatei unter
  `src/CurrenCSharp.Currencies/Iso4217.HistoricalData.cs` anlegen.
- Darin die freigegebenen Currency-Instanzen alphabetisch geordnet und ohne
  neue oeffentliche Felder bereitstellen.
- Die Sammlung intern unveraenderlich halten und jede Waehrung nur einmal
  erzeugen.
- Datenherkunft und besondere Entscheidungen dokumentieren.
- Zunaechst die vorhandene manuelle Katalogpflege beibehalten; keinen
  Source-Generator oder Build-Download einfuehren.

### 4. Gemeinsamen Cache erweitern

- In `Iso4217Cache.CreateCache()` die bisherigen, per Reflection geladenen
  oeffentlichen Waehrungsfelder um die internen historischen Eintraege
  ergaenzen.
- Aus diesem kombinierten Bestand weiterhin genau einen Alpha-Index und
  einen Numeric-Index aufbauen.
- Lazy-Initialisierung, Thread-Sicherheit und Referenzidentitaet erhalten.
- Keine getrennten oeffentlichen Lookups, Mehrfachergebnisse, Datumsparameter
  oder Prioritaetsregeln einfuehren.
- Signaturen, Nullbehandlung und Exception-Typen der bestehenden Methoden
  unveraendert lassen. XML-Dokumentation auf den erweiterten Katalog beziehen.
- Den Dictionary-Aufbau weiterhin auf doppelte Schluessel pruefen lassen;
  Konflikte nicht stillschweigend mit "first wins" verdecken.

### 5. Tests erweitern

Testkonventionen aus `docs/testing.md` beachten. Historische Tests koennen
passend zur Produktionsstruktur in `Iso4217Tests.Historical.cs` liegen;
dafuer die bestehende Testklasse bei Bedarf `partial` machen.

- Alpha-Lookup fuer historische Codes, insbesondere `DEM`, testen.
- Numeric-Lookup fuer historische Codes, insbesondere `276`, testen.
- Fuer beide Lookups dieselbe Instanz erwarten.
- Alle freigegebenen Tupel aus Alpha-Code, Numeric-Code und Minor Units mit
  datengetriebenen Tests pruefen.
- Exakte freigegebene Code-Menge pruefen, nicht nur eine Gesamtzahl.
- Eindeutigkeit des kombinierten Bestands pruefen. Die bisherige Reflection
  ueber oeffentliche Felder allein deckt historische Eintraege nicht ab.
- Ausgeschlossene Alpha-Codes wie `PES` muessen weiterhin
  `InvalidOperationException` ausloesen.
- Bestehende aktuelle Lookups absichern, beispielsweise `604 -> PEN`.
- Unbekannte Codes, Nullargumente und parallele Zugriffe weiterhin testen.
- Eine kleine Integrationsprobe mit `Money` in DEM fuer Konstruktion und
  Formatierung ergaenzen.
- Quellenabgleich und Tests deterministisch und ohne Netzwerk ausfuehren.

### 6. Dokumentation und Beispiel aktualisieren

- In `README.md` aktuelle und ausgewaehlte historische Waehrungen als
  gemeinsamen Lookup-Bestand beschreiben.
- Die bisherige pauschale Aussage "all ISO 4217 codes" an den tatsaechlich
  unterstuetzten Umfang anpassen.
- Ausschlussregeln, Datenstand und die fehlende vollstaendige historische
  Abdeckung dokumentieren.
- `example/CurrenCSharp.Example/Program.cs` um das DEM-Lookup-Beispiel
  ergaenzen.
- Klarstellen: Ein historischer Katalogeintrag stellt keine Wechselkurse
  bereit. Umrechnungen erfolgen weiterhin explizit ueber einen Kontext und
  einen vom Aufrufer bereitgestellten Exchange-Rate-Provider.

### 7. Abschlusspruefung

```bash
dotnet build
dotnet test
```

- Build ohne Warnungen und Tests auf net8.0, net9.0 und net10.0 erfolgreich.
- Den `code-reviewer`-Skill auf dem gesamten Implementierungsdiff ausfuehren.
- Keine ungeprueften Minor Units oder unaufgeloesten Kollisionen im
  freigegebenen Bestand.
- Keine Aenderungen an bestehenden oeffentlichen Waehrungsfeldern und keine
  fachfremden Refactorings.
- Kein Commit, Tag oder Push ohne ausdruecklichen Auftrag.

## Nicht Teil dieser Erweiterung

- Vollstaendige SIX-List-Three-Abdeckung ausserhalb der gewaehlten
  Wikipedia/SIX-Schnittmenge.
- Historische Waehrungen ohne eindeutigen numerischen Code.
- Datumsabhaengige Waehrungsidentitaet oder Minor Units.
- Neue oeffentliche historische Waehrungsfelder.
- Automatische Waehrungsumstellungen oder eingebaute historische Kurse.
- Aenderungen an Currency-Gleichheit, Hashing oder Geldarithmetik.
