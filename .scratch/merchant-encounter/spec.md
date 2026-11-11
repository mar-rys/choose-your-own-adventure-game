# Spec: Merchant encounter on the south path

Status: ready-for-agent

## Problem Statement

Going south is a dead end. The player reads "nothing adventurous happens… you return home, poor, and still bored" and is sent back to the path prompt. Nothing a player does before the dragon changes their odds: every character goes north with the stats and weapon they were dealt at creation. There is no reason to go south and no choice to make before the fight.

## Solution

Going south for the first time leads to a **Merchant encounter**. A **Merchant** on the woodland path offers one of two items for **Gold**:

- **Armor** ("magic armor"), costing 80 Gold, which gives **Protection** 3. Each dragon hit that lands does 3 less damage, never below zero.
- A **Magic weapon**, costing 120 Gold, which enchants the player's own occupation weapon: +4 maximum damage, renamed "magic <weapon>".

Every character now rolls Gold at creation (d20 × 10, so 10 to 200), so whether a player can afford either item depends on that roll. The player buys at most one item or leaves. Afterwards they return to the crossroads and the path prompt, so they can go on to face the dragon better equipped. The Merchant appears only once per game. Later trips south find that the Merchant has moved on.

All new text is available in English, Spanish, French and Italian.

## User Stories

### Gold

1. As a player, I want to roll for Gold during character creation, so that my starting wealth comes from luck like my other stats.
2. As a player, I want to type `roll` for Gold the same way I do for Strength, Agility and Health, so that character creation feels consistent.
3. As a player, I want to see how much Gold I rolled, so that I know what I can afford later.
4. As a player, I want my Gold shown on my character stats screen, so that I can check it at any time I view my stats.
5. As a player, I want my Gold shown on the combat stats screen, so that my full character sheet is visible before the fight.

### Armor and Protection

6. As a player, I want to start with no Armor and Protection 0, so that buying Armor is a meaningful upgrade.
7. As a player, I want Armor to reduce the damage of each dragon hit that lands, so that buying it improves my chance of surviving.
8. As a player, I want damage never to drop below zero after Protection, so that a weak hit can't heal me.
9. As a player, I want to see my Protection on the stats screen, so that I know my Armor is in effect.
10. As a player, I want to see my Protection on the combat stats screen, so that I can see the Armor I paid for before the fight.
11. As a player who bought Armor, I want the stats screen to show that I wear magic armor, so that the purchase feels real.

### Magic weapon

12. As a player, I want the Magic weapon to be my own occupation weapon enchanted, so that my character keeps their identity (a magician keeps casting lightning, an archer keeps their bow).
13. As a player, I want the Magic weapon to deal 4 more maximum damage, so that the purchase makes my attacks stronger.
14. As a player, I want my weapon shown as "magic <weapon>" after buying, so that I can see it has been enchanted.
15. As a player, I want the updated maximum damage shown on both stats screens, so that I can see the improvement.
16. As a thief or archer with a weak weapon, I want the same flat bonus as everyone else, so that the upgrade helps me catch up.

### The Merchant encounter

17. As a player going south for the first time, I want to meet a Merchant on the woodland path, so that the south path has a purpose.
18. As a player, I want the Merchant to show my current Gold alongside both prices, so that I can make an informed choice.
19. As a player, I want to choose by typing 1 (Armor), 2 (Magic weapon) or 3 (leave), so that the menu works the same in every language.
20. As a player, I want invalid input to show the menu again, so that a typo doesn't waste my one chance.
21. As a player, I want to be able to leave without buying, so that I am never forced to spend Gold.
22. As a player who picks an item I can't afford, I want the Merchant to tell me and show the menu again, so that I can try the other item or leave.
23. As a player who can't afford either item, I want the Merchant to say so and end the encounter, so that I am not stuck choosing between options I can't take.
24. As a player who can afford exactly the price, I want the purchase to succeed, so that the price is honest.
25. As a player, I want the price taken from my Gold when I buy, so that my remaining Gold is accurate.
26. As a player, I want to buy at most one item, so that choosing between Armor and the Magic weapon is a real trade-off.
27. As a player, I want a line about returning to the crossroads after the encounter, so that I know where I am.
28. As a player, I want the path prompt to appear again after the encounter, so that I can go north to face the dragon.
29. As a player going south a second time, I want to find that the Merchant has moved on, so that the world is consistent and I can't buy twice.
30. As a player going south a second time, I want to return to the path prompt afterwards, so that the extra trip doesn't end my game.

