using Microsoft.EntityFrameworkCore;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpgEntities.Data;

public class GameContext : DbContext
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Monster> Monsters { get; set; } = null!;
    public DbSet<Ability> Abilities { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;

    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FireballAbility>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<FireballAbility>("FireballAbility");
        modelBuilder.Entity<ShoveAbility>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<ShoveAbility>("ShoveAbility");
        modelBuilder.Entity<Monster>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Goblin>("Goblin");

        modelBuilder.Entity<Item>().HasData(
        new Item { Id = 1, Name = "Iron Sword", Type = "Weapon", Attack = 15, Defense = 0, Weight = 3.5m, Value = 50 },
        new Item { Id = 2, Name = "Steel Axe", Type = "Weapon", Attack = 20, Defense = 0, Weight = 5.0m, Value = 75 },
        new Item { Id = 3, Name = "Magic Staff", Type = "Weapon", Attack = 25, Defense = 5, Weight = 4.0m, Value = 100 }
        );

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 4, Name = "Leather Armor", Type = "Armor", Attack = 0, Defense = 10, Weight = 8.0m, Value = 40 },
            new Item { Id = 5, Name = "Chainmail", Type = "Armor", Attack = 0, Defense = 20, Weight = 15.0m, Value = 100 },
            new Item { Id = 6, Name = "Plate Armor", Type = "Armor", Attack = 0, Defense = 30, Weight = 25.0m, Value = 150 }
        );

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 7, Name = "Health Potion", Type = "Consumable", Attack = 0, Defense = 0, Weight = 0.5m, Value = 10 },
            new Item { Id = 8, Name = "Invisibility Potion", Type = "Consumable", Attack = 0, Defense = 0, Weight = 0.5m, Value = 15 },
            new Item { Id = 9, Name = "Torch", Type = "Utility", Attack = 0, Defense = 0, Weight = 1.0m, Value = 5 }
        );
        ConfigureEquipmentRelationships(modelBuilder);

    }

    private void ConfigureEquipmentRelationships(ModelBuilder modelBuilder)
    {
        // Configuring the Equipment entity to handle relationships with Item entities (Weapon and Armor)
        // without causing multiple cascade paths in SQL Server.

        // Equipment has a nullable foreign key WeaponId, pointing to the Item entity.
        // Setting DeleteBehavior.Restrict ensures that deleting an Item (Weapon) 
        // will NOT cascade delete any Equipment rows that reference it.
        // This prevents conflicts that arise with multiple cascading paths.
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Weapon)  // Define the relationship to the Weapon item
            .WithMany()             // Equipment doesn't need to navigate back to Item
            .HasForeignKey(e => e.WeaponId)  // Specifies the foreign key column in Equipment
                                             //.OnDelete(DeleteBehavior.Restrict)  // Prevents cascading deletes, avoids multiple paths
            .IsRequired(false);

        // Similar configuration for ArmorId, also pointing to the Item entity.
        // Here we are using DeleteBehavior.Restrict for the Armor foreign key relationship as well.
        // The goal is to avoid cascade paths from both WeaponId and ArmorId foreign keys.
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Armor)  // Define the relationship to the Armor item
            .WithMany()            // No need for reverse navigation back to Equipment
            .HasForeignKey(e => e.ArmorId)  // Sets ArmorId as the foreign key in Equipment
                                            //.OnDelete(DeleteBehavior.Restrict)  // Prevents cascading deletes to avoid conflict
            .IsRequired(false);

        // Explanation of Why DeleteBehavior.Restrict:
        // Cascade paths occur when there are multiple relationships in one table pointing to another,
        // each with cascading delete behavior. SQL Server restricts such configurations to prevent 
        // accidental recursive deletions. Here, by setting DeleteBehavior.Restrict, deleting an Item
        // (Weapon or Armor) will simply nullify the WeaponId or ArmorId in Equipment rather than 
        // cascading a delete through multiple paths.

        modelBuilder.Entity<Player>()
        .HasOne(p => p.Equipment)
        .WithOne()
        .HasForeignKey<Equipment>(e => e.Id)
        .IsRequired(false);

        modelBuilder.Entity<Item>()
            .HasOne<Player>()
            .WithMany(p => p.Inventory)
            .HasForeignKey(p => p.PlayerId)
            .IsRequired(false);
    }
}
