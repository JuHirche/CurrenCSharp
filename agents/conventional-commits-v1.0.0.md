# Conventional Commits 1.0.0 (lokale Regeln)

Diese Datei enthaelt die relevanten Regeln der Spezifikation Conventional Commits 1.0.0 als lokale Referenz fuer dieses Repository.

Quelle (Original): https://www.conventionalcommits.org/en/v1.0.0/

## Grundformat

```text
<type>[optional scope][optional !]: <description>

[optional body]

[optional footer(s)]
```

## Kernelemente

1. Jeder Commit MUSS mit einem `type` beginnen, gefolgt von optionalem `scope`, optionalem `!` und einem verpflichtenden `: `.
2. `feat` MUSS fuer neue Features verwendet werden.
3. `fix` MUSS fuer Bugfixes verwendet werden.
4. Ein `scope` DARF angegeben werden und MUSS ein Nomen in Klammern sein, z. B. `fix(parser):`.
5. Direkt nach `: ` MUSS eine kurze `description` folgen.
6. Ein Body DARF verwendet werden und MUSS mit genau einer Leerzeile nach dem Header beginnen.
7. Der Body darf mehrere Absaetze enthalten.
8. Footer duerfen angegeben werden und beginnen nach einer Leerzeile nach dem Body (oder Header, wenn kein Body vorhanden ist).
9. Jeder Footer MUSS aus Token + Separator + Wert bestehen:
   - `Token: value` oder
   - `Token #value`
10. Footer-Token MUESSEN Bindestriche statt Leerzeichen verwenden (Ausnahme: `BREAKING CHANGE`).

## Breaking Changes

Ein Breaking Change MUSS auf eine der folgenden Arten markiert werden:

1. `!` direkt vor dem Doppelpunkt im Header, z. B. `feat(api)!: ...`
2. Footer mit exakt `BREAKING CHANGE: <description>`

Hinweise:

- Wenn `!` verwendet wird, KANN der `BREAKING CHANGE`-Footer entfallen.
- `BREAKING-CHANGE` gilt als synonym zu `BREAKING CHANGE`.
- `BREAKING CHANGE` MUSS in Grossbuchstaben geschrieben werden.

## Erlaubte Typen

Die Spezifikation fordert nur die Bedeutung von `feat` und `fix`.
Weitere Typen sind erlaubt, z. B.:

- `docs`
- `style`
- `refactor`
- `perf`
- `test`
- `build`
- `ci`
- `chore`
- `revert`

Diese zusaetzlichen Typen haben ohne `BREAKING CHANGE` keine implizite SemVer-Bedeutung.

## SemVer-Zuordnung

- `fix` -> PATCH
- `feat` -> MINOR
- `BREAKING CHANGE` (unabhaengig vom Typ) -> MAJOR

## Praktische Konventionen fuer dieses Repository

1. Header-Zeile kurz und praezise halten.
2. Description im Imperativ formulieren.
3. Scope nur verwenden, wenn er echten Mehrwert liefert.
4. Bei gemischten Aenderungen moeglichst in mehrere Commits aufteilen.
5. Keine irrefuehrenden Typen verwenden (z. B. `chore` fuer echte Bugfixes).

## Beispiele

```text
feat(wallet): add weighted distribution strategy
```

```text
fix(money): handle null in comparison operators
```

```text
refactor(currency): split parser and formatter responsibilities
```

```text
feat(api)!: replace legacy conversion endpoint

BREAKING CHANGE: remove /v1/convert and require /v2/convert
```

```text
revert: undo rounding change in money formatter

Refs: a1b2c3d
```
