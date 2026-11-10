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

    public bool EncounterUsed { get; private set; }

    public bool CanAffordArmor(Player player) => player.Gold >= ArmorPrice;

    public bool CanAffordAnything(Player player) => CanAffordArmor(player);

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

    // Ends the encounter without a purchase: the player left or could afford nothing.
    public void Leave()
    {
        EncounterUsed = true;
    }
}
