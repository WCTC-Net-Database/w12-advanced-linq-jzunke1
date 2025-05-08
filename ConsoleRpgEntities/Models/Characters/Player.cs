using Console.RpgEntities.Models;
using ConsoleRpgEntities.Models.Equipments;

public class Player : ITargetable, IPlayer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Experience { get; set; }
    public int Health { get; set; }
    public virtual List<Item> Inventory { get; set; } = new();
    public int MaxWeight { get; set; } = 50;

    public void AddItemToInventory(Item item)
    {
        var totalWeight = Inventory.Sum(i => i.Weight);
        if (totalWeight + item.Weight > MaxWeight)
        {
            Console.WriteLine("Cannot add item. Exceeds weight limit.");
            return;
        }

        Inventory.Add(item);
        Console.WriteLine($"{item.Name} has been added to your inventory.");
    }

    public void UseItemFromInventory(string itemName)
    {
        var item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            Console.WriteLine($"Using {item.Name}...");
            Inventory.Remove(item);
        }
        else
        {
            Console.WriteLine($"Item '{itemName}' not found in inventory.");
        }
    }

    public void EquipItemFromInventory(string itemName)
    {
        var item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            Console.WriteLine($"Item '{itemName}' not found in inventory.");
            return;
        }

        if (item.Type == "Weapon" || item.Type == "Armor")
        {
            Console.WriteLine($"Equipping {item.Name}...");
        }
        else
        {
            Console.WriteLine($"{item.Name} cannot be equipped.");
        }
    }

    public void RemoveItemFromInventory(string itemName)
    {
        var item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            Inventory.Remove(item);
            Console.WriteLine($"{item.Name} has been removed from your inventory.");
        }
        else
        {
            Console.WriteLine($"Item '{itemName}' not found in inventory.");
        }
    }

    public void SearchItemByName(string itemName)
    {
        var matchingItems = Inventory.Where(i => i.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase)).ToList();
        if (matchingItems.Any())
        {
            Console.WriteLine("Matching items:");
            foreach (var item in matchingItems)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
        else
        {
            Console.WriteLine("No matching items found.");
        }
    }

    public void ListItemsByType(string itemType)
    {
        var itemsByType = Inventory.Where(i => i.Type.Equals(itemType, StringComparison.OrdinalIgnoreCase)).ToList();
        if (itemsByType.Any())
        {
            Console.WriteLine($"Items of type '{itemType}':");
            foreach (var item in itemsByType)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
        else
        {
            Console.WriteLine($"No items of type '{itemType}' found.");
        }
    }

    public void SortItems(string sortBy)
    {
        IEnumerable<Item> sortedItems = sortBy.ToLower() switch
        {
            "name" => Inventory.OrderBy(i => i.Name),
            "attack" => Inventory.OrderByDescending(i => i.Attack),
            "defense" => Inventory.OrderByDescending(i => i.Defense),
            _ => Inventory
        };

        Console.WriteLine("Sorted items:");
        foreach (var item in sortedItems)
        {
            Console.WriteLine($"- {item.Name} (Attack: {item.Attack}, Defense: {item.Defense})");
        }
    }
}
