# 01: Combat fights any Foe, with the Dragon unchanged

**What to build:** a prefactor so that a second **Foe** can be added without copying combat. Nothing changes for the player. The Dragon fight reads exactly as before, line for line, in all four languages.

- The **Dragon** becomes a Foe. A Foe has a name, Strength, Agility, Health, a weapon, an attack against a Player that returns the narration, and a stats display.
- Combat takes a Player and any Foe instead of a Dragon. The player's attack takes any Foe too. Turn order, the attack/retreat prompt and the retreat flag are unchanged.
- Three combat lines name the *kind* of Foe rather than its name: the combat stats header, the "you attack the dragon" intro, and the defeated narrative. Each Foe supplies its own wording for these. The Dragon uses its existing message text.
- A Foe may supply a defence taunt and a damage reply after the player's attack. The Dragon keeps its taunts and replies. A Foe that supplies none produces no line there.
- The Foe attack rule stays as the Dragon's is today: d20 at or under its Strength hits, d20 at or under the player's Agility blocks, and damage passes through the player's apply-Protection method with Health clamped at 0.
- Message keys that already fill in the Foe's name (missed, hit, defends, damage dealt, health now) are shared by every Foe. Renaming them from `dragon_*` to Foe-neutral names is optional. If they are renamed, the text stays identical in every language.

See the spec: `.scratch/bandit-encounter/spec.md` ("New concept: Foe", "Combat changes", "Dragon changes"; user stories 13, 39, 40 and 46; required test 11).

**Blocked by:** None (can start immediately).

**Status:** ready-for-agent

- [ ] Dragon is a Foe, and Combat and the player's attack accept any Foe rather than a Dragon specifically.
- [ ] The combat stats header, the attack intro and the defeated narrative come from the Foe.
- [ ] Taunts and damage replies are optional per Foe. The Dragon still produces them.
- [ ] A full Dragon fight in each of the four languages produces the same text as before the change.
- [ ] If any message keys were renamed, every language was updated and the key-coverage test passes.
- [ ] Test (combat): the existing Dragon combat stats tests (headers, player and Dragon stats) pass unchanged.
- [ ] The full test suite passes with no behaviour change.
