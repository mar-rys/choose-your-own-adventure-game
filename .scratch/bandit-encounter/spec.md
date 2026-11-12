# Spec: Bandit encounter on the west path, and Rest before the Dragon

Status: ready-for-agent

## Problem Statement

The crossroads offers only two real choices. North is the Dragon, which ends the game. South is the Merchant, who can be visited once. The only Foe in the game is the Dragon, so the first fight a player ever has is also the last one. Gold is rolled at creation and can be spent once, and nothing a player does can earn more of it. West and east don't lead anywhere.

## Solution

A new **west** path leads to a **Bandit encounter**. **Grimble the Bandit** is a **Foe**, weaker than the **Dragon**, who blocks the woodland road. The player sees the Bandit's stats and chooses to fight or retreat:

- **Retreat before any blows:** free. The player returns to the crossroads.
- **Win:** the player loots the Bandit's purse (d10 × 10 **Gold**, so 10 to 100) and returns to the crossroads. The Bandit is gone for good, and later trips west find only its empty camp.
- **Retreat mid-fight:** the Bandit grabs half the player's Gold (rounded down) as they flee. The player returns to the crossroads. The Bandit stays on the west path and keeps the wounds the player gave it.
- **Lose:** the game ends with a Bandit-specific defeat ending.

The player's **Health** carries over between fights. There's no healing at the crossroads. Instead, a wounded player gets a **Rest** on the way north: they recover d6 Health, never above their starting Health, before the Dragon appears.

Going west before south gives a strategy: rob the robber, then spend the loot at the Merchant. Every fight is a trade-off, because wounds carry into the Dragon fight and Rest gives back only a little.

Combat works with any Foe, so the Dragon and the Bandit fight under the same rules. The Dragon fight looks and reads exactly as it does today.

All new text is available in English, Spanish, French and Italian.

## User Stories

### The west path

1. As a player at the crossroads, I want a third path to the west, so that there is more to explore than the Dragon and the Merchant.
2. As a player, I want to enter `w` or `west` to take the west path, so that it works like `n`/`north` and `s`/`south`.
3. As a player, I want `e` to still exit the game, so that the controls I already know don't change.
4. As a player, I want the crossroads description to mention all three paths, so that I know the west path exists.
5. As a player who types something invalid at the crossroads, I want the error to list `n`, `s`, `w` and `e`, so that I know every valid choice.

### Meeting the Bandit

6. As a player going west, I want a short narrative of the woodland road and the Bandit blocking it, so that the encounter has a setting.
7. As a player, I want the Bandit to introduce itself by name, so that it feels like a character the way the Dragon does.
8. As a player, I want to see the Bandit's Strength, Agility, Health and weapon before deciding, so that I can judge whether to fight.
9. As a player, I want the Bandit to be clearly weaker than the Dragon, so that a fresh character can usually win.
10. As a player, I want to choose attack (`a`) or retreat (`r`), the same keys as the Dragon encounter, so that the controls are consistent.
11. As a player who enters something invalid at the Bandit prompt, I want to be asked again, so that a typo doesn't decide the encounter.
12. As a player who retreats before any blows, I want to lose nothing and return to the crossroads, so that sizing up the Bandit is safe.

### Fighting the Bandit

13. As a player, I want the fight against the Bandit to use the same turn order and rules as the Dragon fight, so that combat is predictable.
14. As a player, I want my Armor's Protection to reduce the Bandit's hits, so that Armor bought from the Merchant helps in every fight.
15. As a player, I want my Magic weapon to work against the Bandit, so that my purchase pays off here too.
16. As a player, I want the combat stats screen to show the Bandit's name and stats, so that I know who I'm fighting.
17. As a player, I want combat messages to name the Bandit, so that they don't wrongly mention a dragon.
18. As a player, I want the Bandit to fight without taunts, so that it reads as a lesser Foe than the Dragon.

### Outcomes

