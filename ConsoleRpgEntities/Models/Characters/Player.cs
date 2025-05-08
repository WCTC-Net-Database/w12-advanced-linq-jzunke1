using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;

public class Player : ITargetable, IPlayer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Experience { get; set; }
    public int Health { get; set; }

    public int? EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = new Equipment();
    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();
    public virtual List<Item> Inventory { get; set; } = new List<Item>();
    public int MaxWeight { get; set; } = 50;

    public void Attack(ITargetable target)
    {
        int damage = Equipment.Weapon?.Attack ?? 1;
        Console.WriteLine($"{Name} attacks {target.Name} with a {Equipment.Weapon?.Name ?? "fist"} dealing {damage} damage!");

        target.Health -= damage;

        if (target.Health <= 0)
        {
            target.Health = 0;
            Console.WriteLine($"{target.Name} has been defeated!");
        }
        else
        {
            Console.WriteLine($"{target.Name} has {target.Health} health remaining.");
        }
    }



    public void UseAbility(IAbility ability, ITargetable target)
    {
        if (Abilities.Contains(ability))
        {
            ability.Activate(this, target);
        }
        else
        {
            Console.WriteLine($"{Name} does not have the ability {ability.Name}!");
        }
    }

    public void SearchItemByName(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Console.WriteLine("Item name cannot be null or empty.");
            return;
        }

        var matchingItems = Inventory
            .Where(i => i.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingItems.Any())
        {
            Console.WriteLine($"Items matching '{itemName}':");
            foreach (var item in matchingItems)
            {
                Console.WriteLine($"- {item.Name} (Type: {item.Type}, Attack: {item.Attack}, Defense: {item.Defense})");
            }
        }
        else
        {
            Console.WriteLine($"No items found matching '{itemName}'.");
        }
    }

    public void ListItemsByType()
    {
        if (Inventory == null || !Inventory.Any())
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        var groupedItems = Inventory.GroupBy(i => i.Type);
        Console.WriteLine("Items grouped by type:");
        foreach (var group in groupedItems)
        {
            Console.WriteLine($"Type: {group.Key}");
            foreach (var item in group)
            {
                Console.WriteLine($"- {item.Name} (Attack: {item.Attack}, Defense: {item.Defense})");
            }
        }
    }

    public void SortInventoryBy(string criterion)
    {
        if (Inventory == null || !Inventory.Any())
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        IEnumerable<Item> sortedItems = criterion switch
        {
            "Name" => Inventory.OrderBy(i => i.Name),
            "Attack" => Inventory.OrderByDescending(i => i.Attack),
            "Defense" => Inventory.OrderByDescending(i => i.Defense),
            _ => Inventory
        };

        Console.WriteLine("Sorted Inventory:");
        foreach (var item in sortedItems)
        {
            Console.WriteLine($"- {item.Name} (Type: {item.Type}, Attack: {item.Attack}, Defense: {item.Defense})");
        }
    }


    public decimal GetTotalWeight()
    {
        return Inventory.Sum(item => item.Weight);
    }

    public IEnumerable<Item> GetEquipableItems()
    {
        decimal remainingWeight = MaxWeight - GetTotalWeight();
        return Inventory.Where(item => item.Weight <= remainingWeight);
    }
}
