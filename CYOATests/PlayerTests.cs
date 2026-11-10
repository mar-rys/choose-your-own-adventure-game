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
