using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;
/// <summary>
/// 
/// </summary>
public interface IEffect
{
    string Apply(GameContext context);
    string GetDescription();
}
