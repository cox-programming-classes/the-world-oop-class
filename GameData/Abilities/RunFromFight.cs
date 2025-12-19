using The_World.GameData.Creatures;
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
    public bool CanUse(Context context, Creature? target = null)
    {
        return context is FightContext;
    }
    
    public string Use(Context context, Creature? target = null)
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
            var fleeEffect = new FleeEffect();
            return fleeEffect.Apply(context);
        }
        else
        {
            // Failed to escape - stay in combat
            return $"You tried to run but couldn't escape! (Rolled {roll}, needed {_successChance} or lower)";
        }
    }
}