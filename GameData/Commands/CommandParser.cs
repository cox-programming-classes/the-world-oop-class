using The_World.GameData.Effects;

namespace The_World.GameData.Commands;

public class CommandParser : IParser
{
    private readonly Dictionary<string, Func<string, ICommand>> _commandFactories;

    public CommandParser()
    {
        _commandFactories = new Dictionary<string, Func<string, ICommand>>
        {
            ["look"] = arg => new LookCommand(arg),
            ["go"] = arg => new GoCommand(arg),
            ["get"] = arg => new GetCommand(arg),
            ["inventory"] = _ => new InventoryCommand(),
            ["attack"] = arg => new AttackCommand(),     
            ["use"] = arg => new UseCommand(arg),          
            ["help"] = _ => new HelpCommand(this),
            ["quit"] = _ => new QuitCommand()
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
        return _commandFactories.Keys.ToList();
    }
}