using The_World.GameData.Commands;

namespace The_World.GameData.Effects;

public class NoEffect : IEffect
{
    public string Apply(GameContext context)
    {
        return "Nothing happens. This item seems to have no effect.";
    }
    public string GetDescription() => "Does nothing at all. Just for the joy of it really";
}
