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
    [DataRow("long sword", 12, "magic long sword", 16)]
    [DataRow("lightning bolt spell", 12, "magic lightning bolt spell", 16)]
    [DataRow("dagger", 6, "magic dagger", 10)]
    [DataRow("long bow", 8, "magic long bow", 12)]
    public void BuyMagicWeapon_WithEnoughGold_Takes120GoldAndEnchantsTheWeapon(
        string weaponType, int maxDamage, string magicType, int magicMaxDamage)
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(150);
        player.Weapon = new Weapon(weaponType, maxDamage, "art");

        PurchaseResult result = merchant.BuyMagicWeapon(player);

        Assert.AreEqual(PurchaseResult.Success, result);
        Assert.AreEqual(30, player.Gold);
        Assert.AreEqual(magicType, player.Weapon!.Type);
        Assert.AreEqual(magicMaxDamage, player.Weapon.MaxDamage);
        Assert.AreEqual("art", player.Weapon.AsciiArt);
    }

    [TestMethod]
    public void BuyMagicWeapon_WithTooLittleGold_LeavesPlayerUnchanged()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(119);

        PurchaseResult result = merchant.BuyMagicWeapon(player);

        Assert.AreEqual(PurchaseResult.CantAfford, result);
        Assert.AreEqual(119, player.Gold);
        Assert.AreEqual(0, player.Protection);
        Assert.AreEqual("long sword", player.Weapon!.Type);
        Assert.AreEqual(12, player.Weapon.MaxDamage);
        Assert.IsFalse(merchant.EncounterUsed);
    }

    [TestMethod]
    public void BuyMagicWeapon_WithExactly120Gold_Succeeds()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(120);

        PurchaseResult result = merchant.BuyMagicWeapon(player);

        Assert.AreEqual(PurchaseResult.Success, result);
        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual("magic long sword", player.Weapon!.Type);
        Assert.AreEqual(16, player.Weapon.MaxDamage);
    }

    [TestMethod]
    public void BuyMagicWeapon_AfterBuyingArmor_IsRefusedAndPlayerUnchanged()
    {
        Merchant merchant = new Merchant();
        Player player = CreatePlayerWithGold(200);
        merchant.BuyArmor(player);

        PurchaseResult result = merchant.BuyMagicWeapon(player);

        Assert.AreEqual(PurchaseResult.EncounterUsed, result);
        Assert.AreEqual(120, player.Gold);
        Assert.AreEqual("long sword", player.Weapon!.Type);
        Assert.AreEqual(12, player.Weapon.MaxDamage);
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
