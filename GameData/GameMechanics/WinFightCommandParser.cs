using The_World.GameData.Commands;

namespace The_World.GameData.Commands;

public class WinFightCommandParser : IParser
{
    private readonly Dictionary<string, Func<string, ICommand>> _commandFactories;

    public WinFightCommandParser()
    {
        _commandFactories = new Dictionary<string, Func<string, ICommand>>
        {
            ["continue"] = _ => new ContinueCommand(),
            ["c"] = _ => new ContinueCommand(),
            ["look"] = arg => new LookCommand(arg),
            ["l"] = arg => new LookCommand(arg),
            ["inventory"] = _ => new InventoryCommand(),
            ["i"] = _ => new InventoryCommand(),
            ["help"] = _ => new HelpCommand(new CommandParser()), // Fixed: HelpCommand needs a parser
            ["h"] = _ => new HelpCommand(new CommandParser())
        };
    }

    public ICommand Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new UnknownCommand("");

        var parts = input.Split(' ', 2);
        var commandName = parts[0].ToLower();
        var argument = parts.Length > 1 ? parts[1] : "";

        return _commandFactories.TryGetValue(commandName, out var factory) 
            ? factory(argument)
            : new UnknownCommand(commandName);
    }

    public List<string> GetAvailableCommands()
    {
        return new List<string> { "continue", "look", "inventory", "help" };
    }
}