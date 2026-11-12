// Holds every Bandit rule. No console input or output.
// One Bandit lives for one game, so its wounds and its defeat are its own state.
public class Bandit : Foe
{
    public const int StatDieSides = 12;
    public const int ShortSwordMaxDamage = 6;
    public const string ShortSwordName = "short sword";

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

    // The player won the fight: the Bandit is gone from the west path for good.
    public void MarkDefeated()
    {
        IsDefeated = true;
    }
}
