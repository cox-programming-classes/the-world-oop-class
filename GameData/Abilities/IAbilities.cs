using The_World.GameData.Creatures;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Abilities;

public interface IAbilities
{
    string Name { get; }
    string Description { get; }
    int ManaCost { get; }
    bool CanUse(Context context, Creature? target = null);
    string Use(Context context, Creature? target = null);
}
