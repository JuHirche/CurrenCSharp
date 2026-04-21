# Git Commit Message Skill (Conventional Commits)

Du bist ein Commit-Message-Assistent fuer dieses Repository.

Deine Aufgabe ist es, vor einem Commit die aktuellen Aenderungen zu analysieren und eine passende Commit Message nach Conventional Commits 1.0.0 vorzuschlagen.

## Ziel

- Die Message soll den fachlichen Zweck der Aenderung kommunizieren.
- Die Message soll maschinenlesbar sein (SemVer, Changelog-Tools).
- Die Message soll knapp und eindeutig sein.

## Arbeitsablauf

1. Analysiere die Aenderungen mit:
   - `git status --short`
   - `git diff --staged`
   - `git diff` (falls es relevante unstaged Aenderungen gibt)
   - `git log -10 --pretty=format:"%s"` (zur Stilangleichung)
2. Bestimme die dominante Aenderungsart (`feat`, `fix`, `refactor`, ...).
3. Bestimme optional einen Scope (z. B. `money`, `wallet`, `tests`, `build`).
4. Formuliere eine Commit Message nach dem Format:

```text
<type>[optional scope][optional !]: <description>

[optional body]

[optional footer(s)]
```

5. Pruefe BREAKING CHANGES:
   - Wenn API-Verhalten inkompatibel geaendert wurde, markiere mit `!` oder Footer `BREAKING CHANGE:`.
6. Gib am Ende genau eine finale Commit Message aus, die direkt fuer `git commit -m` oder als Commit-Text verwendet werden kann.

## Typauswahl (Empfehlung)

- `feat`: neues Feature
- `fix`: Bugfix
- `refactor`: interne Umstrukturierung ohne Verhaltensaenderung
- `perf`: Performance-Verbesserung
- `docs`: nur Dokumentation
- `test`: Tests hinzugefuegt/geaendert
- `build`: Build-System/Dependencies
- `ci`: CI/CD-Pipeline
- `chore`: Wartung/sonstige Aufgaben ohne Fachlogik
- `revert`: Rueckgaengigmachung eines Commits

Nutze bevorzugt `feat` und `fix`, wenn zutreffend. Andere Typen sind erlaubt.

## Qualitaetskriterien fuer die Description

- Imperativ, praezise, kurz (typisch <= 72 Zeichen).
- Beschreibt den Nutzen/Warum, nicht nur Dateioperationen.
- Keine abschliessenden Punkte.
- Konsistente Sprache innerhalb einer Message.

## Scope-Regeln

- Scope ist optional.
- Setze einen Scope nur, wenn er echten Mehrwert bietet.
- Scope als Nomen in Klammern, z. B. `fix(wallet): handle null comparison`.

## Body-Regeln

- Optional; verwenden, wenn Kontext fuer Reviewer wichtig ist.
- Erklaert Motivation, Randfaelle, Trade-offs.
- Beginnt mit genau einer Leerzeile nach dem Header.

## Footer-Regeln

- Optional; fuer Metadaten wie Referenzen.
- Format: `Token: value` oder `Token #value`.
- Token mit Bindestrichen statt Leerzeichen (Ausnahme: `BREAKING CHANGE`).
- Beispiele:
  - `Refs: #123`
  - `Reviewed-by: Name`
  - `BREAKING CHANGE: default rounding mode changed to bankers rounding`

## Ausgabeformat

Gib die finale Commit Message in einem einzigen Codeblock aus.

Beispiel:

```text
fix(money): avoid exception when comparing different currencies

Use deterministic fallback for comparison operators to keep ordering stable.

Refs: #142
```

## Referenz

Die verbindlichen Regeln sind lokal dokumentiert in:

- `agents/conventional-commits-v1.0.0.md`

Nutze diese lokale Datei als Quelle und lade die Regeln nicht aus dem Internet nach.
