public class Player
{
    public const int RestDie = 6;

    private readonly Die _die = new Die();

    public string Name { get; set; } = "";
    public string Race { get; set; } = "";
    public string Occupation { get; set; } = "";

    public int Strength { get; set; }
    public int Agility { get; set; }
    public int HealthPoints { get; set; }
    public int StartingHealth { get; set; }
    public int Gold { get; set; }

    public Weapon? Weapon { get; set; }
    public Armor? Armor { get; set; }

    public int Protection => Armor?.Protection ?? 0;

    public void CreateCharacter(Messages messages)
    {
        Race = PromptForRace(messages);
        Name = PromptForName(messages);
        Occupation = PromptForOccupation(messages);

        AssignWeaponByOccupation();

        int strengthModifier = RollStrength(messages);
        RollAgility(messages);
        RollHealthPoints(messages, strengthModifier);
        RollGold(messages);

        PromptForNextAction(messages);
    }

    private string PromptForRace(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("race_prompt"));
            string? input = Console.ReadLine()?.Trim();

            if (messages.IsValidRace(input))
                return messages.NormalizeRace(input!);

            Console.WriteLine(messages.GetMessage("race_invalid"));
        }
    }

    private string PromptForName(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("name_prompt"));
            string? input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine(messages.GetMessage("name_invalid"));
        }
    }

    private string PromptForOccupation(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("occupation_prompt"));
            string? input = Console.ReadLine()?.Trim();

            if (messages.IsValidOccupation(input))
                return messages.NormalizeOccupation(input!);

            Console.WriteLine(messages.GetMessage("occupation_invalid"));
        }
    }

    private void AssignWeaponByOccupation()
    {
        switch (Occupation.ToLower())
        {
            case "fighter":
                Weapon = new Weapon("long sword", 12, "-)=====>");
                break;
            case "magician":
                Weapon = new Weapon("lightning bolt spell", 12, "zap~~~~~~");
                break;
            case "thief":
                Weapon = new Weapon("dagger", 6, "-)==>");
                break;
            case "archer":
                Weapon = new Weapon("long bow", 8, "}    -->");
                break;
        }
    }

    private int RollStrength(Messages messages)
    {
        PromptRoll(messages.GetMessage("roll_strength"), messages);
        Strength = _die.Roll(20);

        int modifier = _die.Roll(4);

        if (Race == "Halfling")
        {
            Strength -= modifier;
            return -modifier;
        }

        Strength += modifier;
        return modifier;
    }

    private void RollAgility(Messages messages)
    {
        PromptRoll(messages.GetMessage("roll_agility"), messages);
        Agility = _die.Roll(20);

        if (Race == "Halfling" || Race == "Elf")
            Agility += _die.Roll(4);
    }

    private void RollHealthPoints(Messages messages, int strengthModifier)
    {
        PromptRoll(messages.GetMessage("roll_health"), messages);
        HealthPoints = _die.Roll(20) + strengthModifier;
        StartingHealth = HealthPoints;
    }

    private void RollGold(Messages messages)
    {
        PromptRoll(messages.GetMessage("roll_gold"), messages);
        Gold = _die.Roll(20) * 10;
        Console.WriteLine(string.Format(messages.GetMessage("gold_rolled"), Gold));
    }

    private void PromptForNextAction(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("next_action"));
            string? input = Console.ReadLine()?.Trim();

            if (input == "1")
                DisplayStats(messages);
            else if (input == "2")
                return;
            else
                Console.WriteLine(messages.GetMessage("invalid"));
        }
    }

    public void DisplayStats(Messages messages)
    {
        Console.WriteLine(messages.GetMessage("stats_header"));
        Console.WriteLine(messages.GetMessage("stats_name") + Name);
        Console.WriteLine(messages.GetMessage("stats_race") + messages.TranslateRaceForDisplay(Race));
        Console.WriteLine(messages.GetMessage("stats_occupation") + messages.TranslateOccupationForDisplay(Occupation));
        Console.WriteLine(messages.GetMessage("stats_strength") + Strength);
        Console.WriteLine(messages.GetMessage("stats_agility") + Agility);
        Console.WriteLine(messages.GetMessage("stats_health") + HealthPoints);
        Console.WriteLine(messages.GetMessage("stats_gold") + Gold);
        Console.WriteLine(messages.GetMessage("stats_protection") + Protection);

        if (Armor != null)
            Console.WriteLine(messages.GetMessage("stats_armor") + messages.TranslateWeaponForDisplay(Armor.Name));

        if (Weapon != null)
        {
            Console.WriteLine(messages.GetMessage("stats_weapon") + messages.TranslateWeaponForDisplay(Weapon.Type));
            Console.WriteLine(messages.GetMessage("stats_damage") + Weapon.MaxDamage);
        }
    }

    public int ApplyProtection(int rawDamage)
    {
        return Math.Max(0, rawDamage - Protection);
    }

    // Recovers d6 Health, never above starting Health. Returns the amount recovered.
    public int Rest()
    {
        if (HealthPoints >= StartingHealth)
            return 0;

        int before = HealthPoints;
        HealthPoints = Math.Min(StartingHealth, HealthPoints + _die.Roll(RestDie));
        return HealthPoints - before;
    }

    public int RollAttack()
    {
        return _die.Roll(20);
    }

    public bool AttackHits(int attackRoll)
    {
        return attackRoll <= Strength;
    }

    public int RollDamage()
    {
        if (Weapon == null)
            return 0;

        return _die.Roll(Weapon.MaxDamage);
    }

    public string Attack(Foe foe, Messages messages)
    {
        if (Weapon == null)
            return "You have no weapon to attack with.";

        var output = new List<string>();

        output.Add(string.Format(
            messages.GetMessage(foe.PlayerAttackIntroKey),
            messages.TranslateOccupationForDisplay(Occupation),
            messages.TranslateWeaponForDisplay(Weapon.Type)
        ));

        // If this fails to compile, change AsciiArt to whatever exists in Weapon.cs
        output.Add(Weapon.AsciiArt);

        int attackRoll = RollAttack();

        // MISS
        if (!AttackHits(attackRoll))
        {
            output.Add(string.Format(messages.GetMessage("player_missed_dragon"), foe.Name));
            return string.Join(Environment.NewLine, output);
        }

        output.Add(string.Format(messages.GetMessage("player_hit_dragon"), foe.Name));

        int defenseRoll = _die.Roll(20);

        // DEFENSE
        if (defenseRoll <= foe.Agility)
        {
            output.Add(string.Format(messages.GetMessage("dragon_defended_player_attack"), foe.Name));

            string? taunt = foe.GetRandomDefenseTaunt(messages);
            if (taunt != null)
                output.Add(string.Format(taunt, messages.TranslateRaceForDisplay(Race)));

            return string.Join(Environment.NewLine, output);
        }

        // DAMAGE
        int damage = RollDamage();
        foe.HealthPoints -= damage;

        output.Add(string.Format(messages.GetMessage("player_damage_dealt"), damage));
        output.Add(string.Format(messages.GetMessage("dragon_health_now"), foe.Name, foe.HealthPoints));

        // END
        if (foe.HealthPoints <= 0)
        {
            output.Add(messages.GetMessage(foe.DefeatedNarrativeKey));
        }
        else
        {
            string? reply = foe.GetRandomDamageReply(messages);
            if (reply != null)
                output.Add(reply);
        }

        return string.Join(Environment.NewLine, output);
    }

    private void PromptRoll(string prompt, Messages messages)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "roll")
                return;

            Console.WriteLine(messages.GetMessage("roll_invalid"));
        }
    }
}