using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities.PlayerAbilities
{
    public class ShoveAbility : Ability
    {
        public override void Activate(IPlayer user, ITargetable target)
        {
            Console.WriteLine($"{user.Name} shoves {target.Name}!");
        }
    }
}
