# 01: Check that every language has every message key

**What to build:** a test that fails whenever any English dictionary key is missing from Spanish, French or Italian. Today only some keys are checked, per language. Every later ticket in this feature adds new message keys in four languages, so this guard catches a missed translation at build time instead of showing `[key]` placeholder text to a player at runtime. The current game should already pass it.

See the spec: `.scratch/merchant-encounter/spec.md` (required test 8).

**Blocked by:** None (can start immediately).

**Status:** resolved

- [x] A test loads English and each of Spanish, French and Italian, and asserts that every English dictionary key exists in that language.
- [x] A failing assertion names the missing key and the language.
- [x] The test passes against the current `language_data.json`, or any key it shows to be missing is added in plain ASCII, following ADR 0001.
- [x] The existing per-language required-key tests still pass. They may be kept or removed if the new test fully covers them.
- [x] The full test suite passes.

## Comments

**2026-11-10, implemented** on branch `merchant-encounter` in commit 9045446. `ReadDictionary_LanguageHasEveryEnglishKey` runs one DataRow each for Spanish, French and Italian, and a failure lists the language and every missing key. No keys were missing. The Spanish, French and Italian required-key tests were removed because the new test covers them. The English one is kept. Full suite: 103 passed.
