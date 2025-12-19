using The_World.GameData.Abilities;
using The_World.GameData.Creatures;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class UseAbilityCommand : ICommand
{
    private readonly string _abilityName;
    private readonly string _target;

    public UseAbilityCommand(string abilityName, string target = "")
    {
        _abilityName = abilityName?.Trim() ?? "";
        _target = target?.Trim() ?? "";
    }

    public Context Execute(Context context)
    {
        
        if (string.IsNullOrWhiteSpace(_abilityName))
        {
            Console.WriteLine("Use which ability? Try 'use [ability name]'");
            return context;
        }

        // Get ability using Factory Method pattern
        var ability = GetAbilityByName(_abilityName);
        if (ability == null)
        {
            Console.WriteLine($"You don't know an ability called '{_abilityName}'");
            Console.WriteLine("Available abilities: attack, heal, run");
            return context;
        }

        // Check if ability can be used in current context
        if (!CanUseAbility(ability, context))
        {
            Console.WriteLine($"You can't use {ability.Name} right now.");
            return context;
        }

        // Use the ability - Strategy Pattern in action
        var result = UseAbility(ability, context);
        Console.WriteLine(result.Message);

        // Handle context switching for state transitions
        return result.NewContext ?? context;
    }

    // Factory Method Pattern - creates abilities based on name
    private IAbilities? GetAbilityByName(string name) => name.ToLower() switch
    {
        "attack" or "basic attack" => new BasicAttack(),
        "heal" or "basic heal" => new BasicHeal(),
        "run" or "flee" or "run away" => new RunFromFight(),
        _ => null
    };

    // Polymorphism - handle different ability types
    private bool CanUseAbility(IAbilities ability, Context context)
    {
        return ability switch
        {
            BasicAttack attack => context is FightContext fightContext && 
                                 fightContext.Creatures.Any(),
            BasicHeal heal => context is GameContext gameContext && 
                             context.Player.Stats.Health < 100,
            RunFromFight run => context is FightContext,
            _ => false
        };
    }

    // Strategy Pattern - different abilities have different execution logic
    private AbilityResult UseAbility(IAbilities ability, Context context)
    {
        return ability switch
        {
            BasicAttack attack when context is FightContext fightContext => 
                ExecuteAttack(attack, fightContext),
            BasicHeal heal when context is GameContext gameContext => 
                ExecuteHeal(heal, gameContext),
            RunFromFight run when context is FightContext fightContext => 
                ExecuteRun(run, fightContext),
            _ => new AbilityResult("You can't use that ability right now.", context)
        };
    }

    private AbilityResult ExecuteAttack(BasicAttack attack, FightContext context)
    {
        var target = context.Creatures.FirstOrDefault();
        if (target == null)
        {
            return new AbilityResult("There's nothing to attack!", context);
        }

        var result = attack.Use(context, target);
        return new AbilityResult(result, context);
    }

    private AbilityResult ExecuteHeal(BasicHeal heal, GameContext context)
    {
        var result = heal.Use(context, "");
        return new AbilityResult(result, context);
    }

    private AbilityResult ExecuteRun(RunFromFight run, FightContext context)
    {
        var result = run.Use(context);
        
        // Check if run was successful (simple check)
        if (result.Contains("successfully"))
        {
            // Transition back to game context
            return new AbilityResult(result, context.Game);
        }
        
        return new AbilityResult(result, context);
    }

    public string GetHelpText() => "use [ability] - Use a learned ability (attack, heal, run)";
}

// Helper class to encapsulate ability results
public record AbilityResult(string Message, Context? NewContext = null);