using System;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class AttackCommand : ICommand
{
    private readonly string _creatureName;

    public AttackCommand(string creatureName = "")
    {
        _creatureName = creatureName?.Trim() ?? "";
    }

    public void Execute(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_creatureName))
        {
            Console.WriteLine("Attack what? Try 'attack [creature]'");
            return;
        }

        if (context.CurrentArea.Creatures.TryGetValue(_creatureName, out var creature))
        {
            // Start combat using the new combat system!
            var combat = new CombatContext(context.Player, creature);
            combat.RunCombat();
            
            // Remove defeated creatures from the area
            if (combat.Enemy.Stats.Health <= 0)
            {
                context.CurrentArea.Creatures.Remove(_creatureName);
            }
        }
        else
        {
            Console.WriteLine($"There is no '{_creatureName}' here to attack.");
        }
    }

    public string GetHelpText() => "attack [creature] - Attack a creature";
}