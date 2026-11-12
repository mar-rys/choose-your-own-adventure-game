# Spec: Checkpoint, a return to the crossroads after a Defeat

Status: ready-for-agent

## Problem Statement

A **Defeat** ends the game for good. A player may spend several minutes creating a character, robbing the Bandit, visiting the Merchant and resting on the way north. If the Dragon then wins one fight, all of that is lost, and the program exits. The only way to try again is to restart, roll a new character and repeat every encounter. Losing to the Bandit is just as final, even though it is meant to be a side fight.

## Solution

Every time the player stands at the crossroads, the game quietly takes a **Checkpoint**: a record of the player and the world as they are at that moment.

After a Defeat, against the Bandit or the Dragon, the existing defeat message is shown as before. Then the player is offered a choice:

- **`1`: return to the Checkpoint.** Everything is put back as it was when the player last stood at the crossroads: their Health, Gold, Weapon and Armor, whether the Merchant has been visited, the Bandit's Health and whether it was defeated, and the Dragon's Health. A short line tells the player they have awoken at the crossroads and shows their Health and Gold. The player is back at the path prompt.
- **`2`: end the adventure.** The game ends exactly as a Defeat does today.

Returning is free and has no limit. Dice aren't replayed, so a retried fight can go differently. Checkpoints live in memory only and last until the game is closed. There are no save files.

Retreating is never a Defeat, and winning the Dragon fight still ends the game. Neither offers a return.

All new text is available in English, Spanish, French and Italian.

## User Stories

### When a Checkpoint is taken

1. As a player, I want a Checkpoint taken automatically whenever I stand at the crossroads, so that I never have to remember to save.
2. As a player who has just created a character, I want a Checkpoint taken before my first path choice, so that a Defeat on my very first encounter doesn't make me reroll.
3. As a player returning from the Merchant, I want a new Checkpoint that includes what I bought, so that a later Defeat doesn't undo my purchase.
4. As a player returning from the Bandit (after a win, a free retreat, a mid-fight retreat or a visit to the empty camp), I want a new Checkpoint that includes the outcome, so that a later Defeat keeps my loot or my Toll.
5. As a player who has just returned to a Checkpoint, I want the crossroads to take a Checkpoint again, so that the next Defeat returns me to the same place.

### Being offered a return

6. As a player defeated by the Bandit, I want to see the Bandit's defeat ending first, so that losing still reads as losing.
7. As a player defeated by the Dragon, I want to see the Dragon's defeat ending first, so that losing still reads as losing.
8. As a defeated player, I want to be offered a choice between returning to the Checkpoint (`1`) and ending my adventure (`2`), so that I decide whether to try again.
9. As a defeated player who types something invalid, I want to be asked again, so that a typo doesn't end my adventure.
10. As a defeated player who chooses `2`, I want the game to end as it does today, so that I can still stop when I want to.
11. As a player who retreats from the Dragon, I want the game to end as before with no return offered, so that retreating keeps its meaning.
12. As a player who beats the Dragon, I want the victory ending as before with no return offered, so that winning is the end of the story.
13. As a player who retreats from the Bandit, I want to return to the crossroads as before with no return offered, so that retreating is never treated as a Defeat.

### Returning to the Checkpoint

14. As a returning player, I want my Health put back to what it was at the crossroads, so that the Defeat doesn't follow me.
15. As a returning player, I want my Gold put back to what it was at the crossroads, so that my purse is consistent with where I am.
16. As a returning player, I want my Weapon, including any Magic weapon upgrade, to be exactly as it was at the crossroads, so that my purchase is kept and is never doubled.
17. As a returning player, I want my Armor to be exactly as it was at the crossroads, so that my Protection is unchanged.
18. As a returning player who had already visited the Merchant, I want the Merchant to stay gone, so that I can't buy a second time.
19. As a returning player who hadn't yet visited the Merchant, I want the Merchant still to be there, so that I can still shop.
20. As a returning player defeated by the Bandit, I want the Bandit's Health put back to what it was at the crossroads, so that the retry is the same fight I chose to take.
21. As a returning player who had wounded the Bandit in an earlier mid-fight retreat, I want the Bandit to keep those earlier wounds, so that the Checkpoint reflects the world as it was.
22. As a returning player who had already defeated the Bandit, I want it to stay defeated, so that I can't loot it twice.
23. As a returning player defeated by the Dragon, I want the Dragon's Health put back to full, so that I am not fighting a weakened Dragon I didn't earn.
24. As a returning player, I want to see a line saying I have awoken at the crossroads, with my Health and Gold, so that I know where I stand.
25. As a returning player, I want to land at the crossroads path prompt, so that I can choose any path again.
26. As a returning player who was at low Health at the Checkpoint, I want to resume at that Health, so that the Checkpoint is honest about where I was.
27. As a returning player who heads north again, I want the Rest on the way north to happen again, so that I get the same chance to recover.
28. As a returning player, I want new dice rolls in the retried fight, so that trying again can turn out differently.
29. As a player, I want to return as many times as I like at no cost, so that a hard fight is something I can keep working at.
30. As a player who is defeated a second time after returning, I want to be offered a return again, so that retries are truly unlimited.