### Languages

31. As a Spanish, French or Italian player, I want all the Merchant text in my language, so that the encounter is as playable as the rest of the game.
32. As a non-English player, I want "magic armor" and each Magic weapon name translated, so that the stats screens read naturally.
33. As a player in any language, I want the text in the same plain-ASCII style as the rest of the game, so that it displays correctly in the Windows console.

### Developers

34. As a developer, I want the buying rules in a module with no console input or output, so that I can test them directly without scripting keyboard input.
35. As a developer, I want Protection applied in one place, so that the damage rule is tested once and the dice don't need to be controlled.
36. As a developer, I want a test that every English message key exists in every other language, so that a missing translation fails the build rather than showing `[key]` at runtime.

## Implementation Decisions

### New module: Merchant

- A deep module holding every buying rule. It has no console input or output.
- It operates on a Player and changes that Player's Gold, Armor/Protection and Weapon.
- **Interface (behaviour, not exact names):**
  - Try to buy Armor for a Player. This succeeds only when the encounter is unused and the Player has at least 80 Gold. On success it takes 80 Gold, gives Protection 3 with the display name "magic armor", and marks the encounter used.
  - Try to buy the Magic weapon for a Player. This succeeds only when the encounter is unused and the Player has at least 120 Gold. On success it takes 120 Gold, adds 4 to the weapon's maximum damage, renames the weapon to its magic version, and marks the encounter used.
  - Report whether the encounter has been used.
  - Report whether the Player can afford each item, so the console layer can decide between "show the menu" and "can't afford anything".
  - Mark the encounter used without a purchase, when the player leaves or can afford nothing.
- Each purchase reports a result the console layer can turn into a message: success, can't afford, or encounter already used.
- A failed purchase changes nothing on the Player.
- Prices (80, 120), Protection (3) and the damage bonus (+4) are named constants on the Merchant, so balancing changes touch one place.
- One Merchant instance lives for one game, so "once per game" is the Merchant's own state.

### Player changes

- New **Gold** stat, rolled as d20 × 10 in a fourth `roll` step after Health during character creation.
- New **Armor** state, made up of Protection (default 0) and a display name (default none).
- New method that applies Protection to raw damage, returning raw damage minus Protection with a floor of 0. This is the only place the Protection rule lives.
- The stats display adds Gold and Protection lines, plus the Armor name when Armor is worn.

### Dragon changes

- Dragon attacks pass their damage roll through the Player's apply-Protection method instead of subtracting raw damage. The existing "health never below 0" clamp stays.
- The damage message reports the damage actually taken, after Protection.

### Combat changes

- The combat stats display adds Gold and Protection for the player.

### Game changes

- Game creates one Merchant per game alongside the Dragon.
- The south path branch hands off to a thin merchant-encounter routine:
  - **Encounter unused:**
    1. Show the intro.
    2. If the Player can afford neither item, show the "can't afford anything" line and mark the encounter used.
    3. Otherwise show the current Gold, both prices and the menu (1, 2, 3). Re-show the menu on invalid input. Show the "can't afford" message and re-show the menu when a purchase fails for lack of Gold. Show a confirmation on success. Mark the encounter used on leave.
    4. Show the return-to-crossroads line.
  - **Encounter used:** show the "Merchant has moved on" line.
  - Either way, control returns to the path prompt.
- The old "nothing adventurous happens / return home" message is removed or reused as the "moved on" line.

### Language data

