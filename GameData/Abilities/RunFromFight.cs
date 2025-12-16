using The_World.GameData.Effects;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Abilities;

public class RunFromFight : IAbilities
{
    public string Name => "Run Away";
    public string Description => "Attempt to flee from combat and return to exploring";
    public int ManaCost => 0;
    
    private readonly int _successChance;
    
    public RunFromFight(int successChance = 75)
    {
        _successChance = successChance switch
        {
            < 10 => 10,   // Always at least 10% chance
            > 95 => 95,   // Never guaranteed
            _ => successChance
        };
    }

    public bool CanUse(Context context, string target = "")
    {
       
        //assuming this is always FightContext-- deal with this in commands?
        return true;
    }

    public string Use(Context context, string target = "")
    {
        if (!CanUse(context, target))
        {
            return "You can't run away right now.";
        }

        // Use dice to determine success (Strategy Pattern - different outcomes)
        var dice = new Dice(Sides: 100); // d100 roll
        var roll = dice.Roll();
        
        if (roll <= _successChance)
        {
            // Success! Create a FleeEffect that transitions states
            var fleeEffect = new FleeEffect(context, true);
            return fleeEffect.Apply(context);
        }
        else
        {
            // Failed to escape
            var fleeEffect = new FleeEffect(context, false);
            return fleeEffect.Apply(context);
        }
    }
}
}