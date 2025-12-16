using The_World.GameData.Effects;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Abilities;

public class BasicHeal : IAbilities
{
    public string Name => "Basic Heal";
    public string Description => "A simple healing spell, costs 10 mana to apply";
    public int ManaCost => 10; // Basic attacks don't cost mana

    private readonly int _baseHealing;

    public BasicHeal(int baseHealing = 5)
    {
        _baseHealing = baseHealing switch
        {
            < 1 => 1,
            > 50 => 50,
            _ => baseHealing
        };
    }

    public bool CanUse(GameContext context, string target = "")
    {
        // Can always use basic attack if target exists
        if (string.IsNullOrWhiteSpace(target))
            return false;
        else if (context.Player.Stats.Mana < 10)
        {
            Console.WriteLine("You don't have enough mana to do this!");
            return false;
        }

        return context.CurrentArea.Creatures.ContainsKey(target);
    }

    public string Use(GameContext context, string target = "")
    {
        if (!CanUse(context, target))
        {
            return string.IsNullOrWhiteSpace(target)
                ? "Attack what? Specify a target."
                : $"There is no '{target}' here to attack.";
        }

        // Use the Strategy Pattern - create the appropriate effect
        var damageEffect = new HealingEffect(_baseHealing);

        // Apply the effect and return the result
        return damageEffect.Apply(context);
    }
}