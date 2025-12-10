using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class HelpCommand : ICommand
{
    private readonly CommandParser _parser;

    public HelpCommand(CommandParser parser)
    {
        _parser = parser;
    }

    public Context Execute(Context context)
    {
        Console.WriteLine("Available commands:");
        
        // Get all the commands from the current context Parser
        // and print the help text for each command~
        context.Parser.GetAvailableCommands()
            .Select(cmd => context.Parser.Parse(cmd))
            .ToList()
            .ForEach(command => Console.WriteLine(command.GetHelpText()));
        
        return context;
    }

    public string GetHelpText() => "help - Show available commands";
}