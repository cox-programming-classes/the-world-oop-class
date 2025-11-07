namespace The_World.GameData.Commands;

public class LookCommand : ICommand
{
    private readonly string _target;

    public LookCommand(string target = "")
    {
        _target = target?.Trim() ?? "";
    }

    public void Execute(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_target))
        {
            Console.WriteLine(context.CurrentArea.LookAround());
        }
        else
        {
            Console.WriteLine(context.CurrentArea.LookAt(_target));
        }
    }

    public string GetHelpText() => "look [item] - Look around or at specific item";
}