### Keeping the rest of the game the same

31. As a player who never loses, I want the game to look and play exactly as before, so that the Checkpoint is invisible until I need it.
32. As a player, I want the Checkpoint to last only until I close the game, so that nothing is written to my disk.

### Languages

33. As a Spanish, French or Italian player, I want the return prompt and the awakening line in my language, so that the new feature is as playable as the rest of the game.
34. As a player in any language, I want the new text in the same plain-ASCII style as the rest of the game, so that it displays correctly in the Windows console.

### Developers

35. As a developer, I want the Checkpoint as a module with no console input or output, so that capture and restore are tested directly.
36. As a developer, I want the Checkpoint to copy values rather than hold references, so that later changes to the player or world (especially the Weapon, which is changed in place) can't change what is restored.
37. As a developer, I want restore to write back into the same Player, Merchant, Bandit and Dragon objects, so that every reference the game holds stays valid.
38. As a developer, I want the adventure loop in one routine that takes the Player and the world, so that the Defeat → return → crossroads flow can be tested deterministically with rigged stats.
39. As a developer, I want the Dragon encounter to report whether the player was defeated, so that the game loop treats both Foes the same way.
40. As a developer, I want the every-key-in-every-language test to cover the new keys, so that a missing translation fails the build.

## Implementation Decisions

### New module: Checkpoint

- A deep module with no console input or output, in the same style as the Merchant and the Bandit.
- **Interface (behaviour, not exact names):**
  - **Take:** given the Player, the Merchant, the Bandit and the Dragon, record their current state and return a Checkpoint.
  - **Restore:** given the same four objects, write the recorded state back into them.
- **What it records**, all copied as values:
  - Player: Health, Gold, the Weapon's type and maximum damage, and the Armor.
  - Merchant: whether the encounter has been used.
  - Bandit: Health, and whether it has been defeated.
  - Dragon: Health.
- **What it doesn't record**, because it never changes after creation: the player's name, race, occupation, Strength, Agility and starting Health, the Weapon's ASCII art, and the Foes' names, Strength, Agility and Weapons.
- **Reference safety:**
  - The Weapon is mutable and the Merchant's Magic weapon purchase changes it in place. The Checkpoint must copy the Weapon's type and maximum damage, never keep a reference to the Weapon object.
  - Armor is immutable, so keeping a reference to it is safe. Restoring a null Armor (none bought) must put back null.
- Restore writes into the existing objects. It never replaces them, because the game loop holds references to the Merchant, the Bandit and the Dragon.
- Dice aren't recorded or restored.

### Merchant and Bandit changes

- The Merchant's "encounter used" flag and the Bandit's "defeated" flag keep their private setters.
- Each class gains one small method that lets a Checkpoint put the flag back to a given value.
- No buying, loot or toll rules change.

### Game changes

