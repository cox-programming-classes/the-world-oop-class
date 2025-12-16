using System;
using The_World.GameData.GameMechanics;
using System.Linq;
using The_World.GameData.Creatures;

namespace The_World.GameData.Commands;

public class AttackCommand : ICommand
{
    private readonly string _creatureName;

    public AttackCommand(string creatureName = "")
    {
        _creatureName = creatureName?.Trim() ?? "";
    }

    public Context Execute(Context c) => c switch
    {
        GameContext gameContext => ExecuteOnGameContext(gameContext),
        FightContext fightContext => ExecuteOnFightContext(fightContext),
        _ => c
    };

    private Context ExecuteOnFightContext(FightContext context)
    {
        // TODO:  Write attack for the Fight Context~
        
        var targetCreature = SelectTarget(context);
        if (targetCreature == null)
        {
            Console.WriteLine("You missed!");
            return context; 
        }
        
        // calculating the damage dealt-influenced by player level and creature level (int) 
        var randomNumber = Dice.D6.Roll();
        var playerLevel = context.Player.Level;
        var creatureLevel = targetCreature.Level;
        var damage = Math.Pow(2,(randomNumber * 2 - (creatureLevel - playerLevel))); // higher creature level means less dmg
                                                                                              // higher player level means more dmg  

          var pw = 
              (creatureLevel - playerLevel)
              switch
              {
                  > 20 => 20,  // creature is very much higher level
                  > 0 => 1, // creature level > player level
                  0 => 0, // same level
                  _ => -1 // player level is higher.
              };
                                                                                              
        return context;
    }
    
    private Creature? SelectTarget(FightContext context)
    {
        return context.Creatures.FirstOrDefault();
    }
    
    
    private Context ExecuteOnGameContext(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_creatureName))
        {
            Console.WriteLine("Attack what? Try 'attack [creature]'");
            return context;
        }
            
        if (context.CurrentArea.Creatures.TryGetValue(_creatureName, out var creature))
        {
            // TODO: Implement actual combat mechanics
            Console.WriteLine($"You attack the {creature.Name}!");
            // transient Fight Context.  Once the fight is over, it goes poof!
            return new FightContext(context.Player, [creature], context);
        }

        Console.WriteLine($"There is no '{_creatureName}' here to attack.");
        return context;
    }

    public string GetHelpText() => "attack [creature] - Attack a creature";
}