# 03: Protection reduces dragon damage

**What to build:** the player gains a **Protection** stat. Every character starts with no **Armor** and Protection 0, and Protection is shown on the character stats screen and the combat stats screen. When a dragon attack lands, the damage the player takes is reduced by their Protection, never below 0. The damage message reports the damage actually taken. At Protection 0 the game plays exactly as it does today.

The Protection rule lives in one place on the Player, a method that turns raw damage into damage taken. The Dragon's attack uses it instead of subtracting raw damage. This gives a test seam that doesn't depend on the random dice.

Nothing grants Protection yet. The Merchant's Armor (ticket 04) will.

See the spec: `.scratch/merchant-encounter/spec.md` (Armor and Protection user stories 6 to 10; required test 6; required test 7, Protection part).

**Blocked by:** 01 (Check that every language has every message key).

**Status:** resolved

- [x] A new character has Protection 0 and no Armor.
- [x] The Player can turn raw damage into damage taken: raw damage minus Protection, with a floor of 0.
- [x] Dragon attacks reduce the player's health by the damage taken after Protection. The existing "health never below 0" rule still holds.
- [x] The dragon damage message shows the damage after Protection.
- [x] The character stats screen and combat stats screen show a Protection line.
- [x] New message keys exist in all four languages, in plain ASCII, and the key-coverage test passes.
- [x] Test: 12 raw damage with Protection 3 gives 9.
- [x] Test: 2 raw damage with Protection 3 gives 0, never negative.
- [x] Test: 12 raw damage with no Armor (Protection 0) gives 12.
- [x] Test: a newly created character has Protection 0.
- [x] The full test suite passes.

## Comments

**2026-11-10, implemented** on branch `merchant-encounter` in commit `601ac7f` ("Reduce dragon damage by the player's Protection"). Full suite passed (114 of 114). Adds `Armor.cs` (read-only name and Protection), `Player.Armor`, `Player.Protection` and `Player.ApplyProtection`; new key `stats_protection`. Showing the Armor name (user story 11) is left for ticket 04.