- **The adventure loop becomes a routine of its own** that takes the Player, the Merchant, the Bandit, the Dragon and Messages. Character creation and object creation stay in start-game, which then calls this routine. It is private and tested through reflection, like the Bandit-encounter routine.
- **A Checkpoint is taken at the top of every pass through the loop**, just before the path prompt. That covers after character creation, after every return from the south or the west, and after every return to a Checkpoint.
- **The Dragon encounter reports whether the player was defeated**, as the Bandit encounter already does. A win or a retreat still ends the game as today. Only a Defeat leads to the return prompt.
- **On a Defeat** (the Bandit encounter reports the player didn't survive, or the Dragon encounter reports a Defeat):
  1. The existing defeat message has already been shown (`bandit_defeat` for the Bandit, `defeat` for the Dragon).
  2. Show the return prompt and read the choice. Re-prompt with the existing invalid-input message for anything other than `1` or `2`.
  3. **`1`:** restore the Checkpoint, show the awakening line with the restored Health and Gold, and continue the loop. The loop takes a new Checkpoint, which is the same state, and shows the path prompt.
  4. **`2`:** end the game exactly as a Defeat does today.
- The Rest on the way north happens after the Checkpoint, so it happens again on every retry.

### Language data

- Two new dictionary keys in all four languages, plain ASCII, following ADR 0001 conventions:
  - **Return prompt:** offers `1` to return to the last Checkpoint and `2` to end the adventure.
  - **Awakening line:** says the player has awoken back at the crossroads, with placeholders for Health and Gold.
- Suggested names: `defeat_retry_prompt` and `checkpoint_return`.

### Unchanged

- The Die. There is still no dice injection or seeding.
- Every existing message, including both defeat endings.
- The Dragon's stats and behaviour. North still ends the game on a win or a retreat.
- Merchant prices, Bandit loot and toll, Rest, Protection.

## Testing Decisions

### What makes a good test here

- Test external behaviour through the seams below: the Player's Health, Gold, Weapon and Armor, the Merchant's and the Bandit's flags, the Foes' Health, and rendered text. Don't test the Checkpoint's internal fields.
- Avoid randomness by rigging stats, not dice. A Strength of 0 never hits (a d20 never rolls at or under 0), a Strength of 20 always hits, and an Agility of 0 never blocks. A Player at Health 1 against a Foe with Strength 20 and Agility 0 is always defeated in one Foe turn.
- One rule, one place. The capture/restore rule is asserted at the Checkpoint seam. The prompt and loop behaviour are asserted at the adventure-loop seam.

### Seams

1. **Checkpoint (new, primary).** Build a Player, a Merchant, a Bandit and a Dragon with known state, take a Checkpoint, change everything, restore, and assert the original state is back.
2. **Adventure loop (new, via reflection).** Call the private adventure routine with a rigged Player and world and scripted console input, then assert on the output and the objects afterwards. This follows the existing pattern for the Bandit-encounter routine.
3. **Messages per language (existing).** The every-English-key-in-every-language test.

### Required tests

1. **Restore puts back every recorded value.** Take a Checkpoint. Change Health, Gold and Armor, use up the Merchant, wound and defeat the Bandit, and wound the Dragon. Restore, and assert each value is back to what it was.
2. **Restore is immune to in-place Weapon changes.** Take a Checkpoint, then change the Weapon in place the way the Magic weapon purchase does (raise maximum damage, prefix "magic " to the type). Restore, and assert the original type and maximum damage are back.
3. **Restore keeps the same objects.** After restore, the Player's Weapon and the world objects are the same instances the test passed in, or the values have been written into them. The game's references must stay valid.
4. **Restoring "no Armor" puts back no Armor.** Take a Checkpoint with no Armor, give the player Armor, restore, and assert there is no Armor and Protection is 0.
5. **Restoring a used Merchant and a defeated Bandit keeps them so.** Take a Checkpoint after the Merchant is used and the Bandit is looted, restore, and assert both flags are still set.
6. **Bandit Defeat, then `1`.** Run the adventure loop with input `w`, `a`, `1`, `e` and a Player who is certain to lose. Assert the Bandit defeat ending appears, then the return prompt, then the awakening line with the Health and Gold from before the fight, then the path prompt. The Player's Health and the Bandit's Health afterwards equal their values before going west.
7. **Bandit Defeat, then `2`.** Assert the game ends after the defeat ending and the prompt, with no awakening line.
8. **Invalid input at the return prompt.** Input such as `x` then `2` shows the invalid message and asks again.
9. **Dragon Defeat, then `1`.** Run the adventure loop with input `n`, `a`, `1`, `e`. Assert the Dragon's defeat ending, the prompt and the awakening line appear. The Dragon's Health afterwards equals its starting Health, and the Player's Health equals their Health before the Rest.
10. **Retries are unlimited.** Two Defeats in a row, each followed by `1`, both return to the crossroads.
11. **No prompt on a win or a retreat.** A Dragon win and a Dragon retreat end the game without the return prompt. A mid-fight Bandit retreat returns to the crossroads without it.
12. **Every new key in every language.** The existing test covers this automatically.

### Prior art

- **Deep-module tests:** the Merchant and Bandit tests build a Player with set-up values, call a rule, and assert on the objects. The Checkpoint tests follow this.
- **Private routine via reflection with rigged stats:** the Bandit-encounter tests call the Bandit-encounter routine through reflection with scripted input. `LosingTheFight_ShowsTheBanditDefeatEnding_AndEndsTheGame` already forces a Defeat with rigged stats. The adventure-loop tests follow this.
- **Console redirection:** set and restore console input and output in setup and cleanup, with the test class marked do-not-parallelize, as the encounter tests do.
- **Key coverage:** the existing every-English-key-in-every-language test.

## Out of Scope

- Save files, or keeping a Checkpoint after the game is closed.
- Choosing a Checkpoint, or keeping more than the latest one.
- Any cost or limit on returning.
- Offering a return after a retreat or a win.
- Returning to the main menu, or rolling a new character, from the return prompt.
- Replaying the same dice on a retry, or injecting or seeding dice.
- Restoring the chosen language. It isn't game state.
- Changing either existing defeat ending.
- A native-speaker review of the new translations. It is recommended before release but isn't part of this work.

## Further Notes

- Domain terms (Defeat, Checkpoint, Foe, Health, Gold, Rest, Merchant, Bandit, Dragon) are defined in `CONTEXT.md`. Use them in code, messages and test names. Say "return to the Checkpoint", not "respawn", "continue" or "load".
- No ADR is recorded. In-memory Checkpoints are easy to reverse.
- Only one encounter can happen between a Checkpoint and a Defeat: the west fight, or the Rest and the Dragon. A Merchant visit is always followed by a new Checkpoint first. Copying the Weapon's values is still required, because the Weapon is changed in place and future paths may change it between Checkpoints.
- Issues (in `issues/`):
  1. Prefactor: the adventure loop as its own routine, with no visible change.
  2. Return to the Checkpoint after a Bandit Defeat: the Checkpoint module, the Merchant and Bandit restore methods, a Checkpoint taken at the crossroads, the return prompt and both message keys.
  3. Return to the Checkpoint after a Dragon Defeat: the Dragon encounter reports a Defeat, and the return prompt is reused.
