[TestClass]
public class CheckpointTests
{
    private static Player CreatePlayer() => new Player
    {
        Name = "TestHero",
        Race = "Human",
        Occupation = "Fighter",
        Strength = 10,
        Agility = 10,
        HealthPoints = 14,
        StartingHealth = 18,
        Gold = 150,
        Weapon = new Weapon("long sword", 12, "-)=====>")
    };

    private static Bandit CreateBandit() =>
        new Bandit(new Die()) { Strength = 8, Agility = 6, HealthPoints = 9 };

    private static Dragon CreateDragon() =>
        new Dragon(new Die()) { Strength = 15, Agility = 12, HealthPoints = 17 };

    [TestMethod]
    public void Restore_PutsBackHealthGoldArmorTheMerchantTheBanditAndTheDragon()
    {
        Player player = CreatePlayer();
        Armor armor = new Armor("magic armor", 3);
        player.Armor = armor;
        Merchant merchant = new Merchant();
        Bandit bandit = CreateBandit();
        Dragon dragon = CreateDragon();

        Checkpoint checkpoint = Checkpoint.Take(player, merchant, bandit, dragon);

        player.HealthPoints = 2;
        player.Gold = 5;
        player.Armor = new Armor("other armor", 6);
        merchant.Leave();
        bandit.HealthPoints = -3;
        bandit.Loot(player);
        dragon.HealthPoints = 4;

        checkpoint.Restore(player, merchant, bandit, dragon);

        Assert.AreEqual(14, player.HealthPoints);
        Assert.AreEqual(150, player.Gold);
        Assert.AreSame(armor, player.Armor);
        Assert.AreEqual(3, player.Protection);
        Assert.IsFalse(merchant.EncounterUsed);
        Assert.AreEqual(9, bandit.HealthPoints);
        Assert.IsFalse(bandit.IsDefeated);
        Assert.AreEqual(17, dragon.HealthPoints);
    }

    [TestMethod]
    public void Restore_UndoesAnInPlaceMagicWeaponChange()
    {
        Player player = CreatePlayer();
        Checkpoint checkpoint = Checkpoint.Take(player, new Merchant(), CreateBandit(), CreateDragon());

        // The Magic weapon purchase changes the Weapon object itself.
        player.Weapon!.MaxDamage += 4;
        player.Weapon.Type = "magic " + player.Weapon.Type;

        checkpoint.Restore(player, new Merchant(), CreateBandit(), CreateDragon());

        Assert.AreEqual("long sword", player.Weapon!.Type);
        Assert.AreEqual(12, player.Weapon.MaxDamage);
    }

    [TestMethod]
    public void Restore_WritesIntoTheSameObjectsItWasGiven()
    {
        Player player = CreatePlayer();
        Weapon weapon = player.Weapon!;
        Merchant merchant = new Merchant();
        Bandit bandit = CreateBandit();
        Dragon dragon = CreateDragon();
        Checkpoint checkpoint = Checkpoint.Take(player, merchant, bandit, dragon);

        weapon.MaxDamage = 99;
        bandit.HealthPoints = 1;
        dragon.HealthPoints = 1;
        merchant.Leave();

        checkpoint.Restore(player, merchant, bandit, dragon);

        Assert.AreSame(weapon, player.Weapon);
        Assert.AreEqual(12, weapon.MaxDamage);
        Assert.AreEqual(9, bandit.HealthPoints);
        Assert.AreEqual(17, dragon.HealthPoints);
        Assert.IsFalse(merchant.EncounterUsed);
    }

    [TestMethod]
    public void Restore_TakenWithNoArmor_PutsBackNoArmor()
    {
        Player player = CreatePlayer();
        Checkpoint checkpoint = Checkpoint.Take(player, new Merchant(), CreateBandit(), CreateDragon());

        player.Armor = new Armor("magic armor", 3);

        checkpoint.Restore(player, new Merchant(), CreateBandit(), CreateDragon());

        Assert.IsNull(player.Armor);
        Assert.AreEqual(0, player.Protection);
    }

    [TestMethod]
    public void Restore_TakenAfterTheMerchantIsUsedAndTheBanditLooted_KeepsBothFlagsSet()
    {
        Player player = CreatePlayer();
        Merchant merchant = new Merchant();
        Bandit bandit = CreateBandit();
        merchant.BuyArmor(player);
        bandit.Loot(player);
        Checkpoint checkpoint = Checkpoint.Take(player, merchant, bandit, CreateDragon());

        checkpoint.Restore(player, merchant, bandit, CreateDragon());

        Assert.IsTrue(merchant.EncounterUsed);
        Assert.IsTrue(bandit.IsDefeated);
    }
}
