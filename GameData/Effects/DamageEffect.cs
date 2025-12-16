/* GameData/Effects/DamageEffect.cs */
using The_World.GameData.GameMechanics;
using The_World.GameData.Creatures;

namespace The_World.GameData.Effects;

public class DamageEffect : IEffect, ITargetableEffect
{
    private readonly int _baseDamage;

    public DamageEffect(int baseDamage)
    {
        _baseDamage = baseDamage switch
        {
            < 1 => 1,
            _ => baseDamage
        };
    }

    // For IEffect - when applied to player (maybe for self-damage?)
    public string Apply(Context context)
    {
        return "You can't damage yourself with this!";
    }

    // For ITargetableEffect - when applied to creatures
    public string ApplyToCreature(Creature target, Context context)
    {
        // Calculate damage using your complex formula
        var randomNumber = Dice.D6.Roll();
        var playerLevel = context.Player.Level;
        var creatureLevel = target.Level;
        
        // Your damage calculation
        var levelDifference = creatureLevel - playerLevel;
        var damageMultiplier = Math.Pow(2, (randomNumber * 2 - levelDifference));
        var finalDamage = (int)Math.Max(1, _baseDamage * damageMultiplier);

        // Apply damage to the creature
        target.Stats.Health -= finalDamage;
        
        return $"You deal {finalDamage} damage to the {target.Name}!";
    }

    public string GetDescription() => $"Deals {_baseDamage} base damage to target";
}