

using The_World.GameData.Commands;
namespace The_World.GameData.Effects;

public class HealingEffect(int healAmount = 20) : ConsumableEffect
{
    public string Apply(GameContext context)
    {
        var oldHealth = context.Player.Stats.Health;
        context.Player.Stats.RestoreHealth(healAmount);
        var newHealth = context.Player.Stats.Health;
        var actualHealing = newHealth - oldHealth;
        
        return actualHealing > 0 
            ? $"You feel healthier! Restored {actualHealing} health points."
            : "You're already at full health!";
    }
}
