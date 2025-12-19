using System;
using System.Collections.Generic;
using System.Linq;
using The_World.GameData.GameMechanics;
using The_World.GameData.Items;
using The_World.GameData.Creatures;
using The_World.GameData;

namespace The_World.GameData.Commands;

// Discriminated Union for Fight Commands (instead of interface as requested)
public abstract record FightCommandType
{
    public record Attack(string Target = "") : FightCommandType;
    public record Defend : FightCommandType;
    public record UseItem(string ItemName = "") : FightCommandType;
    public record Run : FightCommandType;
    public record Look(string Target = "") : FightCommandType;
    public record Help : FightCommandType;
    public record Unknown(string CommandName) : FightCommandType;
}

public class FightCommandParser : IParser
{
    private readonly Dictionary<string, Func<string, FightCommandType>> _commandFactories;

    public FightCommandParser()
    {
        // Factory Methods pattern for creating command types
        _commandFactories = new Dictionary<string, Func<string, FightCommandType>>
        {
            ["attack"] = arg => new FightCommandType.Attack(arg),
            ["a"] = arg => new FightCommandType.Attack(arg),
            ["defend"] = _ => new FightCommandType.Defend(),
            ["d"] = _ => new FightCommandType.Defend(),
            ["use"] = arg => new FightCommandType.UseItem(arg),
            ["u"] = arg => new FightCommandType.UseItem(arg),
            ["run"] = _ => new FightCommandType.Run(),
            ["r"] = _ => new FightCommandType.Run(),
            ["flee"] = _ => new FightCommandType.Run(),
            ["look"] = arg => new FightCommandType.Look(arg),
            ["l"] = arg => new FightCommandType.Look(arg),
            ["help"] = _ => new FightCommandType.Help(),
            ["h"] = _ => new FightCommandType.Help(),
            ["?"] = _ => new FightCommandType.Help()
        };
    }

    public ICommand Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new FightCommand(new FightCommandType.Unknown(""));

        var parts = input.Split(' ', 2);
        var commandName = parts[0].ToLower();
        var argument = parts.Length > 1 ? parts[1] : "";

        // Special case: if input is just a number, treat it as "attack [number]"
        if (int.TryParse(commandName, out int targetNumber))
        {
            return new FightCommand(new FightCommandType.Attack(targetNumber.ToString()));
        }

        var commandType = _commandFactories.TryGetValue(commandName, out var factory) 
            ? factory(argument)
            : new FightCommandType.Unknown(commandName);

        return new FightCommand(commandType);
    }

    public List<string> GetAvailableCommands()
    {
        return new List<string>
        {
            "attack", "defend", "use", "run", "look", "help"
        };
    }
}

// Command that wraps the discriminated union
public class FightCommand : ICommand
{
    private readonly FightCommandType _commandType;

    public FightCommand(FightCommandType commandType)
    {
        _commandType = commandType;
    }

    public Context Execute(Context context)
    {
        if (context is not FightContext fightContext)
        {
            Console.WriteLine("Fight commands can only be used during combat!");
            return context;
        }

        // Pattern Matching on Discriminated Union
        return _commandType switch
        {
            FightCommandType.Attack attack => ExecuteAttack(fightContext, attack.Target),
            FightCommandType.Defend => ExecuteDefend(fightContext),
            FightCommandType.UseItem useItem => ExecuteUseItem(fightContext, useItem.ItemName),
            FightCommandType.Run => ExecuteRun(fightContext),
            FightCommandType.Look look => ExecuteLook(fightContext, look.Target),
            FightCommandType.Help => ExecuteHelp(fightContext),
            FightCommandType.Unknown unknown => ExecuteUnknown(fightContext, unknown.CommandName),
            _ => context
        };
    }

    private Context ExecuteAttack(FightContext context, string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            // In fight context, show available targets instead of blind swing
            Console.WriteLine("Attack which creature?");
            for (int i = 0; i < context.Creatures.Count; i++)
            {
                var creature = context.Creatures[i];
                Console.WriteLine($"  {i + 1}. {creature.Name} (Health: {creature.Stats.Health})");
            }
            return context;
        }

        // Try to find target by name or number
        Creature? targetCreature = null;
        int targetIndex = -1;

        // Check if target is a number
        if (int.TryParse(target, out int index) && index >= 1 && index <= context.Creatures.Count)
        {
            targetIndex = index - 1;
            targetCreature = context.Creatures[targetIndex];
        }
        else
        {
            // Find by name (partial match)
            for (int i = 0; i < context.Creatures.Count; i++)
            {
                if (context.Creatures[i].Name.Contains(target, StringComparison.OrdinalIgnoreCase))
                {
                    targetCreature = context.Creatures[i];
                    targetIndex = i;
                    break;
                }
            }
        }

