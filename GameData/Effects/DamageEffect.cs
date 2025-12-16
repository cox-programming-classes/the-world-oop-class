/* GameData/Effects/DamageEffect.cs */
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;

public class DamageEffect : IEffect
{
    private readonly int _damage;
    private readonly string _targetName;

    public DamageEffect(int damage, string targetName = "")
    {
        _damage = damage switch
        {
            < 1 => 1,
            //no maximum right now
            _ => damage
        };
        _targetName = targetName?.Trim() ?? "";
    }

    public string Apply(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_targetName))
        {
            return "No target specified for attack!";
        }

        if (context.CurrentArea.Creatures.TryGetValue(_targetName, out var creature))
        {
            // TODO: Implement actual damage to creature stats when combat system is ready
            return $"You deal {_damage} damage to the {creature.Name}!";
        }
        
        return $"There is no '{_targetName}' here to attack.";
    }

    public string GetDescription() => $"Deals {_damage} damage to target";
}