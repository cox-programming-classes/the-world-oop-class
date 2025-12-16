using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;

public class FleeEffect : IEffect
{
    public string Apply(Context context, bool canFlee)
    {
        if (context is FightContext fightContext and canFlee == true)
        {
            
        }
        
    }

    public string GetDescription() => $"Got yourself in a Situation? Solve all your issues with this one simple trick!";
}