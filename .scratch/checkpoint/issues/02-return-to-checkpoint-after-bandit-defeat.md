# 02: Return to the Checkpoint after a Bandit Defeat

**What to build:** a player defeated by the Bandit can try again.

- Every time the player stands at the crossroads, a **Checkpoint** is taken automatically, just before the path prompt.
- After a Bandit **Defeat**, the existing Bandit defeat ending is shown as before. Then the player is offered `1` to return to the Checkpoint or `2` to end the adventure. Anything else shows the existing invalid-input message and asks again.
- **`1`:** everything is put back as it was at the crossroads. The player sees a line saying they have awoken back at the crossroads, with their Health and Gold, and is back at the path prompt.
- **`2`:** the game ends exactly as a Bandit Defeat does today.
- Returning is free and unlimited. Dice aren't replayed.

The Checkpoint is a module with no console input or output, in the style of the Merchant and the Bandit. Taking one records the player's Health, Gold, Weapon type and maximum damage, and Armor; whether the Merchant encounter is used; the Bandit's Health and whether it is defeated; and the Dragon's Health. The Dragon's Health is recorded now, even though only ticket 03 needs it, so the module is complete. Restoring writes those values back into the same objects and never replaces them.

**Copy values, never references.** The Merchant's Magic weapon purchase changes the Weapon object in place, so the Checkpoint copies the Weapon's type and maximum damage. Armor is immutable, so a reference to it is safe. Restoring "no Armor" puts back no Armor.

The Merchant's "encounter used" flag and the Bandit's "defeated" flag keep their private setters. Each class gains one small method that lets a Checkpoint put its flag back.

Two new messages go into all four languages, in plain ASCII, following ADR 0001: the return prompt and the awakening line (with Health and Gold placeholders). Suggested keys: `defeat_retry_prompt` and `checkpoint_return`.

See the spec: `.scratch/checkpoint/spec.md` ("New module: Checkpoint", "Merchant and Bandit changes", "Game changes", "Language data"; user stories 1 to 6, 8 to 10, 13 to 22, 24 to 26 and 28 to 37; required tests 1 to 8, 10 and 11).

**Blocked by:** 01 (Prefactor: the adventure loop as its own routine).

**Status:** done

- [x] A Checkpoint is taken at every pass through the crossroads: after character creation, after every return from the south or the west, and after every return to a Checkpoint.
- [x] After a Bandit Defeat, the Bandit defeat ending is followed by the return prompt.
- [x] `1` restores the Checkpoint, shows the awakening line with the restored Health and Gold, and shows the path prompt.
- [x] `2` ends the game as today, with no awakening line.
- [x] A retreat from the Bandit (before or mid-fight) never shows the return prompt.
- [x] Both new keys exist in all four languages, and the key-coverage test passes.
- [x] Test (Checkpoint): after changing Health, Gold and Armor, using up the Merchant, wounding and defeating the Bandit, and wounding the Dragon, restoring puts every value back.
- [x] Test (Checkpoint): after the Weapon's maximum damage is raised and "magic " is added to its type in place, restoring puts back the original type and maximum damage.
- [x] Test (Checkpoint): restoring writes into the same Player, Merchant, Bandit and Dragon objects that were passed in.
- [x] Test (Checkpoint): taken with no Armor, then Armor given, restoring leaves no Armor and Protection 0.
- [x] Test (Checkpoint): taken after the Merchant is used and the Bandit is looted, restoring keeps both flags set.
- [x] Test (adventure loop): with a Player certain to lose, the input `w`, `a`, `1`, `e` shows the defeat ending, the prompt, the awakening line with the Health and Gold from before the fight, and the path prompt. Afterwards the Player's Health and the Bandit's Health equal their values before going west.
- [x] Test (adventure loop): `w`, `a`, `2` ends the game after the prompt, with no awakening line.
- [x] Test (adventure loop): `w`, `a`, `x`, `2` shows the invalid-input message and asks again.
- [x] Test (adventure loop): two Bandit Defeats in a row, each followed by `1`, both return to the crossroads.
- [x] Test (adventure loop): a mid-fight retreat from the Bandit returns to the crossroads without the return prompt.
- [x] The full test suite passes.
