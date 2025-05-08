using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities.PlayerAbilities
{
    public class FireballAbility : Ability
    {
        public FireballAbility()
        {
            Name = "Fireball";
            Description = "Casts a fireball that deals damage to the target.";
            AbilityType = "Magic";
        }

        public override void Activate(IPlayer user, ITargetable target)
        {
            Console.WriteLine($"{user.Name} casts {Name} on {target.Name}!");
            target.Health -= 50; // Example damage
        }
    }
}
