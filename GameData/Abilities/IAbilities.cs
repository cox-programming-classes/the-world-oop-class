using The_World.GameData.GameMechanics;

namespace The_World.GameData.Abilities;

public interface IAbilities
{
    // GameData/Abilities/IAbility.cs
    public interface IAbility
    {
        string Name { get; }
        string Description { get; }
        int ManaCost { get; }
        string Use(GameContext context /* target parameters */);
        bool CanUse(GameContext context);
    }

}