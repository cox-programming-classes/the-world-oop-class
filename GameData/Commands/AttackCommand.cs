/* GameData/Commands/AttackCommand.cs */
using The_World.GameData.GameMechanics;
using The_World.GameData.Abilities;
using The_World.GameData.Creatures;
using The_World.GameData.Equipment;

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
            Console.WriteLine("There are no enemies left to fight!");
            return new WinFightContext(context.Player, [], context.Game);

        }

        // Your existing damage calculation
        var randomNumber = Dice.D6.Roll();
        var playerLevel = context.Player.Level;
        var creatureLevel = targetCreature.Level;
        var equipmentBuff = context.Player.Equipment.GetTotalAttack();
        var baseDamage = Math.Pow(2, (randomNumber * 2 - (creatureLevel - playerLevel))) + equipmentBuff;

        // Apply level difference modifier
        var levelDifference = creatureLevel - playerLevel;
        var damageModifier = levelDifference switch
        {
            > 20 => 0.1, // Very high level creature - minimal damage
            > 0 => 0.5, // Higher level creature - reduced damage  
            0 => 1.0, // Same level - full damage
            _ => 1.5 // Lower level creature - bonus damage
        };

        var finalDamage = (int)(baseDamage * damageModifier);
        finalDamage = Math.Max(1, finalDamage); // Ensure at least 1 damage

        Console.WriteLine($"You deal {finalDamage} damage to the {targetCreature.Name}!");

        // Apply damage to creature (you'll need to add this to Creature class)
        // For now, let's assume creatures die in one hit and remove them
        context.Creatures.Remove(targetCreature);
        context.Game.CurrentArea.Creatures.Remove(_creatureName);

        Console.WriteLine($"The {targetCreature.Name} has been defeated!");

        // Check if all creatures are defeated
        if (!context.Creatures.Any())
        {
            return new WinFightContext(context.Player, [targetCreature], context.Game);
        }

        // TODO: Add creature counter-attack logic here
        // TODO: Check if player dies -> return LoseFightContext
        
        return context; // Continue fighting
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
