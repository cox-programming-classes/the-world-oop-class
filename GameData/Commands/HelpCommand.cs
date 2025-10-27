namespace The_World.GameData.Commands;

public class HelpCommand : ICommand
{
    private readonly CommandParser _parser;

    public HelpCommand(CommandParser parser)
    {
        _parser = parser;
    }

    public void Execute(GameContext context)
    {
        Console.WriteLine("Available commands:");
        
        // Create instances of each command to get their help text
        var commands = new ICommand[]
        {
            new LookCommand(),
            new GoCommand(),
            new GetCommand(),
            new InventoryCommand(),
            new HelpCommand(_parser),
            new QuitCommand()
        };

        foreach (var command in commands)
        {
            if (command is not HelpCommand) // Avoid infinite recursion
                Console.WriteLine($"  {command.GetHelpText()}");
        }
        Console.WriteLine("  help - Show this help");
    }

    public string GetHelpText() => "help - Show available commands";
}