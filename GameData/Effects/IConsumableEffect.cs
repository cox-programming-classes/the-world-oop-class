using The_World.GameData.Commands;

namespace The_World.GameData.Effects;

public interface IConsumableEffect
{
    string Apply(GameContext context);
    string GetDescription();
}