- New dictionary keys in all four languages, plain ASCII, following the conventions in ADR 0001. They cover:
  - the Gold roll prompt and result
  - Gold and Protection stat labels
  - Merchant intro, Gold-and-prices line and menu
  - invalid choice, can't afford this item, can't afford anything
  - Armor bought, Magic weapon bought
  - return to the crossroads, Merchant has moved on
- New display-name entries in all four languages for "magic armor" and the four Magic weapons ("magic long sword", "magic lightning bolt spell", "magic dagger", "magic long bow"). These go through the existing display-name mapping that weapon names already use.
- Templated messages with a weapon or Armor name avoid gendered articles, as ADR 0001 requires for Italian.
- Menu input uses numbers so commands are language-neutral, consistent with the main menu.

### Unchanged

- The dice. No changes to the Die and no dice injection into Player.
- The Dragon's own stats and weapon. Combat turn order. North path behaviour.

## Testing Decisions

### What makes a good test here

- Test external behaviour through the seams below: the Player's Gold, Protection, Weapon and the damage it takes. Don't test private methods or the internal fields of the Merchant.
- Avoid randomness. Never assert on a dice outcome except where a range is the behaviour, as with Gold being 10 to 200.
- One rule, one place. Each rule is asserted at exactly one seam.

### Seams

1. **Merchant (new, primary).** Give it a Player whose Gold and occupation weapon are set up in the test, call a purchase, and assert on the Player.
2. **Player's apply-Protection method (new, small).** This exists because combat dice can't be controlled in tests.
3. **Messages loaded per language (existing).**
4. **Console flows with scripted input (existing pattern).**

### Required tests

1. Buying Armor with enough Gold takes 80 Gold and gives Protection 3.
2. Buying the Magic weapon with enough Gold takes 120 Gold, adds 4 to maximum damage, and renames the weapon to its magic version. Run this for all four occupations: long sword, lightning bolt spell, dagger, long bow.
3. An unaffordable purchase (either item) leaves Gold, Protection and Weapon unchanged.
4. Gold exactly equal to the price succeeds, for both items.
5. After one encounter, whether a purchase or leaving, the Merchant refuses a further purchase and the Player is unchanged.
6. Protection reduces damage: 12 with Protection 3 gives 9. 2 with Protection 3 gives 0, never negative. 12 with no Armor gives 12.
7. A newly created character has Protection 0 and Gold from 10 to 200. Run character creation with scripted input.
8. Every English dictionary key exists in Spanish, French and Italian, including the new Merchant keys.
9. The merchant-encounter console flow runs without throwing in each of the four languages, with scripted input.

### Prior art

- **Console scripting:** the Dragon tests redirect `Console.SetIn` / `Console.SetOut` to a `StringReader` / `StringWriter`, restore the originals in cleanup, and feed attack/retreat input. Use the same pattern for tests 7 and 9.
- **Key coverage:** the Messages tests already check required keys per language (`ReadDictionary_AllRequiredKeysExistIn…`). Test 8 generalises this to "every English key exists in every language".
- **Does-not-throw style:** the Game tests (`EndGame_DoesNotThrow_When…`) run a flow in each language and pass if nothing throws. Test 9 follows this style.

## Out of Scope

- Buying both items, or more than one of either.
- Selling, haggling, or any other use for Gold.
- Making the Dragon use Armor or Protection.
- Changing the Agility-based block roll.
- Injecting or seeding dice for deterministic combat tests.
- Building the language menu from the JSON file, or switching to UTF-8 accented text (both are follow-ups in ADR 0001).
- A native-speaker review of the new translations. It is recommended before release but isn't part of this work.
- New ASCII art for Magic weapons. The existing art is kept.

## Further Notes

- Domain terms (Gold, Armor, Protection, Magic weapon, Merchant, Merchant encounter) are defined in `CONTEXT.md`. Use them in code, messages and test names.
- No ADR is recorded for this feature. The balance numbers are easy to change named constants, so they don't meet the "hard to reverse" bar.
- Balance at the chosen numbers: about 60% of characters can afford Armor, about 40% can afford the Magic weapon, and the rest can afford neither.
- Every new message key has to be added to all four languages. Test 8 is what enforces this.
