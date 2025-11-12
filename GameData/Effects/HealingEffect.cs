using The_World.GameData.Commands;
namespace The_World.GameData.Effects;

public class HealingEffect : ConsumableEffect
{
    public string Apply(GameContext context)
    {
        context.Player.Stats.Health += 10;
        return "You feel healthier!";
    }
}