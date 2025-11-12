using System.Runtime.InteropServices.ComTypes;
using The_World.GameData.Commands;
namespace The_World.GameData.Effects;

public class HealingEffect : ConsumableEffect
{
    public string Apply(GameContext context)
    {
        Player.Stats.Health += 50;
        return "You feel healthier!";
    }
}