        if (targetCreature == null)
        {
            Console.WriteLine($"There is no '{target}' to attack here.");
            return context;
        }

        // Execute attack using existing combat system
        var attackRoll = new Dice(1, 20).Roll();
        var damage = new Dice(1, 6, 2).Roll();
        
        Console.WriteLine($"You attack {targetCreature.Name}! (Roll: {attackRoll})");
        
        if (attackRoll >= 10)
        {
            var newHealth = Math.Max(0, targetCreature.Stats.Health - damage);
            var newStats = new StatChart { Health = newHealth, Mana = targetCreature.Stats.Mana };
            var updatedCreature = targetCreature with { Stats = newStats };
            
            context.Creatures[targetIndex] = updatedCreature;
            
            Console.WriteLine($"Hit! You deal {damage} damage to {updatedCreature.Name}!");
            Console.WriteLine($"{updatedCreature.Name} has {updatedCreature.Stats.Health} health remaining.");
            
            if (updatedCreature.Stats.Health <= 0)
            {
                Console.WriteLine($"{updatedCreature.Name} is defeated!");
                // TODO:  Move this to the WinContext
                // Give XP and currency rewards
                context.Player.AddExperience(updatedCreature.XP);
                var goldReward = updatedCreature.Level * 5; // 5 gold per creature level
                context.Player.Money.Earn(goldReward);
    
                Console.WriteLine($"You gained {updatedCreature.XP} XP and {goldReward} gold!");
                Console.WriteLine($"You now have {context.Player.Money}.");
    
                
                // Add to defeated creatures list BEFORE removing from active creatures
                context.DefeatedCreatures.Add(updatedCreature);
                context.Creatures.RemoveAt(targetIndex);
                
                if (context.Creatures.Count == 0)
                {
                    // CREATE WinFightContext with ALL defeated creatures
                    return new WinFightContext(context.Player, context.DefeatedCreatures, context.Game);
                }
            }
        }
        else
        {
            Console.WriteLine("Miss! Your attack fails to connect.");
        }

        // Process enemy turns after player attack
        context = ProcessEnemyTurns(context);

        // Check if player is still alive and combat continues
        if (context.Player.Stats.Health <= 0)
        {
            // TODO: Create LoseFightContext here
            Console.WriteLine("💀 You have been defeated! 💀");
            context.KeepPlaying = false;
            return context;
        }
        
        if (context.Creatures.Count == 0)
        {
            // All enemies defeated (shouldn't reach here, but safety check)
            return new WinFightContext(context.Player, context.DefeatedCreatures, context.Game);
        }

        Console.WriteLine("\nWhat do you want to do next?");
        ShowAvailableCommands();

