# 05: Merchant sells the Magic weapon

**What to build:** the Merchant also offers a **Magic weapon** for 120 Gold. Its price is shown alongside the Armor price, and menu option **2** buys it. Buying it enchants the player's own occupation weapon:

- +4 maximum damage: long sword and lightning bolt spell 12 → 16, long bow 8 → 12, dagger 6 → 10.
- The weapon is renamed to its magic version: "magic long sword", "magic lightning bolt spell", "magic long bow" or "magic dagger".
- The ASCII art stays the same.

The new name and damage show on both stats screens and apply in combat.

The player still buys at most one item per encounter. Choosing the Magic weapon without enough Gold gets "can't afford" and the menu comes back. The encounter now ends early only when the player can afford neither item. All new text, including the four Magic weapon display names, is available in English, Spanish, French and Italian.

The enchantment rule and its price and bonus belong on the Merchant module from ticket 04, as named constants.

See the spec: `.scratch/merchant-encounter/spec.md` (Magic weapon user stories 12 to 16, story 26; required test 2, and the weapon versions of tests 3 and 4).

**Blocked by:** 04 (Merchant encounter: buy Armor or leave).

**Status:** resolved

- [x] The Merchant shows the Magic weapon price (120 Gold) next to the Armor price, and the menu offers **1** Armor, **2** Magic weapon, **3** Leave.
- [x] Buying the Magic weapon with at least 120 Gold takes 120 Gold, adds 4 to the weapon's maximum damage, and renames it to its magic version.
- [x] The enchanted weapon's name and maximum damage show on both stats screens and are used in combat.
- [x] Choosing the Magic weapon with less than 120 Gold shows "can't afford" and re-shows the menu. Gold and the weapon are unchanged.
- [x] Only a player who can afford neither item is told so and has the encounter end without the menu.
- [x] At most one item is bought per encounter.
- [x] New message keys and the four Magic weapon display names exist in all four languages, in plain ASCII, with no gendered articles in templated weapon sentences (ADR 0001), and the key-coverage test passes.
- [x] Test (Merchant): buying the Magic weapon with enough Gold takes 120 Gold, adds 4 maximum damage and renames the weapon. Run this for each occupation: long sword, lightning bolt spell, dagger, long bow.
- [x] Test (Merchant): buying the Magic weapon with too little Gold leaves Gold and the weapon unchanged.
- [x] Test (Merchant): buying the Magic weapon with exactly 120 Gold succeeds.
- [x] The console-flow tests from ticket 04 still pass in all four languages, and the full suite passes.
