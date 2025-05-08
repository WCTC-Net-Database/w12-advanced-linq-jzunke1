using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpg.Helpers;

public class MenuManager
{
    private readonly OutputManager _outputManager;
    private readonly GameContext _context;
    private Player? _player;

    public MenuManager(OutputManager outputManager, GameContext context)
    {
        _outputManager = outputManager ?? throw new ArgumentNullException(nameof(outputManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public bool ShowMainMenu()
    {
        _player = _context.Players.FirstOrDefault();
        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database. Exiting game...", ConsoleColor.Red);
            Environment.Exit(1);
        }

        _outputManager.WriteLine("Welcome to the RPG Game!", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Start Game", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Manage Inventory", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Exit", ConsoleColor.Cyan);
        _outputManager.Display();

        return HandleMainMenuInput();
    }

    private bool HandleMainMenuInput()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                _outputManager.WriteLine("Invalid input. Please try again.", ConsoleColor.Red);
                continue;
            }

            switch (input)
            {
                case "1":
                    _outputManager.WriteLine("Starting game...", ConsoleColor.Green);
                    _outputManager.Display();
                    return true;
                case "2":
                    ShowInventoryMenu();
                    break;
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

    private void ShowInventoryMenu()
    {
        if (_player == null) return;

        _outputManager.WriteLine("\nInventory Management:", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Search for item by name", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. List items by type", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Sort items", ConsoleColor.Cyan);
        _outputManager.WriteLine("4. Back to Main Menu", ConsoleColor.Cyan);
        _outputManager.Display();

        HandleInventoryMenuInput();
    }

    private void HandleInventoryMenuInput()
    {
        if (_player == null) return;

        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                _outputManager.WriteLine("Invalid input. Please try again.", ConsoleColor.Red);
                continue;
            }

            switch (input)
            {
                case "1":
                    _outputManager.Write("Enter item name to search: ", ConsoleColor.Cyan);
                    var itemName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(itemName))
                    {
                        _player.SearchItemByName(itemName);
                    }
                    else
                    {
                        _outputManager.WriteLine("Invalid item name.", ConsoleColor.Red);
                    }
                    _outputManager.Display();
                    break;
                case "2":
                    _outputManager.Write("Enter item type to list (e.g., Weapon, Armor): ", ConsoleColor.Cyan);
                    var itemType = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(itemType))
                    {
                        _player.ListItemsByType(itemType);
                    }
                    else
                    {
                        _outputManager.WriteLine("Invalid item type.", ConsoleColor.Red);
                    }
                    _outputManager.Display();
                    break;
                case "3":
                    ShowSortMenu();
                    break;
                case "4":
                    return;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1, 2, 3, or 4.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }

    private void ShowSortMenu()
    {
        if (_player == null) return;

        _outputManager.WriteLine("\nSort Options:", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Sort by Name", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Sort by Attack Value", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Sort by Defense Value", ConsoleColor.Cyan);
        _outputManager.WriteLine("4. Back to Inventory Menu", ConsoleColor.Cyan);
        _outputManager.Display();

        HandleSortMenuInput();
    }

    private void HandleSortMenuInput()
    {
        if (_player == null) return;

        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                _outputManager.WriteLine("Invalid input. Please try again.", ConsoleColor.Red);
                continue;
            }

            switch (input)
            {
                case "1":
                    _player.SortItems("name");
                    _outputManager.Display();
                    break;
                case "2":
                    _player.SortItems("attack");
                    _outputManager.Display();
                    break;
                case "3":
                    _player.SortItems("defense");
                    _outputManager.Display();
                    break;
                case "4":
                    return;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1, 2, 3, or 4.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }
}
