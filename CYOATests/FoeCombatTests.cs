[TestClass]
public class FoeCombatTests
{
    // A Foe with its own wording keys and no taunts or damage replies.
    // Missing keys render as "[key]", so its lines are easy to spot.
    private class TrainingDummy : Foe
    {
        public TrainingDummy() : base(new Die())
        {
            Name = "Dummy";
            Weapon = new Weapon("stick", 4, "--");
        }

        public override string StatsHeaderKey => "dummy_stats_header";
        public override string CombatHeaderKey => "dummy_combat_header";
        public override string PlayerAttackIntroKey => "dummy_attack_intro";
        public override string DefeatedNarrativeKey => "dummy_defeated_narrative";
    }

    private static Messages EnglishMessages() => MessagesFor("English");

    private static Messages MessagesFor(string language)
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();
        return messages;
    }

    [TestMethod]
    public void CombatStatsDisplay_UsesTheFoesHeader_NotTheDragons()
    {
        Messages messages = EnglishMessages();
        Player player = new Player { Name = "TestHero", Weapon = new Weapon("long sword", 12, "-)=====>") };
        Combat combat = new Combat(player, new TrainingDummy(), messages);

        string result = combat.GetCombatStatsDisplay();

        StringAssert.Contains(result, "[dummy_combat_header]");
        StringAssert.Contains(result, "Dummy");
        Assert.IsFalse(result.Contains("--- Dragon ---"));
    }

    [DataTestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void CombatStatsDisplay_AgainstTheBandit_ShowsItsNameAndHeader_NotTheDragons(string language)
    {
        Messages messages = MessagesFor(language);
        Player player = new Player { Name = "TestHero", Weapon = new Weapon("long sword", 12, "-)=====>") };
        Combat combat = new Combat(player, new Bandit(new Die()), messages);

        string result = combat.GetCombatStatsDisplay();

        string banditHeader = messages.GetMessage("combat_bandit_header");
        Assert.AreNotEqual("[combat_bandit_header]", banditHeader);
        StringAssert.Contains(result, banditHeader);
        StringAssert.Contains(result, "Grimble the Bandit");
        StringAssert.Contains(result, messages.TranslateWeaponForDisplay("short sword"));
        Assert.IsFalse(result.Contains(messages.GetMessage("combat_dragon_header")));
    }

    // Strength 20 always hits (d20 <= 20).
    private static Player SureHitPlayer() => new Player
    {
        Name = "TestHero",
        Race = "Human",
        Occupation = "Fighter",
        Strength = 20,
        Weapon = new Weapon("long sword", 12, "-)=====>")
    };

    private static string[] Lines(string text) => text.Split(Environment.NewLine);

    [TestMethod]
    public void PlayerAttack_FoeWithoutTaunt_Blocks_EndsAtTheDefendLine()
    {
        Messages messages = EnglishMessages();
        // Agility 20 always blocks.
        TrainingDummy dummy = new TrainingDummy { Agility = 20, HealthPoints = 10 };

        string[] lines = Lines(SureHitPlayer().Attack(dummy, messages));

        CollectionAssert.AreEqual(new[]
        {
            "[dummy_attack_intro]",
            "-)=====>",
            "You hit Dummy.",
            "Dummy successfully defends your attack."
        }, lines);
    }

    [TestMethod]
    public void PlayerAttack_FoeWithoutReply_Wounded_EndsAtTheHealthLine()
    {
        Messages messages = EnglishMessages();
        // Agility 0 never blocks; 1000 Health survives any sword hit.
        TrainingDummy dummy = new TrainingDummy { Agility = 0, HealthPoints = 1000 };

        string[] lines = Lines(SureHitPlayer().Attack(dummy, messages));

        Assert.AreEqual(5, lines.Length);
        StringAssert.StartsWith(lines[4], "Dummy's health points are now ");
    }

    [TestMethod]
    public void PlayerAttack_FoeDefeated_UsesTheFoesDefeatedNarrative()
    {
        Messages messages = EnglishMessages();
        TrainingDummy dummy = new TrainingDummy { Agility = 0, HealthPoints = 1 };

        string[] lines = Lines(SureHitPlayer().Attack(dummy, messages));

        Assert.AreEqual("[dummy_defeated_narrative]", lines[^1]);
    }

    [DataTestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void PlayerAttack_DragonBlocks_StillUsesItsIntroAndTaunts(string language)
    {
        Messages messages = MessagesFor(language);
        Player player = SureHitPlayer();
        Dragon dragon = new Dragon(new Die()) { Agility = 20 };

        string[] lines = Lines(player.Attack(dragon, messages));

        string race = messages.TranslateRaceForDisplay(player.Race);
        string[] taunts = Enumerable.Range(1, 4)
            .Select(i => string.Format(messages.GetMessage($"dragon_defense_taunt_{i}"), race))
            .ToArray();

        Assert.AreEqual(5, lines.Length);
        Assert.AreEqual(string.Format(messages.GetMessage("player_attack_intro"),
            messages.TranslateOccupationForDisplay(player.Occupation),
            messages.TranslateWeaponForDisplay(player.Weapon!.Type)), lines[0]);
        CollectionAssert.Contains(taunts, lines[4]);
    }

    [DataTestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void PlayerAttack_DragonDefeated_StillUsesItsDefeatedNarrative(string language)
    {
        Messages messages = MessagesFor(language);
        Dragon dragon = new Dragon(new Die()) { Agility = 0, HealthPoints = 1 };

        string[] lines = Lines(SureHitPlayer().Attack(dragon, messages));

        Assert.AreEqual(messages.GetMessage("dragon_defeated_narrative"), lines[^1]);
    }
}
