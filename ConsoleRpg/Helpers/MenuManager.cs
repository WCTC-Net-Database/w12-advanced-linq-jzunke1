using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpg.Helpers;

public class MenuManager
{
    private readonly OutputManager _outputManager;

    public MenuManager(OutputManager outputManager)
    {
        _outputManager = outputManager;
    }

    public bool ShowMainMenu(Player player)
    {
        _outputManager.WriteLine("Welcome to the RPG Game!", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Start Game", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Manage Inventory", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Exit", ConsoleColor.Cyan);
        _outputManager.Display();

        return HandleMainMenuInput(player);
    }

    private bool HandleMainMenuInput(Player player)
    {
        while (true)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    _outputManager.WriteLine("Starting game...", ConsoleColor.Green); 
                    _outputManager.Display(); 
                    return true;
                case "2":
                    ShowInventoryMenu(player);
                    return false;
                case "3":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1, 2, or 3.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }

    public bool ShowInventoryMenu(Player player)
    {
        while (true)
        {
            _outputManager.WriteLine("Inventory Management", ConsoleColor.Yellow);
            _outputManager.WriteLine("1. Search Item by Name", ConsoleColor.Cyan);
            _outputManager.WriteLine("2. List Items by Type", ConsoleColor.Cyan);
            _outputManager.WriteLine("3. Sort Inventory", ConsoleColor.Cyan);
            _outputManager.WriteLine("4. Display Total Weight of Inventory", ConsoleColor.Cyan);
            _outputManager.WriteLine("5. Back to Main Menu", ConsoleColor.Cyan);
            _outputManager.Display();

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    _outputManager.Write("Enter item name to search: ", ConsoleColor.White);
                    var itemName = Console.ReadLine();
                    player.SearchItemByName(itemName);
                    break;
                case "2":
                    player.ListItemsByType();
                    break;
                case "3":
                    ShowItemsMenu(player);
                    break;
                case "4":
                    _outputManager.WriteLine($"Total Weight: {player.GetTotalWeight()} / {player.MaxWeight}", ConsoleColor.Green);
                    break;
                case "5":
                    ShowMainMenu(player);
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose a valid option.", ConsoleColor.Red);
                    break;
            }
        }
    }


    public bool ShowItemsMenu(Player player)
    {
        while (true)
        {
            _outputManager.WriteLine("Sort Items", ConsoleColor.Yellow);
            _outputManager.WriteLine("1. Sort Items by Name", ConsoleColor.Cyan);
            _outputManager.WriteLine("2. Sort Items by Attack Value", ConsoleColor.Cyan);
            _outputManager.WriteLine("3. Sort Items by Defence Value", ConsoleColor.Cyan);
            _outputManager.WriteLine("4. Back to Main Menu", ConsoleColor.Cyan);
            _outputManager.Display();

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player.SortInventoryBy("Name");
                    break;
                case "2":
                    player.SortInventoryBy("Attack");
                    break;
                case "3":
                    player.SortInventoryBy("Defense");
                    break;
                case "4":
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose a valid option.", ConsoleColor.Red);
                    break;
            }
        }
    }
}
