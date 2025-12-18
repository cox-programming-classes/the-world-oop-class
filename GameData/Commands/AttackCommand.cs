/* GameData/Commands/AttackCommand.cs */
using The_World.GameData.GameMechanics;
using The_World.GameData.Abilities;
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
        // Strategy Pattern - using BasicAttack ability
        var attack = new BasicAttack();
        var targetCreature = SelectTarget(context);
        
        if (targetCreature == null)
        {
            Console.WriteLine("You swing at empty air!");
            return context;
        }

        // Apply the attack using the Strategy Pattern
        string attackResult = attack.Use(context, targetCreature);
        Console.WriteLine(attackResult);
        
        // Check if target was defeated
        if (targetCreature.Stats.Health <= 0)
        {
            Console.WriteLine($"{targetCreature.Name} is defeated!");
            context.Player.AddExperience(targetCreature.XP);
            context.Creatures.Remove(targetCreature);
            
            if (context.Creatures.Count == 0)
            {
                Console.WriteLine("Victory! All enemies defeated!");
                // Return to game context instead of incomplete WinFightContext
                return context.Game;
            }
        }
        
        return context;
    }

    private Creature? SelectTarget(FightContext context)
    {
        if (string.IsNullOrWhiteSpace(_creatureName))
        {
            // If no specific target, attack first available creature
            return context.Creatures.FirstOrDefault();
        }
        
        // Find creature by name (case-insensitive partial match)
        return context.Creatures.FirstOrDefault(c => 
            c.Name.Contains(_creatureName, StringComparison.OrdinalIgnoreCase));
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
            Console.WriteLine($"You attack the {creature.Name}!");
            // State Machine Pattern - transitioning to Fight Context
            return new FightContext(context.Player, [creature], context);
        }

        Console.WriteLine($"There is no '{_creatureName}' here to attack.");
        return context;
    }

    public string GetHelpText() => "attack [creature] - Attack a creature";
}
