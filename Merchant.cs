public enum PurchaseResult
{
    Success,
    CantAfford,
    EncounterUsed
}

// Holds every buying rule for the Merchant encounter. No console input or output.
// One Merchant lives for one game, so "once per game" is this object's own state.
public class Merchant
{
    public const int ArmorPrice = 80;
    public const int ArmorProtection = 3;
    public const string ArmorName = "magic armor";

    public const int MagicWeaponPrice = 120;
    public const int MagicWeaponDamageBonus = 4;
    public const string MagicWeaponNamePrefix = "magic ";

    public bool EncounterUsed { get; private set; }

    public bool CanAffordArmor(Player player) => player.Gold >= ArmorPrice;

    public bool CanAffordMagicWeapon(Player player) => player.Gold >= MagicWeaponPrice;

    public bool CanAffordAnything(Player player) => CanAffordArmor(player) || CanAffordMagicWeapon(player);

    public PurchaseResult BuyArmor(Player player)
    {
        if (EncounterUsed)
            return PurchaseResult.EncounterUsed;

        if (!CanAffordArmor(player))
            return PurchaseResult.CantAfford;

        player.Gold -= ArmorPrice;
        player.Armor = new Armor(ArmorName, ArmorProtection);
        EncounterUsed = true;
        return PurchaseResult.Success;
    }

    // Enchants the player's own occupation weapon: more damage and a "magic" name, same art.
    public PurchaseResult BuyMagicWeapon(Player player)
    {
        if (EncounterUsed)
            return PurchaseResult.EncounterUsed;

        if (!CanAffordMagicWeapon(player))
            return PurchaseResult.CantAfford;

        player.Gold -= MagicWeaponPrice;
        player.Weapon!.MaxDamage += MagicWeaponDamageBonus;
        player.Weapon.Type = MagicWeaponNamePrefix + player.Weapon.Type;
        EncounterUsed = true;
        return PurchaseResult.Success;
    }

    // Ends the encounter without a purchase: the player left or could afford nothing.
    public void Leave()
    {
        EncounterUsed = true;
    }
}