        return context;
    }

    private Context ExecuteDefend(FightContext context)
    {
        Console.WriteLine("You raise your guard, preparing to defend against incoming attacks.");
        // TODO: Implement defend logic - could reduce damage taken next turn
        
        // Process enemy turns after defend
        context = ProcessEnemyTurns(context);

        // Check if player is still alive and combat continues
        if (context.Player.Stats.Health <= 0)
        {
            Console.WriteLine("💀 You have been defeated! 💀");
            context.KeepPlaying = false;
            return context;
        }
        
        if (context.Creatures.Count == 0)
        {
            return new WinFightContext(context.Player, context.DefeatedCreatures, context.Game);
        }

        Console.WriteLine("\nWhat do you want to do next?");
        ShowAvailableCommands();
        
        return context;
    }

    private Context ExecuteUseItem(FightContext context, string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Console.WriteLine("Use which item? Available items:");
            foreach (var inventoryItem in context.Player.Inventory)
            {
                Console.WriteLine($"  {inventoryItem.Name}");
            }
            return context;
        }

        var selectedItem = context.Player.Inventory.FirstOrDefault(i => 
            i.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase));

        if (selectedItem == null)
        {
            Console.WriteLine($"You don't have an item called '{itemName}'.");
            return context;
        }

        // Use existing UseCommand logic but in fight context
        if (selectedItem is Consumable consumable)
        {
            context.Player.Inventory.Remove(selectedItem);
            Console.WriteLine($"You use the {selectedItem.Name} during combat!");
            
            // Apply effect
            string result = consumable.Effect.Apply(context.Game);
            Console.WriteLine(result);

            // Process enemy turns after using item
            context = ProcessEnemyTurns(context);

            // Check if player is still alive and combat continues
            if (context.Player.Stats.Health <= 0)
            {
                Console.WriteLine("💀 You have been defeated! 💀");
                context.KeepPlaying = false;
                return context;
            }
            
            if (context.Creatures.Count == 0)
            {
                return new WinFightContext(context.Player, context.DefeatedCreatures, context.Game);
            }

            Console.WriteLine("\nWhat do you want to do next?");
            ShowAvailableCommands();
        }
        else
        {
            Console.WriteLine($"You can't use {selectedItem.Name} during combat.");
        }

        return context;
    }

    private Context ExecuteRun(FightContext context)
    {
        Console.WriteLine("You attempt to flee from combat!");
        
        // Simple escape chance - could be based on player stats vs enemy stats
        var escapeRoll = new Dice(1, 20).Roll();
        if (escapeRoll >= 10)
        {
            Console.WriteLine("You successfully escape!");
            return context.Game; // Return to game context
        }
        else
        {
            Console.WriteLine("You couldn't escape! The enemies block your path.");
            
            // Process enemy turns after failed escape
            context = ProcessEnemyTurns(context);

            // Check if player is still alive and combat continues
            if (context.Player.Stats.Health <= 0)
            {
                Console.WriteLine("💀 You have been defeated! 💀");
                context.KeepPlaying = false;
                return context;
            }
            
            if (context.Creatures.Count == 0)
            {
                return new WinFightContext(context.Player, context.DefeatedCreatures, context.Game);
            }

            Console.WriteLine("\nWhat do you want to do next?");
            ShowAvailableCommands();
            
            return context;
        }
    }

    private Context ExecuteLook(FightContext context, string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            // Look around the fight
            Console.WriteLine("--- Combat Situation ---");
            Console.WriteLine($"Your Health: {context.Player.Stats.Health}");
            Console.WriteLine("Enemies:");
            foreach (var creature in context.Creatures)
            {
                Console.WriteLine($"  {creature.Name} - Health: {creature.Stats.Health}");
            }
        }
        else
        {
            // Look at specific target
            var creature = context.Creatures.FirstOrDefault(c => 
                c.Name.Contains(target, StringComparison.OrdinalIgnoreCase));
            
            if (creature != null)
            {
                Console.WriteLine(creature.Look());
            }
            else
            {
                Console.WriteLine($"You don't see '{target}' here.");
            }
        }
        
        return context;
    }

    private Context ExecuteHelp(FightContext context)
    {
        Console.WriteLine("Available combat commands:");
        Console.WriteLine("  attack [target] - Attack a creature");
        Console.WriteLine("  defend - Raise your guard");
        Console.WriteLine("  use [item] - Use an item from inventory");
        Console.WriteLine("  run - Attempt to flee from combat");
        Console.WriteLine("  look [target] - Look around or at specific target");
        Console.WriteLine("  help - Show this help message");
        
        return context;
    }

    private Context ExecuteUnknown(FightContext context, string commandName)
    {
        Console.WriteLine($"Unknown combat command: '{commandName}'. Type 'help' for available commands.");
        return context;
    }

    // Process enemy turns after player actions
    private FightContext ProcessEnemyTurns(FightContext context)
    {
        if (context.Creatures.Count == 0) return context; // No enemies left
        
        Console.WriteLine("\n--- Enemy Turn ---");
        
        for (int i = context.Creatures.Count - 1; i >= 0; i--) // Reverse iteration for safe removal
        {
            var creature = context.Creatures[i];
            
            // Simple AI: creature attacks player
            var attackRoll = new Dice(1, 20).Roll();
            var damage = new Dice(1, 4, creature.Level).Roll(); // Scale damage with creature level
            
            Console.WriteLine($"{creature.Name} attacks you! (Roll: {attackRoll})");
            
            if (attackRoll >= 8) // Enemies have slightly easier hit chance
            {
                var oldHealth = context.Player.Stats.Health;
                var newHealth = Math.Max(0, oldHealth - damage);
                context.Player.Stats.Health = newHealth;
                
                Console.WriteLine($"Hit! {creature.Name} deals {damage} damage to you!");
                Console.WriteLine($"Your health: {context.Player.Stats.Health}");
                
                if (context.Player.Stats.Health <= 0)
                {
                    Console.WriteLine("💀 You have been defeated! 💀");
                    context.KeepPlaying = false;
                    return context;
                }
            }
            else
            {
                Console.WriteLine($"Miss! {creature.Name}'s attack fails to connect.");
            }
        }
        
        Console.WriteLine("\n--- Your Turn ---");
        return context;
    }

    // Helper method to show available commands
    // Helper method to show available commands
    // Helper method to show available commands
    private void ShowAvailableCommands()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("  (a)ttack [target] - Attack a creature");
        Console.WriteLine("  (d)efend - Raise your guard");
        Console.WriteLine("  (u)se [item] - Use an item from inventory");
        Console.WriteLine("  (r)un - Attempt to flee from combat");
        Console.WriteLine("  (l)ook [target] - Look around or at specific target");
        Console.WriteLine("  (h)elp - Show help message");
    }

    public string GetHelpText() => "Fight commands - use during combat";
}