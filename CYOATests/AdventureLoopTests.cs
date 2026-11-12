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

    [TestMethod]
    public void ADefeatByTheBandit_ShowsTheBanditDefeatEnding_AndTheLoopReturns()
    {
        Messages messages = GetMessages("English");
        // The player never hits or blocks, and one hit of at least 1 damage takes their last Health.
        Player player = CreatePlayer(0, 0, 1);
        Bandit bandit = CreateBandit(20, 0, 1000);

        // The trailing `e` would end the adventure if the loop wrongly asked for a path again.
        string output = RunAdventure(player, new Merchant(), bandit, new Dragon(new Die()), messages, "w\na\ne\n");

        int defeat = output.IndexOf(messages.GetMessage("bandit_defeat"));
        Assert.IsTrue(defeat >= 0, "The Bandit defeat ending should be shown.");
        Assert.AreEqual(-1, output.IndexOf(messages.GetMessage("path_prompt_full"), defeat),
            "No path prompt should follow the Bandit defeat ending.");
        Assert.IsFalse(output.Contains(messages.GetMessage("adventure_ends")),
            "The loop should return after the Defeat without reading another path.");
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
