using System.IO;

[TestClass]
public class PlayerTests
{
    [TestMethod]
    public void NewPlayer_ShouldHaveDefaultValues()
    {
        Player player = new Player();

        Assert.AreEqual("", player.Name);
        Assert.AreEqual("", player.Race);
        Assert.AreEqual("", player.Occupation);

        Assert.AreEqual(0, player.Strength);
        Assert.AreEqual(0, player.Agility);
        Assert.AreEqual(0, player.HealthPoints);

        Assert.IsNull(player.Weapon);
    }

    [TestMethod]
    public void ApplyProtection_WithNoArmor_TakesFullDamage()
    {
        Player player = new Player();

        Assert.AreEqual(12, player.ApplyProtection(12));
    }

    [TestMethod]
    public void ApplyProtection_WithProtection3_Reduces12DamageTo9()
    {
        Player player = new Player { Armor = new Armor("magic armor", 3) };

        Assert.AreEqual(9, player.ApplyProtection(12));
    }

    [TestMethod]
    public void ApplyProtection_NeverReturnsNegativeDamage()
    {
        Player player = new Player { Armor = new Armor("magic armor", 3) };

        Assert.AreEqual(0, player.ApplyProtection(2));
    }

    [TestMethod]
    public void Rest_OneBelowStartingHealth_RecoversExactlyToStartingHealth()
    {
        Player player = new Player { StartingHealth = 10, HealthPoints = 9 };

        int recovered = player.Rest();

        Assert.AreEqual(10, player.HealthPoints);
        Assert.AreEqual(1, recovered);
    }

    [TestMethod]
    public void Rest_AtFullHealth_ChangesNothingAndReturns0()
    {
        Player player = new Player { StartingHealth = 15, HealthPoints = 15 };

        int recovered = player.Rest();

        Assert.AreEqual(15, player.HealthPoints);
        Assert.AreEqual(0, recovered);
    }

    [TestMethod]
    public void Rest_FarBelowStartingHealth_RecoversOneToSix()
    {
        Player player = new Player { StartingHealth = 20, HealthPoints = 1 };

        int recovered = player.Rest();

        Assert.IsTrue(player.HealthPoints >= 2 && player.HealthPoints <= 7, $"Health was {player.HealthPoints}");
        Assert.AreEqual(player.HealthPoints - 1, recovered);
    }
}

[TestClass]
[DoNotParallelize]
public class PlayerCharacterCreationTests
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

    private static Messages GetMessages(string language = "English")
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();
        return messages;
    }

    private static string RunScriptedCreation(Player player, Messages messages, string input)
    {
        Console.SetIn(new StringReader(input));
        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        player.CreateCharacter(messages);

        return writer.ToString();
    }

    private const string ScriptedCreation = "Human\nTestHero\nFighter\nroll\nroll\nroll\nroll\n2\n";

    [TestMethod]
    public void CreateCharacter_RollsGoldBetween10And200()
    {
        Player player = new Player();

        RunScriptedCreation(player, GetMessages(), ScriptedCreation);

        Assert.IsTrue(player.Gold >= 10 && player.Gold <= 200, $"Gold was {player.Gold}");
    }

    [TestMethod]
    public void CreateCharacter_RecordsStartingHealthEqualToRolledHealth()
    {
        Player player = new Player();

        RunScriptedCreation(player, GetMessages(), ScriptedCreation);

        Assert.AreNotEqual(0, player.StartingHealth);
        Assert.AreEqual(player.HealthPoints, player.StartingHealth);
    }

    [TestMethod]
    public void CreateCharacter_StartsWithNoArmorAndProtection0()
    {
        Player player = new Player();

        RunScriptedCreation(player, GetMessages(), ScriptedCreation);

        Assert.IsNull(player.Armor);
        Assert.AreEqual(0, player.Protection);
    }

    [TestMethod]
    public void CreateCharacter_ShowsTheRolledGold()
    {
        Player player = new Player();
        Messages messages = GetMessages();

        string output = RunScriptedCreation(player, messages, ScriptedCreation);

        StringAssert.Contains(output, string.Format(messages.GetMessage("gold_rolled"), player.Gold));
    }

    [TestMethod]
    public void DisplayStats_ShowsGoldLine()
    {
        Player player = new Player { Name = "TestHero", Race = "Human", Occupation = "Fighter", Gold = 137 };
        Messages messages = GetMessages();

        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        player.DisplayStats(messages);

        StringAssert.Contains(writer.ToString(), messages.GetMessage("stats_gold") + 137);
    }

    [TestMethod]
    public void DisplayStats_ShowsProtectionLine()
    {
        Player player = new Player { Name = "TestHero", Race = "Human", Occupation = "Fighter", Armor = new Armor("magic armor", 3) };
        Messages messages = GetMessages();

        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        player.DisplayStats(messages);

        StringAssert.Contains(writer.ToString(), messages.GetMessage("stats_protection") + 3);
    }

    [TestMethod]
    public void DisplayStats_ShowsArmorNameInPlayersLanguage()
    {
        Player player = new Player { Name = "TestHero", Race = "Human", Occupation = "Fighter", Armor = new Armor("magic armor", 3) };
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Spanish");
        messages.ReadDictionary();

        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        player.DisplayStats(messages);

        StringAssert.Contains(writer.ToString(), "Armadura: armadura magica");
    }

    [TestMethod]
    public void CreateCharacter_RepromptsForGoldRollOnInvalidInput()
    {
        Player player = new Player();
        Messages messages = GetMessages();

        string output = RunScriptedCreation(player, messages,
            "Human\nTestHero\nFighter\nroll\nroll\nroll\nnope\nroll\n2\n");

        Assert.IsTrue(player.Gold >= 10 && player.Gold <= 200, $"Gold was {player.Gold}");
        StringAssert.Contains(output, messages.GetMessage("roll_invalid"));
    }
}
