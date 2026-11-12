using System.Reflection;

[TestClass]
[DoNotParallelize]
public class AdventureLoopTests
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

    // Runs the adventure loop with scripted input and returns everything it printed.
    private static string RunAdventure(Player player, Merchant merchant, Bandit bandit, Dragon dragon, Messages messages, string input)
    {
        Console.SetIn(new StringReader(input));
        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        MethodInfo? method = typeof(Game).GetMethod(
            "RunAdventure",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        Assert.IsNotNull(method, "RunAdventure method was not found.");

        method!.Invoke(new Game(), new object[] { player, merchant, bandit, dragon, messages });
        return writer.ToString();
    }

    // The player always wounds the Bandit (which never blocks) but can't finish it, never blocks,
    // and one Bandit hit of at least 1 damage takes their last Health.
    private static Player CreatePlayerCertainToLose() => CreatePlayer(20, 0, 1, gold: 50);
    private static Bandit CreateBanditCertainToWin() => CreateBandit(20, 0, 1000);

    [TestMethod]
    public void ABanditDefeat_ThenReturningToTheCheckpoint_PutsThePlayerBackAtTheCrossroads()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayerCertainToLose();
        Bandit bandit = CreateBanditCertainToWin();

        string output = RunAdventure(player, new Merchant(), bandit, new Dragon(new Die()), messages, "w\na\n1\ne\n");

        int defeat = output.IndexOf(messages.GetMessage("bandit_defeat"));
        Assert.IsTrue(defeat >= 0, "The Bandit defeat ending should be shown.");
        int prompt = output.IndexOf(messages.GetMessage("defeat_retry_prompt"), defeat);
        Assert.IsTrue(prompt > defeat, "The return prompt should follow the defeat ending.");
        int awoken = output.IndexOf(string.Format(messages.GetMessage("checkpoint_return"), 1, 50), prompt);
        Assert.IsTrue(awoken > prompt, "The awakening line, with the Health and Gold from before the fight, should follow.");
        int pathPrompt = output.IndexOf(messages.GetMessage("path_prompt_full"), awoken);
        Assert.IsTrue(pathPrompt > awoken, "The path prompt should follow the awakening line.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("adventure_ends"), pathPrompt) > pathPrompt);

        Assert.AreEqual(1, player.HealthPoints);
        Assert.AreEqual(1000, bandit.HealthPoints);
    }

    [TestMethod]
    public void ABanditDefeat_ThenEndingTheAdventure_EndsTheGameWithNoAwakening()
    {
        Messages messages = GetMessages("English");

        // The trailing `e` would end the adventure if the loop wrongly asked for a path again.
        string output = RunAdventure(CreatePlayerCertainToLose(), new Merchant(), CreateBanditCertainToWin(),
            new Dragon(new Die()), messages, "w\na\n2\ne\n");

        int defeat = output.IndexOf(messages.GetMessage("bandit_defeat"));
        Assert.IsTrue(defeat >= 0, "The Bandit defeat ending should be shown.");
        int prompt = output.IndexOf(messages.GetMessage("defeat_retry_prompt"), defeat);
        Assert.IsTrue(prompt > defeat, "The return prompt should follow the defeat ending.");
        Assert.IsFalse(output.Contains(string.Format(messages.GetMessage("checkpoint_return"), 1, 50)),
            "No awakening line should be shown.");
        Assert.AreEqual(-1, output.IndexOf(messages.GetMessage("path_prompt_full"), prompt),
            "No path prompt should follow the choice to end the adventure.");
        Assert.IsFalse(output.Contains(messages.GetMessage("adventure_ends")),
            "The loop should return without reading another path.");
    }

    [TestMethod]
    public void InvalidInputAtTheReturnPrompt_ShowsTheInvalidMessage_AndAsksAgain()
    {
        Messages messages = GetMessages("English");

        string output = RunAdventure(CreatePlayerCertainToLose(), new Merchant(), CreateBanditCertainToWin(),
            new Dragon(new Die()), messages, "w\na\nx\n2\n");

        int firstPrompt = output.IndexOf(messages.GetMessage("defeat_retry_prompt"));
        Assert.IsTrue(firstPrompt >= 0, "The return prompt should be shown.");
        int invalid = output.IndexOf(messages.GetMessage("invalid"), firstPrompt);
        Assert.IsTrue(invalid > firstPrompt, "The invalid-input message should follow the bad choice.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("defeat_retry_prompt"), invalid) > invalid,
            "The return prompt should be asked again.");
    }

    [TestMethod]
    public void TwoBanditDefeatsInARow_EachFollowedByAReturn_BothComeBackToTheCrossroads()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayerCertainToLose();
        Bandit bandit = CreateBanditCertainToWin();

        string output = RunAdventure(player, new Merchant(), bandit, new Dragon(new Die()), messages,
            "w\na\n1\nw\na\n1\ne\n");

        string awakening = string.Format(messages.GetMessage("checkpoint_return"), 1, 50);
        Assert.AreEqual(2, output.Split(awakening).Length - 1, "Both Defeats should end with an awakening.");
        int secondAwakening = output.LastIndexOf(awakening);
        Assert.IsTrue(output.IndexOf(messages.GetMessage("adventure_ends"), secondAwakening) > secondAwakening,
            "The player should be back at the path prompt after the second return.");
        Assert.AreEqual(1, player.HealthPoints);
        Assert.AreEqual(1000, bandit.HealthPoints);
    }

    [TestMethod]
    public void AFreeRetreatFromTheBandit_ReturnsToTheCrossroads_WithNoReturnPrompt()
    {
        Messages messages = GetMessages("English");

        string output = RunAdventure(CreatePlayer(10, 10, 10), new Merchant(), CreateBandit(10, 10, 10),
            new Dragon(new Die()), messages, "w\nr\ne\n");

        int returned = output.IndexOf(messages.GetMessage("bandit_return"));
        Assert.IsTrue(returned >= 0, "The player should walk back to the crossroads.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("adventure_ends"), returned) > returned);
        Assert.IsFalse(output.Contains(messages.GetMessage("defeat_retry_prompt")),
            "A retreat is never a Defeat, so no return prompt should be shown.");
    }

    [TestMethod]
    public void AMidFightRetreatFromTheBandit_ReturnsToTheCrossroads_WithNoReturnPrompt()
    {
        Messages messages = GetMessages("English");
        // The player always hits but can't finish the Bandit; the Bandit always misses.
        Player player = CreatePlayer(20, 10, 10, gold: 60);
        Bandit bandit = CreateBandit(0, 0, 1000);

        string output = RunAdventure(player, new Merchant(), bandit, new Dragon(new Die()), messages, "w\na\nr\ne\n");

        int returned = output.IndexOf(messages.GetMessage("bandit_return"));
        Assert.IsTrue(returned >= 0, "The player should walk back to the crossroads.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("adventure_ends"), returned) > returned);
        Assert.IsFalse(output.Contains(messages.GetMessage("defeat_retry_prompt")),
            "A retreat is never a Defeat, so no return prompt should be shown.");
    }

    [TestMethod]
    public void GoingSouthThenExiting_VisitsTheMerchant_ReturnsToThePathPrompt_ThenEndsTheAdventure()
    {
        Messages messages = GetMessages("English");
        // Too little Gold to buy anything, so the Merchant visit needs no input of its own.
        Player player = CreatePlayer(10, 10, 10, gold: 0);

        string output = RunAdventure(player, new Merchant(), CreateBandit(10, 10, 10), new Dragon(new Die()), messages, "s\ne\n");

        int intro = output.IndexOf(messages.GetMessage("merchant_intro"));
        Assert.IsTrue(intro >= 0, "The Merchant should be visited.");
        int returned = output.IndexOf(messages.GetMessage("merchant_return"), intro);
        Assert.IsTrue(returned > intro, "The return-to-crossroads line should follow the Merchant visit.");
        int prompt = output.IndexOf(messages.GetMessage("path_prompt_full"), returned);
        Assert.IsTrue(prompt > returned, "The path prompt should be shown again after the Merchant.");
        Assert.IsTrue(output.IndexOf(messages.GetMessage("adventure_ends"), prompt) > prompt,
            "The adventure-ends line should follow the second path prompt.");
    }
}
