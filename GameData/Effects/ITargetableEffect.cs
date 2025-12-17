/* GameData/Effects/ITargetableEffect.cs */
using The_World.GameData.Creatures;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;

public interface ITargetableEffect
{
    string ApplyToCreature(Creature target, Context context);
    string GetDescription();
}