// Holds every Bandit rule. No console input or output.
// One Bandit lives for one game, so its wounds and its defeat are its own state.
public class Bandit : Foe
{
    public const int StatDieSides = 12;
    public const int ShortSwordMaxDamage = 6;
    public const string ShortSwordName = "short sword";

    // Loot is d10 x 10 Gold: 10 to 100.
    public const int LootDieSides = 10;
    public const int LootMultiplier = 10;

    // The fleeing toll takes half the player's Gold.
    public const int TollDivisor = 2;

    public bool IsDefeated { get; private set; }

    public override string StatsHeaderKey => "bandit_stats_header";
    public override string CombatHeaderKey => "combat_bandit_header";
    public override string PlayerAttackIntroKey => "player_attack_bandit_intro";
    public override string DefeatedNarrativeKey => "bandit_defeated_narrative";

    public Bandit(Die die) : base(die)
    {
        Name = "Grimble the Bandit";

        Strength = _die.Roll(StatDieSides);
        Agility = _die.Roll(StatDieSides);
        HealthPoints = _die.Roll(StatDieSides);

        Weapon = new Weapon(ShortSwordName, ShortSwordMaxDamage, "--|====>");
    }

    // The player won the fight: they loot the Bandit's purse, and the Bandit is gone
    // from the west path for good. Returns the Gold looted.
    public int Loot(Player player)
    {
        int looted = _die.Roll(LootDieSides) * LootMultiplier;
        player.Gold += looted;
        IsDefeated = true;
        return looted;
    }

    // The player fled mid-fight: the Bandit grabs part of their Gold, rounded down.
    // The Bandit stays on the west path. Returns the Gold taken.
    public int TakeToll(Player player)
    {
        int taken = Math.Max(0, player.Gold) / TollDivisor;
        player.Gold -= taken;
        return taken;
    }
}
