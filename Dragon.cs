public class Dragon : Foe
{
    // Example use of taunts:
    // string taunt = dragon.GetRandomDefenseTaunt().Replace("{race}", player.Race);
    private readonly List<string> _defenseTauntKeys = new()
{
    "dragon_defense_taunt_1",
    "dragon_defense_taunt_2",
    "dragon_defense_taunt_3",
    "dragon_defense_taunt_4"
};

    private readonly List<string> _damageReplyKeys = new()
{
    "dragon_damage_reply_1",
    "dragon_damage_reply_2",
    "dragon_damage_reply_3",
    "dragon_damage_reply_4"
};

    public override string StatsHeaderKey => "dragon_stats_header";
    public override string CombatHeaderKey => "combat_dragon_header";
    public override string PlayerAttackIntroKey => "player_attack_intro";
    public override string DefeatedNarrativeKey => "dragon_defeated_narrative";

    public Dragon(Die die) : base(die)
    {
        Name = "Smolderfang";

        // Dragon stats are initialized using Die so the class aligns better
        // with the sequence/start-game diagrams and shared repo structure.
        Strength = _die.Roll(20);
        Agility = _die.Roll(20);
        HealthPoints = _die.Roll(20);

        Weapon = new Weapon("claws", 12, "<<< claws >>>");
    }

    public override string GetRandomDefenseTaunt(Messages messages)
    {
        int index = _die.Roll(_defenseTauntKeys.Count) - 1;
        return messages.GetMessage(_defenseTauntKeys[index]);
    }

    public override string GetRandomDamageReply(Messages messages)
    {
        int index = _die.Roll(_damageReplyKeys.Count) - 1;
        return messages.GetMessage(_damageReplyKeys[index]);
    }
}
