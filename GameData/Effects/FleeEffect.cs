using The_World.GameData.GameMechanics;

namespace The_World.GameData.Effects;


public class FleeEffect : IEffect
{
    private readonly int _successChance;
    
    public FleeEffect(int successChance = 75)
    {
        
        _successChance = successChance;
    }

    public string Apply(Context context)
    {
        if (context is not FightContext fightContext)
        {
            return "You can only flee from combat!";
        }

        // Use dice to determine success
        var dice = new Dice(Sides: 100);
        var roll = dice.Roll();

        if (roll <= _successChance)
        {
            // Success - this will be handled by the command system
            return $"Success! (Rolled {roll}, needed {_successChance} or less)";
        }

        return $"You couldn't escape! The enemies block your path. (Rolled {roll}, needed {_successChance} or less)";

    }

    public string GetDescription() => $"{_successChance}% chance to escape from combat";
}
