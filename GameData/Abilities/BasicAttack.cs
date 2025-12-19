/* GameData/Abilities/BasicAttackAbility.cs */

using The_World.GameData.Creatures;
using The_World.GameData.GameMechanics;
using The_World.GameData.Effects;

namespace The_World.GameData.Abilities;

public class BasicAttack : IAbilities
{
    public string Name => "Basic Attack";
    public string Description => "A simple physical attack that deals moderate damage";
    public int ManaCost => 0; // Basic attacks don't cost mana
    
    private readonly int _baseDamage;
    private readonly Creature _target;

    public BasicAttack(int baseDamage = 5)
    {
        _baseDamage = baseDamage switch
        {
            < 1 => 1,
            > 50 => 50,
            _ => baseDamage
        };
    }

    public bool CanUse(Context context, Creature target)
    {
        // Can always use basic attack if target exists
        
        if (context is FightContext fightContext)
        {
            return fightContext.Creatures.Contains(target);
        }
        return false;
    }

    public string Use(Context context, Creature target)
    {
        if (!CanUse(context,  target))
        {
            return $"There is no '{target}' here to attack.";
        }

        // Use the Strategy Pattern - create the appropriate effect
        var damageEffect = new DamageEffect(_baseDamage, target);
        
        // Apply the effect and return the result
        return damageEffect.Apply(context);
    }
}