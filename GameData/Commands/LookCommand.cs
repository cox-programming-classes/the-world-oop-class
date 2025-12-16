using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class LookCommand : ICommand
{
    private readonly string _target;

    public LookCommand(string target = "")
    {
        _target = target?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't do that right now.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_target))
        {
            Console.WriteLine(context.CurrentArea.LookAround());
        }
        else
        {
            Console.WriteLine(context.CurrentArea.LookAt(_target));
        }

        return c;
    }

    public string GetHelpText() => "look [item] - Look around at the current area or at specific item";
}