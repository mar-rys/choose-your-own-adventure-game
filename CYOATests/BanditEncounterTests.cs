using System.Reflection;

[TestClass]
[DoNotParallelize]
public class BanditEncounterTests
{
    private TextReader? _originalIn;
    private TextWriter? _originalOut;

    [TestInitialize]
    public void Setup()
    {
        _originalIn = Console.In;
        _originalOut = Console.Out;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (_originalIn != null)
            Console.SetIn(_originalIn);

        if (_originalOut != null)
            Console.SetOut(_originalOut);
    }

    private static Messages GetMessages(string language)
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();
        return messages;
    }

    // Language menu number, then the Human race and Fighter occupation as typed in that language.
    private static string ScriptedNewCharacter(string language) => language switch
    {
        "Spanish" => "2\n1\nhumano\nTestHero\nguerrero\nroll\nroll\nroll\nroll\n2\n",
        "French" => "3\n1\nhumain\nTestHero\nguerrier\nroll\nroll\nroll\nroll\n2\n",
        "Italian" => "4\n1\numano\nTestHero\nguerriero\nroll\nroll\nroll\nroll\n2\n",
        _ => "1\n1\nhuman\nTestHero\nfighter\nroll\nroll\nroll\nroll\n2\n"
    };

    private static string RunGame(string input)
    {
        Console.SetIn(new StringReader(input));
        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        new Game().StartGame();
        return writer.ToString();
    }

    [TestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void GoingWest_RetreatingBeforeTheFight_ReturnsToTheCrossroads(string language)
    {
        Messages messages = GetMessages(language);

        string output = RunGame(ScriptedNewCharacter(language) + "w\nr\ne\n");

        int narrative = output.IndexOf(messages.GetMessage("west_path_narrative"));
        Assert.IsTrue(narrative >= 0, "The west-path narrative should be shown.");
        int returned = output.IndexOf(messages.GetMessage("bandit_return"), narrative);
        StringAssert.Contains(output, string.Format(messages.GetMessage("bandit_intro"), "Grimble the Bandit"));
        StringAssert.Contains(output, messages.GetMessage("bandit_stats_header"));
        StringAssert.Contains(output, messages.GetMessage("bandit_free_retreat"));
        Assert.IsTrue(returned > narrative, "The return-to-crossroads line should follow the encounter.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("path_prompt_full"), returned) > returned,
            "The path prompt should be shown again after returning.");
        StringAssert.Contains(output, messages.GetMessage("adventure_ends"));
        Assert.IsFalse(output.Contains(messages.GetMessage("path_invalid")));
    }

    private static Player CreatePlayer(int strength, int agility, int health, int gold = 50) => new Player
    {
        Name = "TestHero",
        Race = "Human",
        Occupation = "Fighter",
        Strength = strength,
        Agility = agility,
        HealthPoints = health,
        Gold = gold,
        Weapon = new Weapon("long sword", 12, "-)=====>")
    };

    private static Bandit CreateBandit(int strength, int agility, int health) =>
        new Bandit(new Die()) { Strength = strength, Agility = agility, HealthPoints = health };

    // Runs the west-path routine with scripted input. Returns whether the player survived, and what it printed.
    private static (bool survived, string output) RunEncounter(Player player, Bandit bandit, Messages messages, string input)
    {
        Console.SetIn(new StringReader(input));
        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        MethodInfo? method = typeof(Game).GetMethod(
            "HandleBanditEncounter",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        Assert.IsNotNull(method, "HandleBanditEncounter method was not found.");

        bool survived = (bool)method!.Invoke(new Game(), new object[] { player, bandit, messages })!;
        return (survived, writer.ToString());
    }

    [TestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void BanditAlreadyDefeated_ShowsTheEmptyCampAndReturns(string language)
    {
        Messages messages = GetMessages(language);
        Bandit bandit = CreateBandit(5, 5, 5);
        bandit.MarkDefeated();

        (bool survived, string output) = RunEncounter(CreatePlayer(10, 10, 10), bandit, messages, "");

        Assert.IsTrue(survived);
        StringAssert.Contains(output, messages.GetMessage("bandit_empty_camp"));
        Assert.IsFalse(output.Contains(messages.GetMessage("west_path_narrative")));
    }

    [TestMethod]
    public void RetreatingBeforeAnyBlows_CostsNothing_AndInvalidInputReprompts()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayer(10, 10, 9, gold: 75);

        (bool survived, string output) = RunEncounter(player, CreateBandit(12, 12, 12), messages, "x\nr\n");

        Assert.IsTrue(survived);
        StringAssert.Contains(output, messages.GetMessage("invalid"));
        Assert.AreEqual(2, output.Split(messages.GetMessage("bandit_encounter_prompt")).Length - 1,
            "The attack/retreat prompt should be asked again after invalid input.");
        StringAssert.Contains(output, messages.GetMessage("bandit_free_retreat"));
        StringAssert.Contains(output, messages.GetMessage("bandit_return"));
        Assert.AreEqual(75, player.Gold);
        Assert.AreEqual(9, player.HealthPoints);
    }

    [TestMethod]
    public void WinningTheFight_MarksTheBanditDefeated_AndLaterTripsFindTheEmptyCamp()
    {
        Messages messages = GetMessages("English");
        // The player always hits, and the Bandit can't block or survive one hit.
        Player player = CreatePlayer(20, 10, 10);
        Bandit bandit = CreateBandit(12, 0, 1);

        (bool survived, string output) = RunEncounter(player, bandit, messages, "a\n");

        Assert.IsTrue(survived);
        Assert.IsTrue(bandit.IsDefeated);
        StringAssert.Contains(output, messages.GetMessage("bandit_defeated_narrative"));
        StringAssert.Contains(output, messages.GetMessage("bandit_return"));
        Assert.IsFalse(output.Contains("dragon", StringComparison.OrdinalIgnoreCase),"Combat lines against the Bandit should never mention a dragon.");

        (_, string secondTrip) = RunEncounter(player, bandit, messages, "");
        StringAssert.Contains(secondTrip, messages.GetMessage("bandit_empty_camp"));
    }

    [TestMethod]
    public void RetreatingMidFight_ReturnsToTheCrossroads_AndTheBanditKeepsItsWounds()
    {
        Messages messages = GetMessages("English");
        // The player always hits but can't finish the Bandit; the Bandit always misses.
        Player player = CreatePlayer(20, 10, 10, gold: 60);
        Bandit bandit = CreateBandit(0, 0, 1000);

        (bool survived, string output) = RunEncounter(player, bandit, messages, "a\nr\n");

        Assert.IsTrue(survived);
        Assert.IsFalse(bandit.IsDefeated);
        Assert.IsTrue(bandit.HealthPoints < 1000, "The player's hit should have wounded the Bandit.");
        StringAssert.Contains(output, messages.GetMessage("bandit_return"));
        Assert.AreEqual(10, player.HealthPoints);

        int woundedHealth = bandit.HealthPoints;
        (_, string secondTrip) = RunEncounter(player, bandit, messages, "r\n");
        StringAssert.Contains(secondTrip, messages.GetMessage("stats_health") + woundedHealth);
    }

    [TestMethod]
    public void LosingTheFight_ShowsTheBanditDefeatEnding_AndEndsTheGame()
    {
        Messages messages = GetMessages("English");
        // The player never hits or blocks, and one hit of at least 1 damage takes their last Health.
        Player player = CreatePlayer(0, 0, 1);
        Bandit bandit = CreateBandit(20, 0, 1000);

        (bool survived, string output) = RunEncounter(player, bandit, messages, "a\n");

        Assert.IsFalse(survived);
        Assert.AreEqual(0, player.HealthPoints);
        StringAssert.Contains(output, messages.GetMessage("bandit_defeat"));
        Assert.IsFalse(output.Contains(messages.GetMessage("bandit_return")));
    }

    [TestMethod]
    public void BanditHits_AreReducedByProtection()
    {
        Messages messages = GetMessages("English");
        // Protection 6 absorbs every short-sword hit (max 6), so the player takes no damage.
        Player player = CreatePlayer(0, 0, 5);
        player.Armor = new Armor("magic armor", 6);
        Bandit bandit = CreateBandit(20, 0, 1000);

        bandit.Attack(player, messages);

        Assert.AreEqual(5, player.HealthPoints);
    }
}
