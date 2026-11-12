# Alderaan Adventure

A text adventure in which a player creates a character, chooses a path, and faces what waits at the end of it.

## Language

### Characters and items

**Health**:
A character's current hit points. A player's Health never rises above their starting Health, the value rolled at creation.
_Avoid_: HP, hit points, life

**Rest**:
The recovery of a little Health a wounded player gets on the way north, before facing the Dragon. It happens only then.
_Avoid_: Heal-up, potion, regen

**Gold**:
The currency a player carries, rolled when the character is created.
_Avoid_: Money, coins, currency

**Armor**:
Worn equipment that grants Protection. A player starts with none.
_Avoid_: Shield, gear

**Protection**:
The amount by which Armor reduces the damage of each hit that lands. Damage never drops below zero.
_Avoid_: Defense, armor class, AC

**Magic weapon**:
A player's own occupation weapon after enchantment, dealing more maximum damage.
_Avoid_: Enchanted blade, new weapon, upgraded weapon

### Foes

**Foe**:
Any creature or person the player fights. The Dragon and the Bandit are both Foes.
_Avoid_: Monster, enemy, opponent

**Dragon**:
The Foe at the end of the north path. Facing it always ends the game.
_Avoid_: Boss, Smolderfang (that is its name, not what it is)

**Bandit**:
A Foe on the west path, weaker than the Dragon. Fighting it is a side encounter: winning or retreating returns the player to the crossroads.
_Avoid_: Thief (that is a player occupation), robber, highwayman

### Encounters

**Merchant**:
A trader met on the south path who offers Armor or a Magic weapon for Gold.
_Avoid_: Shopkeeper, vendor, trader

**Merchant encounter**:
The one-time meeting with the Merchant, in which the player buys at most one item or leaves.
_Avoid_: Shop, store, south event

**Bandit encounter**:
A meeting with the Bandit on the west path. The player fights or flees. It repeats on each trip west until the Bandit is defeated, after which the west path is quiet.
_Avoid_: Ambush, west event, bandit fight
