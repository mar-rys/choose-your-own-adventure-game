# 01: Prefactor: the adventure loop as its own routine

**What to build:** no change the player can see. The crossroads loop (path prompt, south, west, north, exit) moves out of start-game into a private routine of its own that takes the Player, the Merchant, the Bandit, the Dragon and Messages. Start-game still shows the title, picks the language, creates the Die and the world, runs character creation from the main menu, and then calls the routine.

This creates the seam that tickets 02 and 03 test through. A test can hand the routine a Player and Foes with rigged stats (Strength 0 never hits, Strength 20 always hits, Agility 0 never blocks) plus scripted console input, and get the same result every time, even though the dice can't be controlled.

Add a reflection test harness for the routine, following the existing Bandit-encounter tests: redirect console input and output, restore them in cleanup, and don't run the class in parallel.

See the spec: `.scratch/checkpoint/spec.md` ("Game changes", the adventure-loop seam; user story 38).

**Blocked by:** None (can start immediately).

**Status:** done

- [x] The adventure loop is a private routine that takes the Player, the Merchant, the Bandit, the Dragon and Messages, and start-game calls it.
- [x] Game behaviour is unchanged: every existing test passes without being edited.
- [x] Test (adventure loop): with a Player certain to lose to the Bandit, the input `w`, `a` shows the Bandit defeat ending and the routine returns, with no further path prompt.
- [x] Test (adventure loop): the input `s` then `e` visits the Merchant, returns to the path prompt, and then shows the adventure-ends line.
- [x] The full test suite passes.
