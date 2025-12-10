namespace The_World.GameData.Commands;

public class LoseFightCommandParser : IParser
{
    private readonly Dictionary<string, Func<string, ICommand>> _commandFactories;
    
    public LoseFightCommandParser()
    {
        _commandFactories = new Dictionary<string, Func<string, ICommand>>
        {
            ["look"] = arg => new LookCommand(arg)
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
        throw new NotImplementedException();
    }
}
