using System;
using The_World.GameData.GameMechanics;
using System.Linq;
using The_World.GameData.Creatures;
using The_World.GameData.Effects;

namespace The_World.GameData.Commands;

public class AttackCommand : ICommand
{
    private readonly string _creatureName;
    private readonly DamageEffect _effect;
    

    public AttackCommand(DamageEffect effect, string creatureName = "")
    {
        _effect = effect;
        _creatureName = creatureName?.Trim() ?? "";
    }

    public AttackCommand()
    {
        throw new NotImplementedException();
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
        var result = _effect.ApplyToCreature(targetCreature, context.Game);
        Console.WriteLine(result);
        // calculating the damage dealt-influenced by player level and creature level (int) 
        //stole the damage code for me --Anne
        
        if (targetCreature.Stats.Health <= 0)
        {
            //TODO: Finish writing WinFightContext there's nothing there right now - Anne
            return new WinFightContext();
        }
        return context.Game;
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