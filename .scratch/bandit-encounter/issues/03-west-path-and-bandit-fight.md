# 03: West path and the Bandit fight

**What to build:** the crossroads gets a third path. Going west leads to the **Bandit encounter**:

1. The crossroads accepts `w` / `west` and describes three paths: north, south and west. `e` still exits. The invalid-path line lists `n`, `s`, `w` and `e`.
2. The west-path narrative plays, then **Grimble the Bandit** introduces itself and its stats are shown.
3. The player chooses attack (`a`) or retreat (`r`). Invalid input re-prompts.
4. **Retreat before any blows:** costs nothing. Show the free-retreat line and the return-to-crossroads line, then the path prompt.
5. **Attack:** run Combat against the Bandit, using the Foe engine from ticket 01.
   - **Won:** the Bandit is marked defeated. Show the return-to-crossroads line, then the path prompt.
   - **Retreated mid-fight:** show the return-to-crossroads line, then the path prompt. The Bandit stays on the west path with the Health it had left.
   - **Lost:** show the Bandit defeat ending, and the game ends.
6. Once the Bandit is defeated, later trips west show the empty-camp line and return to the path prompt.

No Gold changes hands in this ticket. Loot and the fleeing toll are added in ticket 04.

The Bandit is a Foe and a new module with no console input or output. It takes the shared Die. Its Strength, Agility and Health are rolled on d12. It fights with a short sword (maximum damage 6, with its own ASCII art). It has no taunts or damage replies. It supplies its own combat header, "you attack the bandit" intro and defeated narrative. It reports whether it has been defeated. Game creates one Bandit per game, alongside the Dragon and the Merchant, so its wounds and defeat persist between visits. The dice size and weapon damage are named constants. The west-path console routine stays thin.

All new text is in English, Spanish, French and Italian, in plain ASCII, with no gendered articles before templated names (ADR 0001). That includes the "short sword" display name. Don't call the Bandit a "thief" anywhere, because Thief is a player occupation.

See the spec: `.scratch/bandit-encounter/spec.md` ("New module: Bandit", "Game changes", "Language data"; user stories 1 to 18, 21, 25 to 31, 41 to 44; required tests 1, 2, 10, 12, 13, 14 and 15).

**Blocked by:** 01 (Combat fights any Foe, with the Dragon unchanged).

**Status:** ready-for-agent

- [ ] The crossroads description mentions three paths in all four languages, and `w` / `west` lead to the Bandit encounter.
- [ ] `n`, `s` and `e`, and their long forms, still work as before.
- [ ] The Bandit encounter shows the narrative, the Bandit's intro with its name, and its stats before the attack/retreat prompt.
- [ ] Retreating before the fight returns to the crossroads with Gold and Health unchanged.
- [ ] The Bandit fight uses the shared combat rules. Protection reduces its hits, and combat lines name the Bandit, never a dragon.
- [ ] Winning marks the Bandit defeated and returns to the crossroads. Later trips west show the empty camp.
- [ ] Retreating mid-fight returns to the crossroads. The next trip west finds the Bandit with the Health it had left.
- [ ] Losing shows the Bandit defeat ending and ends the game.
- [ ] All new keys and the "short sword" display name exist in all four languages, and the key-coverage test passes.
- [ ] Test (Bandit): a new Bandit is named "Grimble the Bandit", has Strength, Agility and Health from 1 to 12, and a short sword with maximum damage 6 and non-empty art.
- [ ] Test (Bandit): a new Bandit is not defeated.
- [ ] Test (combat): the combat stats display against the Bandit includes its name and its Foe header, and not the Dragon header.
- [ ] Test (Game): path validation accepts `w` and `west`, and still accepts `n`, `s`, `e` and their long forms.
- [ ] Test (console): in each language, a scripted flow (go west, retreat before the fight, exit) runs without throwing.
- [ ] Test (console): in each language, the Bandit encounter with the Bandit already defeated shows the empty camp and returns, without throwing.
- [ ] The full test suite passes.
