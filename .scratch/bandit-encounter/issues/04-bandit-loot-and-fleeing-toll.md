# 04: Bandit loot and the fleeing toll

**What to build:** the Bandit fight now has stakes in **Gold**.

- **Win:** the player loots the Bandit's purse for d10 × 10 Gold (10 to 100) and is told how much they found. The Bandit is marked defeated.
- **Retreat mid-fight:** the Bandit takes half the player's Gold, rounded down, and the player is told how much they lost. Gold never goes below 0. The Bandit is *not* marked defeated.
- Retreating *before* the fight stays free.

This makes the "rob the robber, then shop" route work end to end: go west, win, go south, and spend the loot at the Merchant.

Both rules live on the Bandit, with no console input or output. Each one changes the Player's Gold and returns the amount. Looting also marks the Bandit defeated, replacing the plain "mark defeated" step from ticket 03 on the win path. Loot (d10 × 10) and the toll fraction (half) are named constants on the Bandit. The console routine only prints the amounts.

The new messages (Gold looted, Gold lost) are in all four languages, in plain ASCII.

See the spec: `.scratch/bandit-encounter/spec.md` ("New module: Bandit" interface; user stories 19, 20, 22 to 24 and 47; required tests 3 to 5).

**Blocked by:** 03 (West path and the Bandit fight).

**Status:** resolved

- [x] Winning against the Bandit adds 10 to 100 Gold, in multiples of 10, and shows the amount.
- [x] Retreating mid-fight removes half the player's Gold, rounded down, and shows the amount lost.
- [x] Retreating before the fight changes no Gold.
- [x] The Gold looted and Gold lost keys exist in all four languages, and the key-coverage test passes.
- [x] Test (Bandit): looting gives 10 to 100 Gold, a multiple of 10, returns that amount, and marks the Bandit defeated.
- [x] Test (Bandit): the toll on 75 Gold takes 37 and leaves 38. On 100 it leaves 50. On 1 it takes 0 and leaves 1. On 0 it leaves 0. Each returns the amount taken.
- [x] Test (Bandit): the toll does not mark the Bandit defeated.
- [x] The full test suite passes.

## Comments

Resolved in commit 648d6a0: `Bandit.Loot` and `Bandit.TakeToll` with named constants, Gold looted / Gold lost messages in all four languages, and tests in `BanditTests` and `BanditEncounterTests`. Full suite: 184 passed.
