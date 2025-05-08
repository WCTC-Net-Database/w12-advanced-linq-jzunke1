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

    private Player? _player;
    private IMonster? _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
        _player = context.Players
            .Include(p => p.Inventory)
            .Include(p => p.Abilities)
            .Include(p => p.Equipment)
            .ThenInclude(e => e.Weapon)
            .FirstOrDefault(p => p.Id == 1);
    }

    public void Run()
    {
        _player = _context.Players.FirstOrDefault();
        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database.", ConsoleColor.Red);
            return;
        }

        if (_menuManager.ShowMainMenu(_player))
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
            _outputManager.WriteLine("2. Manage Inventory");
            _outputManager.WriteLine("3. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AttackCharacter();
                    break;
                case "2":
                    if (_player != null)
                    {
                        _menuManager.ShowInventoryMenu(_player);
                    }
                    else
                    {
                        _outputManager.WriteLine("Player not initialized.", ConsoleColor.Red);
                    }
                    break;
                case "3":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose a valid option.", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void AttackCharacter()
    {
        if (_player == null || _goblin == null)
        {
            _outputManager.WriteLine("Player or Goblin not initialized.", ConsoleColor.Red);
            return;
        }

        if (_goblin is ITargetable targetableGoblin)
        {
            _player.Attack(targetableGoblin);
            _player.UseAbility(_player.Abilities.First(), targetableGoblin);
        }
    }

    private void SetupGame()
    {
        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database.", ConsoleColor.Red);
            return;
        }

        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

        LoadMonsters();

        Thread.Sleep(500);
        GameLoop();
    }

    private void LoadMonsters()
    {
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
        if (_goblin == null)
        {
            _outputManager.WriteLine("No goblins found in the database.", ConsoleColor.Red);
        }
    }
}
