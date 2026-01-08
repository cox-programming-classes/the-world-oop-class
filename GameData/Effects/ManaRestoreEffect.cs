using The_World.GameData.Commands;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;

public class ManaRestoreEffect(int manaAmount) : IEffect
{
    private readonly int _manaAmount = manaAmount switch
    {
        < 1 => throw new ArgumentException("Restored mana amount must be at least 1."),
        > 150 => throw new ArgumentException("Restored mana amount cannot exceed 150."),
        _ => manaAmount
    };
    public string Apply(Context context)
    {
        var oldMana = context.Player.Stats.Mana;
        context.Player.Stats.RestoreMana(manaAmount);
        var newMana = context.Player.Stats.Mana;
        var actualMana = newMana - oldMana;

        return actualMana > 0
            ? $"You feel better! Restored {actualMana} mana points."
            : "You're already at full mana!";
    }
    
    public string GetDescription() => $"Restores {_manaAmount} health points";
}