[TestClass]
public class MerchantTests
{
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

    [TestMethod]
    public void BuyArmor_WithEnoughGold_Takes80GoldAndGivesProtection3()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(150);

        PurchaseResult result = merchant.BuyArmor(player);

        Assert.AreEqual(PurchaseResult.Success, result);
        Assert.AreEqual(70, player.Gold);
        Assert.AreEqual(3, player.Protection);
        Assert.AreEqual("magic armor", player.Armor!.Name);
    }

    [TestMethod]
    public void BuyArmor_WithTooLittleGold_LeavesPlayerUnchanged()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(79);

        PurchaseResult result = merchant.BuyArmor(player);

        Assert.AreEqual(PurchaseResult.CantAfford, result);
        Assert.AreEqual(79, player.Gold);
        Assert.AreEqual(0, player.Protection);
        Assert.IsNull(player.Armor);
    }

    [TestMethod]
    public void BuyArmor_WithExactly80Gold_Succeeds()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(80);

        PurchaseResult result = merchant.BuyArmor(player);

        Assert.AreEqual(PurchaseResult.Success, result);
        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual(3, player.Protection);
    }

    [TestMethod]
    public void BuyArmor_AfterAPurchase_IsRefusedAndPlayerUnchanged()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(200);
        merchant.BuyArmor(player);

        PurchaseResult result = merchant.BuyArmor(player);

        Assert.AreEqual(PurchaseResult.EncounterUsed, result);
        Assert.AreEqual(120, player.Gold);
        Assert.AreEqual(3, player.Protection);
    }

    [TestMethod]
    public void BuyArmor_AfterLeaving_IsRefusedAndPlayerUnchanged()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(200);
        merchant.Leave();

        PurchaseResult result = merchant.BuyArmor(player);

        Assert.AreEqual(PurchaseResult.EncounterUsed, result);
        Assert.AreEqual(200, player.Gold);
        Assert.AreEqual(0, player.Protection);
        Assert.IsNull(player.Armor);
    }
}
