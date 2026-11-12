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

    [TestMethod]
    public void MarkDefeated_MarksTheBanditDefeated()
    {
        Bandit bandit = new Bandit(new Die());

        bandit.MarkDefeated();

        Assert.IsTrue(bandit.IsDefeated);
    }
}
