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
            // TODO: Implement actual combat mechanics
            Console.WriteLine($"You attack the {creature.Name}!");
            Console.WriteLine("Combat system not yet implemented.");
        }
        else
        {
            Console.WriteLine($"There is no '{_creatureName}' here to attack.");
        }
    }

    public string GetHelpText() => "attack [creature] - Attack a creature";
}