[TestClass]
public class BanditTests
{
    [TestMethod]
    public void NewBandit_IsGrimbleWithD12StatsAndAShortSword()
    {
        Bandit bandit = new Bandit(new Die());

        Assert.AreEqual("Grimble the Bandit", bandit.Name);
        Assert.IsTrue(bandit.Strength >= 1 && bandit.Strength <= 12);
        Assert.IsTrue(bandit.Agility >= 1 && bandit.Agility <= 12);
        Assert.IsTrue(bandit.HealthPoints >= 1 && bandit.HealthPoints <= 12);

        Assert.IsNotNull(bandit.Weapon);
        Assert.AreEqual("short sword", bandit.Weapon.Type);
        Assert.AreEqual(6, bandit.Weapon.MaxDamage);
        Assert.IsFalse(string.IsNullOrWhiteSpace(bandit.Weapon.AsciiArt));
    }

    [TestMethod]
    public void NewBandit_IsNotDefeated()
    {
        Bandit bandit = new Bandit(new Die());

        Assert.IsFalse(bandit.IsDefeated);
    }

    private static Player CreatePlayerWithGold(int gold) => new Player
    {
        Name = "TestHero",
        Race = "Human",
        Occupation = "Fighter",
        Gold = gold,
        Weapon = new Weapon("long sword", 12, "-)=====>")
    };

    [TestMethod]
    public void Loot_Gives10To100GoldInTens_ReturnsIt_AndMarksTheBanditDefeated()
    {
        Bandit bandit = new Bandit(new Die());
        Player player = CreatePlayerWithGold(25);

        int looted = bandit.Loot(player);

        Assert.IsTrue(looted >= 10 && looted <= 100, $"Looted {looted}, expected 10 to 100.");
        Assert.AreEqual(0, looted % 10, $"Looted {looted}, expected a multiple of 10.");
        Assert.AreEqual(25 + looted, player.Gold);
        Assert.IsTrue(bandit.IsDefeated);
    }

    [TestMethod]
    [DataRow(75, 37, 38)]
    [DataRow(100, 50, 50)]
    [DataRow(1, 0, 1)]
    [DataRow(0, 0, 0)]
    public void TakeToll_TakesHalfTheGoldRoundedDown_AndReturnsIt(int gold, int expectedTaken, int expectedLeft)
    {
        Bandit bandit = new Bandit(new Die());
        Player player = CreatePlayerWithGold(gold);

        int taken = bandit.TakeToll(player);

        Assert.AreEqual(expectedTaken, taken);
        Assert.AreEqual(expectedLeft, player.Gold);
    }

    [TestMethod]
    public void TakeToll_DoesNotMarkTheBanditDefeated()
    {
        Bandit bandit = new Bandit(new Die());

        bandit.TakeToll(CreatePlayerWithGold(75));

        Assert.IsFalse(bandit.IsDefeated);
    }
}
