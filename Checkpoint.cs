// A record of the player and the world as they were at the crossroads. No console input or output.
// Everything is copied as a value, and Restore writes back into the same objects it is given.
public class Checkpoint
{
    private readonly int _playerHealth;
    private readonly int _playerGold;
    // The Weapon is changed in place by the Magic weapon purchase, so its values are copied.
    private readonly string? _weaponType;
    private readonly int _weaponMaxDamage;
    private readonly Armor? _playerArmor; // Armor is immutable, so keeping the reference is safe.
    private readonly bool _merchantEncounterUsed;
    private readonly int _banditHealth;
    private readonly bool _banditDefeated;
    private readonly int _dragonHealth;

    private Checkpoint(Player player, Merchant merchant, Bandit bandit, Dragon dragon)
    {
        _playerHealth = player.HealthPoints;
        _playerGold = player.Gold;
        _weaponType = player.Weapon?.Type;
        _weaponMaxDamage = player.Weapon?.MaxDamage ?? 0;
        _playerArmor = player.Armor;
        _merchantEncounterUsed = merchant.EncounterUsed;
        _banditHealth = bandit.HealthPoints;
        _banditDefeated = bandit.IsDefeated;
        _dragonHealth = dragon.HealthPoints;
    }

    public static Checkpoint Take(Player player, Merchant merchant, Bandit bandit, Dragon dragon) =>
        new Checkpoint(player, merchant, bandit, dragon);

    public void Restore(Player player, Merchant merchant, Bandit bandit, Dragon dragon)
    {
        player.HealthPoints = _playerHealth;
        player.Gold = _playerGold;
        if (player.Weapon != null && _weaponType != null)
        {
            player.Weapon.Type = _weaponType;
            player.Weapon.MaxDamage = _weaponMaxDamage;
        }
        player.Armor = _playerArmor;
        merchant.RestoreEncounterUsed(_merchantEncounterUsed);
        bandit.HealthPoints = _banditHealth;
        bandit.RestoreDefeated(_banditDefeated);
        dragon.HealthPoints = _dragonHealth;
    }
}
