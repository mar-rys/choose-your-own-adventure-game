# 03: Return to the Checkpoint after a Dragon Defeat

**What to build:** a player defeated by the Dragon can try again, the same way they can after a Bandit Defeat.

- The Dragon encounter reports whether the player was defeated, as the Bandit encounter already does. Today the north path always ends the game, so the loop can't tell a Defeat from a win or a retreat.
- After a Dragon **Defeat**, the existing Dragon defeat ending is shown as before, followed by the same return prompt as in ticket 02.
- **`1`:** the Checkpoint taken at the crossroads is restored. The Dragon is back at full Health, and the player has the Health they had before the Rest on the way north. The awakening line is shown and the player is back at the path prompt. Going north again gives another Rest.
- **`2`:** the game ends exactly as a Dragon Defeat does today.
- A Dragon win and a Dragon retreat (before or mid-fight) still end the game with no return prompt.

No new messages are needed. This ticket reuses the prompt, the awakening line and the Checkpoint from ticket 02.

See the spec: `.scratch/checkpoint/spec.md` ("Game changes"; user stories 7, 11, 12, 23, 27 and 39; required tests 9 and 11).

**Blocked by:** 02 (Return to the Checkpoint after a Bandit Defeat).

**Status:** done

- [x] The Dragon encounter reports whether the player was defeated.
- [x] After a Dragon Defeat, the Dragon defeat ending is followed by the return prompt.
- [x] `1` restores the Dragon's Health and the player's pre-Rest Health, shows the awakening line, and shows the path prompt.
- [x] `2` ends the game as today.
- [x] A Dragon win and a Dragon retreat end the game without the return prompt.
- [x] Test (adventure loop): with a Player certain to lose to the Dragon, the input `n`, `a`, `1`, `e` shows the Dragon defeat ending, the prompt and the awakening line. Afterwards the Dragon's Health equals its starting Health and the Player's Health equals their Health before the Rest.
- [x] Test (adventure loop): with a Player certain to beat the Dragon, the input `n`, `a` shows the victory ending and no return prompt.
- [x] Test (adventure loop): the input `n`, `r` shows the retreat ending and no return prompt.
- [x] The full test suite passes.