19. As a player who defeats the Bandit, I want to loot its purse for 10 to 100 Gold, so that winning is rewarded.
20. As a player who defeats the Bandit, I want to be told how much Gold I found, so that I know what I can now afford.
21. As a player who defeats the Bandit, I want to return to the crossroads, so that I can go on to the Merchant or the Dragon.
22. As a player who retreats mid-fight, I want the Bandit to take half my Gold, rounded down, so that fleeing a fight I started has a cost.
23. As a player who retreats mid-fight, I want to be told how much Gold I lost, so that I know what I have left.
24. As a player with no Gold who retreats mid-fight, I want to lose nothing, so that the toll never makes my Gold negative.
25. As a player who retreats mid-fight, I want to return to the crossroads, so that I can still choose another path.
26. As a player defeated by the Bandit, I want my own defeat ending, so that losing to a bandit reads differently from losing to the Dragon.
27. As a player defeated by the Bandit, I want the game to end, so that defeat has consequences.

### Coming back west

28. As a player who retreated from the Bandit, I want to find it still on the west path next time, so that the world stays consistent.
29. As a player returning to a Bandit I wounded, I want it to have the Health I left it with, so that my earlier attacks still count.
30. As a player who defeated the Bandit, I want later trips west to find only its empty camp, so that I can't loot it twice.
31. As a player visiting the empty camp, I want to return to the crossroads, so that the extra trip doesn't end my game.

### Health and Rest

32. As a player, I want my Health after a Bandit fight to carry over, so that every fight has a lasting cost.
33. As a player, I want no healing at the crossroads, so that I can't retreat from the Bandit, heal, and try again for free.
34. As a wounded player taking the north path, I want to Rest and recover a little Health before the Dragon appears, so that a side fight doesn't doom me.
35. As a player, I want Rest to recover d6 Health, so that it helps without undoing the Bandit fight.
36. As a player, I want Rest never to take me above my starting Health, so that Rest can't make me stronger than I was created.
37. As a player at full Health taking the north path, I want no Rest message, so that the story doesn't mention wounds I don't have.
38. As a player who Rests, I want to be told how much Health I recovered and my Health now, so that I go into the Dragon fight informed.

### The Dragon, unchanged

39. As a player, I want the Dragon fight to read exactly as it did before, so that the Foe refactor changes nothing I can see.
40. As a player, I want the Dragon to keep its taunts and damage replies, so that it keeps its personality.

### Languages

41. As a Spanish, French or Italian player, I want all the Bandit and Rest text in my language, so that the new content is as playable as the rest of the game.
42. As a player in any language, I want the new text in the same plain-ASCII style as the rest of the game, so that it displays correctly in the Windows console.
43. As a player in any language, I want the Bandit's weapon name translated on the stats screens, so that they read naturally.

### Developers

44. As a developer, I want every Bandit rule in a module with no console input or output, so that I can test loot, toll and defeat directly.
45. As a developer, I want Rest to be a Player method with a testable result, so that the cap rule is tested without scripting the north path.
46. As a developer, I want one combat engine that takes any Foe, so that combat bugs are fixed and tested in one place.
47. As a developer, I want the balance numbers (Bandit dice, weapon damage, loot, toll, Rest die) as named constants, so that rebalancing touches one place.
48. As a developer, I want the every-key-in-every-language test to cover the new keys, so that a missing translation fails the build.

## Implementation Decisions

### New concept: Foe

- A Foe is anything the player fights. Dragon and Bandit are both Foes.
- A Foe exposes what combat needs: a name, Strength, Agility, Health, a weapon, an attack against a Player that returns the narration, and a stats display.
- A Foe also supplies its own wording for the few combat lines that name the kind of Foe rather than its name: the combat stats header, the "you attack the …" intro, and the defeated narrative.
- A Foe may supply a defence taunt and a damage reply after the player's attack. The Dragon does. The Bandit supplies none, and combat then prints nothing for that line.
- The Foe's attack on the player follows the current Dragon rule: d20 at or under its Strength hits, d20 at or under the player's Agility blocks, damage is a weapon roll passed through the player's apply-Protection method, and Health is clamped at 0.

### Combat changes

