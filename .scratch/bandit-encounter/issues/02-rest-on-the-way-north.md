# 02: Rest on the way north

**What to build:** a wounded player taking the north path **Rests** before the Dragon appears. They recover d6 **Health**, never above their starting Health, and are told how much they recovered and their Health now. A player at full Health sees no Rest line. Rest happens only on the north path, never at the crossroads.

- The player's starting Health is recorded when Health is rolled during character creation.
- Rest is a Player method with no console output. If Health is below starting Health, it recovers d6, capped at starting Health. It returns the amount recovered, which is 0 at full Health. The Rest die (6) is a named constant.
- The north branch calls Rest first. If anything was recovered, it shows the Rest line, then runs the Dragon encounter as today.
- The Rest line is available in English, Spanish, French and Italian, in plain ASCII.

In play, Rest becomes meaningful once the Bandit fight exists (ticket 03). Until then it is verified through tests.

See the spec: `.scratch/bandit-encounter/spec.md` ("Player changes", the north branch under "Game changes"; user stories 32 to 38 and 45; required tests 6 to 9).

**Blocked by:** None (can start immediately).

**Status:** resolved

- [x] Character creation records starting Health equal to the Health rolled.
- [x] Rest recovers d6 Health, never above starting Health, and returns the amount recovered.
- [x] Rest at full Health changes nothing and returns 0.
- [x] Taking the north path while wounded shows the Rest line, with the amount and the new Health, before the Dragon narrative.
- [x] Taking the north path at full Health shows no Rest line.
- [x] The Rest message key exists in all four languages and the key-coverage test passes.
- [x] Test (Player): starting 10, Health 9 → Rest leaves Health at exactly 10 and returns 1.
- [x] Test (Player): Health at its starting value → Rest leaves Health unchanged and returns 0.
- [x] Test (Player): starting 20, Health 1 → Rest leaves Health from 2 to 7 and returns Health minus 1.
- [x] Test (console): a newly created character's starting Health equals their Health, using scripted input.
- [x] The full test suite passes.
