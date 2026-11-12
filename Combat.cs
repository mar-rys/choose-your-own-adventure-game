public class Combat
{
    public Player player { get; set; }
    public Foe foe { get; set; }
    public Messages messages { get; set; }

    public bool PlayerRetreated { get; private set; }

    public Combat(Player player, Foe foe, Messages messages)
    {
        this.player = player;
        this.foe = foe;
        this.messages = messages;
        PlayerRetreated = false;
    }

    public bool StartCombat()
    {
        Console.WriteLine(GetCombatStatsDisplay());

        while (true)
        {
            PlayerAttacksFoeSequence();

            if (foe.HealthPoints <= 0)
                return true;

            FoeAttacksPlayerSequence();

            if (player.HealthPoints <= 0)
                return false;

            Console.WriteLine(messages.GetMessage("attack_prompt"));
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "r" || input == "retreat")
            {
                PlayerRetreated = true;
                Console.WriteLine(messages.GetMessage("retreat_combat"));
                return false;
            }

            if (input == "a" || input == "attack")
                continue;

            Console.WriteLine(messages.GetMessage("invalid"));
        }
    }

    public string GetCombatStatsDisplay()
    {
        var output = new List<string>();

        output.Add(messages.GetMessage("combat_stats_header"));
        output.Add(messages.GetMessage("combat_player_header"));
        output.Add(messages.GetMessage("stats_name") + player.Name);
        output.Add(messages.GetMessage("stats_strength") + player.Strength);
        output.Add(messages.GetMessage("stats_agility") + player.Agility);
        output.Add(messages.GetMessage("stats_health") + player.HealthPoints);
        output.Add(messages.GetMessage("stats_gold") + player.Gold);
        output.Add(messages.GetMessage("stats_protection") + player.Protection);

        if (player.Armor != null)
            output.Add(messages.GetMessage("stats_armor") + messages.TranslateWeaponForDisplay(player.Armor.Name));

        if (player.Weapon != null)
        {
            output.Add(messages.GetMessage("stats_weapon") + messages.TranslateWeaponForDisplay(player.Weapon.Type));
            output.Add(messages.GetMessage("stats_damage") + player.Weapon.MaxDamage);
        }

        output.Add("");
        output.Add(messages.GetMessage(foe.CombatHeaderKey));
        output.Add(messages.GetMessage("stats_name") + foe.Name);
        output.Add(messages.GetMessage("stats_strength") + foe.Strength);
        output.Add(messages.GetMessage("stats_agility") + foe.Agility);
        output.Add(messages.GetMessage("stats_health") + foe.HealthPoints);
        output.Add(messages.GetMessage("stats_weapon") + messages.TranslateWeaponForDisplay(foe.Weapon.Type));
        output.Add(messages.GetMessage("stats_damage") + foe.Weapon.MaxDamage);

        return string.Join(Environment.NewLine, output);
    }

    public void PlayerAttacksFoeSequence()
    {
        Console.WriteLine(player.Attack(foe, messages));
    }

    public void FoeAttacksPlayerSequence()
    {
        Console.WriteLine(foe.Attack(player, messages));
    }
}