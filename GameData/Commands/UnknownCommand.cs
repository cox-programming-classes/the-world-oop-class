using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class UnknownCommand : ICommand
{
    private readonly string _commandName;

    public UnknownCommand(string commandName)
    {
        _commandName = commandName;
    }

    public void Execute(GameContext context)
    {
        Console.WriteLine($"Unknown command: {_commandName}");
    }

    public string GetHelpText() => "Unknown command";
}