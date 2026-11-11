# 04: Merchant encounter: buy Armor or leave

**What to build:** going south now leads to the **Merchant encounter**. The first trip south goes like this:

1. The Merchant appears on the woodland path.
2. The player sees their Gold and the Armor price (80 Gold).
3. A menu offers **1** Armor and **3** Leave. Option 2, the Magic weapon, is added in ticket 05. Invalid input re-shows the menu.
4. Choosing Armor without enough Gold gets a "can't afford" message, and the menu comes back. If the player can't afford Armor at all, the Merchant says so and the encounter ends without the menu.
5. Buying takes 80 Gold and gives "magic armor" with Protection 3, which shows on both stats screens and reduces dragon damage, as built in ticket 03.
6. The player gets a return-to-crossroads line, then the path prompt again, so they can go north to the dragon.

The Merchant appears once per game. Later trips south show a "the Merchant has moved on" line and return to the path prompt. The old "nothing adventurous happens / return home" text is removed or reused as the "moved on" line. All new text is available in English, Spanish, French and Italian.

The buying rules go in a new **Merchant** module with no console input or output. It operates on a Player: it checks whether they can afford it, takes the Gold, gives the Armor and marks the encounter used. Prices and Protection are named constants on the Merchant. The south-path console routine stays thin and only turns the Merchant's results into messages.

See the spec: `.scratch/merchant-encounter/spec.md` (Merchant encounter user stories 17 to 30, Armor stories 7 and 11; required tests 1, 3, 4, 5 and 9 for Armor).

**Blocked by:** 02 (Gold rolled at character creation), 03 (Protection reduces dragon damage).

**Status:** resolved

- [x] The first trip south shows the Merchant intro, the player's current Gold, and the Armor price.
- [x] The menu accepts **1** (Armor) and **3** (Leave). Any other input re-shows the menu.
- [x] Buying Armor with at least 80 Gold takes 80 Gold and gives Protection 3, shown as "magic armor" on the stats screens.
- [x] Choosing Armor with less than 80 Gold shows "can't afford" and re-shows the menu. Gold and Protection are unchanged.
- [x] A player who can't afford Armor is told so and the encounter ends.
- [x] Leaving buys nothing and changes nothing.
- [x] After the encounter, the return-to-crossroads line is shown and the path prompt returns.
- [x] Every later trip south shows the "moved on" line and returns to the path prompt, and no purchase is possible.
- [x] The old "nothing adventurous happens / return home" text no longer appears.
- [x] New message keys and the "magic armor" display name exist in all four languages, in plain ASCII, and the key-coverage test passes.
- [x] Test (Merchant): buying Armor with enough Gold takes 80 Gold and gives Protection 3.
- [x] Test (Merchant): buying Armor with too little Gold leaves Gold and Protection unchanged.
- [x] Test (Merchant): buying Armor with exactly 80 Gold succeeds.
- [x] Test (Merchant): after one encounter, whether a purchase or leaving, a further purchase is refused and the Player is unchanged.
- [x] Test (console): the merchant encounter flow with scripted input runs without throwing in each of the four languages.
- [x] The full test suite passes.

## Comments

**2026-11-10, implemented** on branch `merchant-encounter` in commit `b28b6c5` ("Add the Merchant encounter on the south path: buy Armor or leave"). Adds `Merchant.cs` (`BuyArmor`, `Leave`, `CanAffordArmor`, `EncounterUsed`, a `PurchaseResult` enum, and named constants for price, Protection and name), wires one Merchant per game into `Game.cs`'s south path, and shows the Armor name on both stats screens. New keys: `merchant_intro`, `merchant_offer`, `merchant_menu`, `merchant_invalid`, `merchant_cant_afford`, `merchant_cant_afford_anything`, `merchant_armor_bought`, `merchant_return`, `merchant_moved_on`, `stats_armor`, plus the "magic armor" display name. Tests are in `MerchantTests.cs` and `MerchantEncounterTests.cs`. The status was left at `ready-for-agent` when the work landed and was corrected afterwards. Full suite after ticket 05: 141 passed.
