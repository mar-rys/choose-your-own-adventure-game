using System.Reflection;

[TestClass]
[DoNotParallelize]
public class MerchantEncounterTests
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

    private static Player CreatePlayerWithGold(int gold)
    {
        return new Player
        {
            Name = "TestHero",
            Race = "Human",
            Occupation = "Fighter",
            Gold = gold,
            Weapon = new Weapon("long sword", 12, "-)=====>")
        };
    }

    // Runs the south-path routine with scripted input and returns everything it printed.
    private static string RunEncounter(Player player, Merchant merchant, Messages messages, string input)
    {
        Console.SetIn(new StringReader(input));
        using StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        MethodInfo? method = typeof(Game).GetMethod(
            "HandleMerchantEncounter",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        Assert.IsNotNull(method, "HandleMerchantEncounter method was not found.");

        method!.Invoke(new Game(), new object[] { player, merchant, messages });
        return writer.ToString();
    }

    [TestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void MerchantEncounter_BuyingArmor_RunsWithoutThrowing(string language)
    {
        Messages messages = GetMessages(language);
        Player player = CreatePlayerWithGold(100);
        Merchant merchant = new Merchant();

        // An invalid choice first, then 1 to buy Armor.
        string output = RunEncounter(player, merchant, messages, "x\n1\n");

        StringAssert.Contains(output, messages.GetMessage("merchant_intro"));
        StringAssert.Contains(output, string.Format(messages.GetMessage("merchant_offer"), 100, Merchant.ArmorPrice));
        StringAssert.Contains(output, messages.GetMessage("merchant_invalid"));
        StringAssert.Contains(output, messages.GetMessage("merchant_return"));
        Assert.AreEqual(20, player.Gold);
        Assert.AreEqual(3, player.Protection);
    }

    [TestMethod]
    public void MerchantEncounter_Leaving_BuysNothingAndEndsTheEncounter()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayerWithGold(100);
        Merchant merchant = new Merchant();

        string output = RunEncounter(player, merchant, messages, "3\n");

        StringAssert.Contains(output, messages.GetMessage("merchant_return"));
        Assert.AreEqual(100, player.Gold);
        Assert.AreEqual(0, player.Protection);
        Assert.IsTrue(merchant.EncounterUsed);
    }

    [TestMethod]
    public void MerchantEncounter_CantAffordAnything_EndsWithoutTheMenu()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayerWithGold(50);
        Merchant merchant = new Merchant();

        string output = RunEncounter(player, merchant, messages, "");

        StringAssert.Contains(output, messages.GetMessage("merchant_cant_afford_anything"));
        StringAssert.Contains(output, messages.GetMessage("merchant_return"));
        Assert.IsFalse(output.Contains(messages.GetMessage("merchant_menu")));
        Assert.AreEqual(50, player.Gold);
        Assert.IsTrue(merchant.EncounterUsed);
    }

    [TestMethod]
    public void MerchantEncounter_SecondTrip_ShowsMovedOnAndAllowsNoPurchase()
    {
        Messages messages = GetMessages("English");
        Player player = CreatePlayerWithGold(200);
        Merchant merchant = new Merchant();
        RunEncounter(player, merchant, messages, "3\n");

        string output = RunEncounter(player, merchant, messages, "1\n");

        StringAssert.Contains(output, messages.GetMessage("merchant_moved_on"));
        Assert.IsFalse(output.Contains(messages.GetMessage("merchant_intro")));
        Assert.AreEqual(200, player.Gold);
        Assert.AreEqual(0, player.Protection);
    }
}
