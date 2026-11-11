# 02: Gold rolled at character creation

**What to build:** every character gets **Gold** during character creation. After rolling Health, the player is asked to type `roll` once more and receives d20 × 10 Gold (10 to 200), and the amount rolled is shown. Gold appears on the character stats screen and on the combat stats screen. All new text is available in English, Spanish, French and Italian.

Gold has no use yet. The Merchant (ticket 04) will spend it.

See the spec: `.scratch/merchant-encounter/spec.md` (Gold user stories 1 to 5; required test 7, Gold part).

**Blocked by:** 01 (Check that every language has every message key).

**Status:** resolved

- [x] Character creation asks for a fourth `roll` after Health, re-prompting on anything other than `roll`, the same as the other stat rolls.
- [x] The player's Gold is d20 × 10, so between 10 and 200 inclusive.
- [x] The rolled Gold is shown to the player.
- [x] The character stats screen shows a Gold line.
- [x] The combat stats screen shows the player's Gold.
- [x] New message keys exist in all four languages, in plain ASCII, and ticket 01's key-coverage test passes.
- [x] Test: creating a character with scripted console input (using the `Console.SetIn` pattern from the Dragon tests) gives Gold between 10 and 200.
- [x] Existing tests that script character creation are updated for the extra `roll` input, and the full suite passes.

## Comments

**2026-11-10, implemented** on branch `merchant-encounter` in commit `c24bfc5` ("Roll Gold at character creation"). Full suite passed (108 of 108). New keys: `roll_gold`, `gold_rolled`, `stats_gold`. The "existing tests updated" item needed no changes: no test scripted character creation before this ticket.
