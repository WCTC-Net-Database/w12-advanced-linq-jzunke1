using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;

    private IPlayer? _player;
    private IMonster? _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Attack");
            _outputManager.WriteLine("2. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AttackCharacter();
                    break;
                case "2":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1 or 2.", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void AttackCharacter()
    {
        if (_goblin == null)
        {
            _outputManager.WriteLine("No goblin to attack.", ConsoleColor.Red);
            return;
        }

        if (_goblin is ITargetable targetableGoblin)
        {
            // Use a local variable to ensure the compiler knows _player is not null
            var player = _player;
            if (player == null)
            {
                _outputManager.WriteLine("Player is not initialized.", ConsoleColor.Red);
                return;
            }

            // Player attacks the goblin
            player.Attack(targetableGoblin);

            // Check if the player has abilities before using one
            var abilities = player.Abilities;
            if (abilities != null && abilities.Any())
            {
                var firstAbility = abilities.First();
                player.UseAbility(firstAbility, targetableGoblin);
            }
            else
            {
                _outputManager.WriteLine("Player has no abilities to use.", ConsoleColor.Red);
            }
        }
    }

    private void SetupGame()
    {
        // Load the player from the database
        _player = _context.Players
            .Include(p => p.Inventory)
            .Include(p => p.Equipment)
            .ThenInclude(static e => e.Weapon)
            .Include(p => p.Equipment)
            .ThenInclude(static e => e.Armor)
            .FirstOrDefault();

        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database. Exiting game...", ConsoleColor.Red);
            Environment.Exit(1);
        }

        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

        // Load monsters
        LoadMonsters();

        // Start the game loop
        Task.Delay(500).Wait();
        GameLoop();
    }

    private void LoadMonsters()
    {
        // Load the first goblin from the database
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();

        if (_goblin == null)
        {
            _outputManager.WriteLine("No goblin found in the database. Exiting game...", ConsoleColor.Red);
            Environment.Exit(1);
        }
    }
}
