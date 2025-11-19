using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class GoCommand : ICommand
{
    private readonly string _direction;

    public GoCommand(string direction = "")
    {
        _direction = direction?.Trim() ?? "";
    }

    public void Execute(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_direction))
        {
            Console.WriteLine("Go where? Try 'go [direction]'");
            return;
        }

        if (context.CurrentArea.ConnectedAreas.TryGetValue(_direction, out var newArea))
        {
            context.CurrentArea = newArea;
            Console.WriteLine($"You move to the {context.CurrentArea.Name}.");
            Console.WriteLine(context.CurrentArea.Description);
        }
        else
        {
            Console.WriteLine($"You can't go '{_direction}' from here.");
        }
    }

    public string GetHelpText() => "go [direction] - Move to connected area";
}