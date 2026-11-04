# 1. Add Italian as a supported language

Date: 2026-11-04

## Status

Accepted

## Context

The game supports English, Spanish, and French. The player picks a language
on the first screen, before any translated text has been loaded.

All player-facing text lives in `language_data.json`. Each language has its own
section with six parts:

- `dictionary`: every message the game prints (69 keys).
- `raceMap` and `occupationMap`: the words a player can type, mapped to the
  internal English names (`Human`, `Fighter`, and so on).
- `displayRaceMap`, `displayOccupationMap` and `displayWeaponMap`: the internal
  names, mapped to what is shown on screen.

`Messages.ReadDictionary()` loads whichever section matches
`CurrentLanguage`. Game logic always works with the internal English names.
Because of this, adding a language should only need data plus a menu entry,
not changes to game logic.

We want to reach Italian-speaking players. Some constraints affect how Italian
can be added:

- **Commands are fixed in code.** `Game.cs` only accepts `n`/`s`/`e` for
  paths, `a`/`r` for combat, and `roll` for dice, in every language.
- **The existing translations use plain ASCII.** For example, the menu shows
  "Espanol" and "Francais". Accented characters can display incorrectly in the
  Windows console with its default code page.
- **Weapon names are inserted into sentences.** Italian articles and
  possessives change with the noun's gender (il/la, tuo/tua). A single
  template such as "you draw your {1}" cannot be correct for every weapon.

## Decision

Add Italian as the fourth language, following the same pattern as the others.

1. **Data:** add an `Italian` section to `language_data.json`. It has the same
   six parts and the same 69 dictionary keys as English, and nothing else in
   the file changes.
2. **Menu:** in `Game.cs`, add `4. Italiano` to the language menu and
   "Seleziona la lingua" to its heading. Map the input `4` to `"Italian"` in
   the language `switch`.
3. **No code changes elsewhere.** `Messages.cs` and the game logic stay as
   they are.
4. **Translation conventions:**
   - Use plain ASCII, matching the existing languages. Accented final vowels
     are written with an apostrophe (`e'` for è, `piu'` for più).
   - Keep the shared command letters. They happen to match Italian words:
     **n**ord, **s**ud, **e**sci, **a**ttaccare, **r**itirarsi. Keep `roll`,
     as Spanish and French do.
   - Word the sentences that contain a weapon name so they need no
     gender-specific article, for example "impugni la tua arma ({1})" and
     "attacca {1} con: {2}!".
   - Races are Umano, Elfo, Nano, Halfling. Occupations are Guerriero, Mago,
     Ladro, Arciere.
5. **Tests:** copy the existing Spanish and French tests for Italian. Add tests
   for Italian input, for Italian display names, and that every English
   dictionary key exists in Italian.

## Consequences

### Positive

- Italian-speaking players can play the whole game in their language.
- The change is small and low-risk. It adds 104 lines of JSON and changes
  three lines in `Game.cs`. Existing languages and game logic are untouched.
- Adding a language without code changes shows the data-driven design works
  as intended, and this ADR records that process for future languages.
- New tests make a missing Italian key fail the build, instead of showing up
  as `[key]` placeholder text at runtime. The test suite grows from 94 to 104
  tests, and all pass.

### Negative

- **ASCII spelling looks less polished.** Writing `e'` instead of `è` is
  understandable, but it is not proper Italian. Switching to real accents
  later would need console UTF-8 support, which also affects the other
  languages.
- **Some wording is less natural.** Avoiding gendered articles makes a few
  combat lines sound more formal than a native writer would make them.
- **The text has not been reviewed by a native speaker.** It should be
  proofread before release.
- **There is more to maintain.** Every new message key must now be translated
  into four languages. The key-coverage test catches gaps for Italian only, so
  Spanish and French have no matching check.
- **The menu does not scale well.** The language menu, its multilingual
  heading, and the `switch` are hard-coded in `Game.cs`, so each new language
  needs a code change. Building the menu from the languages in
  `language_data.json` would remove that.

### Follow-up

- Have a native Italian speaker review the translations.
- Consider a test that checks every language has every English key, not just
  Italian.
- Consider building the language menu from the JSON file.
- Revisit UTF-8 console output if accented text becomes a requirement.
