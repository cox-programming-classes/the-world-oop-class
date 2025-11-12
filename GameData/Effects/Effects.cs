using The_World.GameData.Commands;

namespace The_World.GameData.Effects;

public interface ConsumableEffect
{
    string Apply(GameContext context);
    
    // action: Action<GameContext> -- represents a lambda expression
    // name
    // description
}