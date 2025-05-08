using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
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
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>(m => m.MonsterType)
                .HasValue<Goblin>("Goblin");

            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>(pa => pa.AbilityType)
                .HasValue<ShoveAbility>("ShoveAbility");

            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            ConfigureEquipmentRelationships(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureEquipmentRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Weapon)
                .WithMany()
                .HasForeignKey(e => e.WeaponId)
                .IsRequired(false);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Armor)
                .WithMany()
                .HasForeignKey(e => e.ArmorId)
                .IsRequired(false);
        }

        public void SeedData()
        {
            if (Players.Any() || Monsters.Any() || Items.Any() || Abilities.Any() || Equipments.Any())
            {
                return;
            }

            var sword = new Item { Name = "Sword", Type = "Weapon", Attack = 10, Defense = 0, Weight = 2.5m, Value = 100 };
            var axe = new Item { Name = "Axe", Type = "Weapon", Attack = 15, Defense = 0, Weight = 3.0m, Value = 120 };

            var ironArmor = new Item { Name = "Iron Armor", Type = "Armor", Attack = 0, Defense = 15, Weight = 5.0m, Value = 150 };
            var leatherArmor = new Item { Name = "Leather Armor", Type = "Armor", Attack = 0, Defense = 10, Weight = 3.5m, Value = 80 };

            var healthPotion = new Item { Name = "Health Potion", Type = "Consumable", Attack = 0, Defense = 0, Weight = 0.5m, Value = 50 };

            Items.AddRange(sword, axe, ironArmor, leatherArmor, healthPotion);

            var equipment1 = new Equipment { Weapon = sword, Armor = ironArmor };
            var equipment2 = new Equipment { Weapon = axe, Armor = leatherArmor };

            Equipments.AddRange(equipment1, equipment2);

            var shoveAbility = new ShoveAbility { Name = "Shove", Description = "Pushes the enemy back.", AbilityType = "ShoveAbility" };
            Abilities.Add(shoveAbility);

            var player = new Player
            {
                Name = "Sir Josh",
                Experience = 0,
                Health = 100,
                Inventory = new List<Item> { sword, ironArmor, healthPotion },
                Equipment = equipment1,
                Abilities = new List<Ability> { shoveAbility }
            };
            Players.Add(player);

            var goblin = new Goblin
            {
                Name = "Goblin",
                Health = 50,
                AggressionLevel = 3,
                MonsterType = "Goblin"
            };
            Monsters.Add(goblin);

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}

