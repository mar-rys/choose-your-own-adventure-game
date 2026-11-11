public class Game
{
    public void StartGame()
    {
        // Title screen
        Console.WriteLine("=====================================");
        Console.WriteLine("Welcome to Choose Your Adventure Game");
        Console.WriteLine("=====================================\n");

        // Instantiate Die first (per sequence diagram)
        Die die = new Die();

        Dragon dragon = new Dragon(die);
        Merchant merchant = new Merchant();

        // Language selection shown before Messages dictionary is loaded
        Console.WriteLine("Select language / Selecciona idioma / Choisissez la langue / Seleziona la lingua:");
        Console.WriteLine("1. English  2. Espanol  3. Francais  4. Italiano");
        Console.Write("Enter choice: ");
        string? langInput = Console.ReadLine()?.Trim();
        string selectedLanguage = langInput switch { "2" => "Spanish", "3" => "French", "4" => "Italian", _ => "English" };

        // Initialize Messages with selected language (per sequence diagram)

        Messages messages = new Messages();
        messages.SetCurrentLanguage(selectedLanguage);
        messages.ReadDictionary();
        // Display welcome message
        Console.WriteLine();
        Console.WriteLine(messages.GetMessage("welcome"));
        Console.WriteLine();

        // Main menu loop
        bool running = true;
        while (running)
        {
            Console.WriteLine(messages.GetMessage("menu"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                Player player = new Player();
                player.CreateCharacter(messages);

                bool inAdventureMenu = true;
                while (inAdventureMenu)
                {
                    string selectedPath = PromptForPath(messages);

                    if (selectedPath == "exit")
                    {
                        Console.WriteLine(messages.GetMessage("adventure_ends"));
                        running = false;
                        inAdventureMenu = false;
                    }
                    else if (selectedPath == "south")
                    {
                        HandleMerchantEncounter(player, merchant, messages);
                    }
                    else if (selectedPath == "north")
                    {
                        HandleDragonEncounter(player, dragon, messages);

                        running = false;
                        inAdventureMenu = false;
                    }
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine(messages.GetMessage("goodbye"));
                running = false;
            }
            else
            {
                Console.WriteLine(messages.GetMessage("invalid"));
            }
        }
    }

    public void EndGame(bool playerWon, bool playerRetreated, Messages messages)
    {
        if (playerRetreated)
            Console.WriteLine(messages.GetMessage("retreat"));
        else if (playerWon)
            Console.WriteLine(messages.GetMessage("victory"));
        else
            Console.WriteLine(messages.GetMessage("defeat"));
    }

    public bool IsValidMenuChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim();
        return trimmed == "1" || trimmed == "2";
    }

    public bool IsValidPathChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim().ToLower();
        return trimmed == "n" || trimmed == "north" ||
               trimmed == "s" || trimmed == "south" ||
               trimmed == "e" || trimmed == "exit";
    }

    public bool IsValidCombatChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim().ToLower();
        return trimmed == "a" || trimmed == "attack" ||
               trimmed == "r" || trimmed == "retreat";
    }

    private string PromptForPath(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("path_prompt_full"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "north" || input == "n")
                return "north";

            if (input == "south" || input == "s")
                return "south";

            if (input == "exit" || input == "e")
                return "exit";

            Console.WriteLine(messages.GetMessage("path_invalid"));
        }
    }

    // Thin console layer over the Merchant: reads choices and turns its results into messages.
    private void HandleMerchantEncounter(Player player, Merchant merchant, Messages messages)
    {
        if (merchant.EncounterUsed)
        {
            Console.WriteLine(messages.GetMessage("merchant_moved_on"));
            return;
        }

        Console.WriteLine(messages.GetMessage("merchant_intro"));

        if (!merchant.CanAffordAnything(player))
        {
            Console.WriteLine(messages.GetMessage("merchant_cant_afford_anything"));
            merchant.Leave();
        }
        else
        {
            RunMerchantMenu(player, merchant, messages);
        }

        Console.WriteLine(messages.GetMessage("merchant_return"));
    }

    private void RunMerchantMenu(Player player, Merchant merchant, Messages messages)
    {
        while (true)
        {
            Console.WriteLine(string.Format(messages.GetMessage("merchant_offer"), player.Gold, Merchant.ArmorPrice, Merchant.MagicWeaponPrice));
            Console.WriteLine(messages.GetMessage("merchant_menu"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim();

            if (input == "1")
            {
                if (ShowPurchaseResult(merchant.BuyArmor(player), messages, () => string.Format(
                        messages.GetMessage("merchant_armor_bought"), Merchant.ArmorPrice, player.Protection)))
                    return;
            }
            else if (input == "2")
            {
                if (ShowPurchaseResult(merchant.BuyMagicWeapon(player), messages, () => string.Format(
                        messages.GetMessage("merchant_weapon_bought"),
                        Merchant.MagicWeaponPrice,
                        messages.TranslateWeaponForDisplay(player.Weapon!.Type),
                        player.Weapon.MaxDamage)))
                    return;
            }
            else if (input == "3")
            {
                merchant.Leave();
                return;
            }
            else
            {
                Console.WriteLine(messages.GetMessage("merchant_invalid"));
            }
        }
    }

    // Prints the outcome of a purchase. The confirmation is built only after a successful
    // purchase, so it reads the player's new state. Returns true when the encounter is over.
    private static bool ShowPurchaseResult(PurchaseResult result, Messages messages, Func<string> boughtMessage)
    {
        switch (result)
        {
            case PurchaseResult.Success:
                Console.WriteLine(boughtMessage());
                return true;
            case PurchaseResult.CantAfford:
                Console.WriteLine(messages.GetMessage("merchant_cant_afford"));
                return false;
            case PurchaseResult.EncounterUsed:
                Console.WriteLine(messages.GetMessage("merchant_moved_on"));
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }

    private void HandleDragonEncounter(Player player, Dragon dragon, Messages messages)
    {
        Console.WriteLine(messages.GetMessage("north_path_narrative"));

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(string.Format(messages.GetMessage("dragon_intro"), dragon.Name));
        Console.ResetColor();

        Console.WriteLine(messages.GetMessage("dragon_stats_intro"));
        dragon.DisplayStats(messages);
        while (true)
        {
            Console.WriteLine(messages.GetMessage("dragon_encounter_prompt"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "a" || input == "attack")
            {
                Combat combat = new Combat(player, dragon, messages);
                bool playerWon = combat.StartCombat();

                if (combat.PlayerRetreated)
                {
                    return; //combat already handled retreat message
                }
                else
                {
                   EndGame(playerWon, false, messages); 
                }
                
                return;
            }

        if (input == "r" || input == "retreat")
        {
            Console.WriteLine(messages.GetMessage("retreat"));
            return;
        }

        Console.WriteLine(messages.GetMessage("invalid"));
    }
}
}