- Combat takes a Player and any Foe instead of a Dragon. Turn order, the attack/retreat prompt, and the retreat flag are unchanged.
- Player's attack takes any Foe instead of a Dragon.
- The combat stats display shows the Foe's header, name and stats.

### Dragon changes

- The Dragon becomes a Foe. Its stats (d20), claws (12), name, taunts and damage replies are unchanged.
- Its Foe-specific wording uses its existing message text, so every Dragon line renders exactly as before in all four languages.

### New module: Bandit

- A deep module holding every Bandit rule. It has no console input or output. It takes the shared Die, like the Dragon.
- Name: "Grimble the Bandit". Strength, Agility and Health are rolled on d12. Its weapon is a short sword with maximum damage 6 and its own ASCII art.
- **Interface (behaviour, not exact names):**
  - Report whether the Bandit has been defeated.
  - Loot it: give the Player d10 × 10 Gold, mark the Bandit defeated, and return the amount looted.
  - Take the fleeing toll: remove half the Player's Gold, rounded down, and return the amount taken. Gold never goes below 0.
- The Bandit's Health is its own state and persists between visits. One Bandit lives for one game, so "defeated for good" and "keeps its wounds" are both the Bandit's own state.
- Dice sizes (12), weapon damage (6), loot (d10 × 10) and the toll fraction (half) are named constants on the Bandit.

### Player changes

- New **starting Health**, recorded when Health is rolled at character creation.
- New **Rest** method: when Health is below starting Health, recover d6 Health, capped at starting Health. Return the amount recovered, which is 0 when the player is already at full Health. The Rest die (6) is a named constant.

### Game changes

- Game creates one Bandit per game, alongside the Dragon and the Merchant, using the shared Die.
- The path prompt accepts `w` / `west`, and the path validation accepts them too.
- The west branch hands off to a thin Bandit-encounter routine:
  - **Bandit defeated:** show the empty-camp line, then return to the path prompt.
  - **Bandit not defeated:**
    1. Show the west-path narrative, the Bandit's intro with its name, and its stats.
    2. Prompt attack or retreat. Re-prompt on invalid input.
    3. **Retreat:** show the free-retreat line and the return-to-crossroads line, then return to the path prompt.
    4. **Attack:** run Combat against the Bandit.
       - **Won:** loot the Bandit, show the Gold found, show the return-to-crossroads line, and return to the path prompt.
       - **Retreated mid-fight:** take the toll, show the Gold lost, show the return-to-crossroads line, and return to the path prompt.
       - **Lost:** show the Bandit defeat ending and end the game.
- The north branch first calls Rest. If any Health was recovered, show the Rest line with the amount and the new Health. Then the Dragon encounter runs as today.

### Language data

- New dictionary keys in all four languages, plain ASCII, following ADR 0001 conventions (no gendered articles before templated names). They cover:
  - the updated crossroads description (three paths) and the updated invalid-path line
  - west-path narrative, Bandit intro, Bandit stats header, Bandit attack/retreat prompt
  - free retreat, Gold looted, Gold lost to the toll, return to the crossroads, empty camp
  - Bandit defeat ending
  - the Bandit's Foe-specific combat lines: combat header, "you attack the bandit" intro, defeated narrative
  - the Rest line (Health recovered, Health now)
- New display-name entries in all four languages for the Bandit's weapon ("short sword").
- Combat keys that already take the Foe's name as a placeholder (missed, hit, defends, damage dealt, health now) are shared by every Foe. Renaming them from `dragon_*` to Foe-neutral names is allowed but not required. If they are renamed, the text stays identical.

### Unchanged

- The Die, and there's still no dice injection into Player.
- The Merchant and its rules. Gold at creation. Protection.
- Dragon stats, weapon and behaviour. The north path still ends the game whatever happens.

## Testing Decisions

### What makes a good test here

- Test external behaviour through the seams below: the Player's Gold and Health, the Bandit's defeated state and stats, and rendered text. Don't test private methods or internal fields.
- Avoid randomness. Assert exact values where the rule fixes them (the toll, the Rest cap), and ranges only where a range is the behaviour (loot, rolled stats, uncapped Rest).
- One rule, one place. Each rule is asserted at exactly one seam.

