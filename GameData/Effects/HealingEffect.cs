using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;

public class HealingEffect(int healAmount) : IEffect
{
    private readonly int _healAmount = healAmount switch
    {
        < 1 => throw new ArgumentException("Restored heal amount must be at least 1."),
        > 200 => throw new ArgumentException("Restored heal amount cannot exceed 200."),
        _ => healAmount
    };

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
    public string GetDescription() => $"Restores up to {_healAmount} health points";
}