### Seams

1. **Bandit (new, primary).** Build a Bandit with a real Die and a Player whose Gold is set up in the test, call loot or toll, and assert on the Player and the Bandit.
2. **Player Rest (new, small).** Set Health and starting Health directly, call Rest, and assert Health and the returned amount.
3. **Combat with any Foe (existing, widened).** The existing combat tests keep passing with the Dragon. New tests run against the Bandit.
4. **Messages per language (existing).**
5. **Console flows with scripted input (existing pattern).**

### Required tests

1. A new Bandit is named "Grimble the Bandit", has Strength, Agility and Health from 1 to 12, and a short sword with maximum damage 6 and non-empty art.
2. A new Bandit is not defeated.
3. Looting gives the Player 10 to 100 Gold, a multiple of 10, returns the amount given, and marks the Bandit defeated.
4. The toll takes half the Gold rounded down and returns the amount taken: 75 → 38 left (37 taken), 100 → 50 left, 1 → 1 left (0 taken), 0 → 0 left.
5. The toll does not mark the Bandit defeated.
6. Rest at starting 10 and Health 9 leaves Health at exactly 10 and returns 1.
7. Rest at full Health leaves Health unchanged and returns 0.
8. Rest at starting 20 and Health 1 leaves Health from 2 to 7 and returns Health minus 1.
9. A newly created character's starting Health equals their Health. Run character creation with scripted input.
10. The combat stats display against the Bandit includes the Bandit's name and its Foe header, and doesn't include the Dragon header.
11. The combat stats display against the Dragon is unchanged in English. The existing header and stat assertions still pass.
12. Path validation accepts `w` and `west`, and still accepts `n`, `s`, `e` and their long forms.
13. Every English dictionary key exists in Spanish, French and Italian, including the new keys. The existing test covers this.
14. In each of the four languages, a scripted west-path flow (go west, retreat before the fight, exit) runs without throwing.
15. In each of the four languages, a scripted Bandit-encounter flow with the Bandit already defeated shows the empty camp and returns, without throwing.

### Prior art

- **Deep-module tests:** the Merchant tests build a Player with set-up Gold and a weapon, call a rule, and assert on the Player. The Bandit tests follow this.
- **Stats-in-range:** the Dragon test `NewDragon_InitializesRequiredProperties` covers test 1.
- **Console scripting:** the Dragon and merchant-encounter tests redirect `Console.SetIn` / `Console.SetOut` and restore them in cleanup. Use this for tests 9, 14 and 15.
- **Key coverage:** the existing every-English-key-in-every-language test covers test 13 automatically.
- **Does-not-throw style:** the Game tests (`EndGame_DoesNotThrow_When…`) and the merchant-encounter flow tests.

## Out of Scope

- An east path, or remapping `e` away from exit.
- Taunts or damage replies for the Bandit.
- Healing anywhere except Rest on the way north, including potions and healing at the crossroads.
- The Bandit healing between visits.
- Making Foes use Armor or Protection.
- Injecting or seeding dice for deterministic combat tests.
- Fixing the existing case where a Halfling's Strength penalty can roll starting Health at 0 or below.
- A native-speaker review of the new translations. It is recommended before release but isn't part of this work.

## Further Notes

- Domain terms (Foe, Dragon, Bandit, Bandit encounter, Health, Rest, Gold, Protection) are defined in `CONTEXT.md`. Use them in code, messages and test names. Don't call the Bandit a "thief", because Thief is a player occupation.
- No ADR is recorded. Generalising combat to Foes is easy to reverse, and the balance numbers are named constants.
- Order matters to the player: Bandit loot is only spendable if the player goes west before visiting the Merchant, who appears once.
- Suggested slicing for issues: (1) the Foe refactor with the Dragon unchanged, (2) starting Health and Rest on the north path, (3) the west path and the Bandit fight with free retreat, defeat and the empty camp, (4) loot and the fleeing toll.
