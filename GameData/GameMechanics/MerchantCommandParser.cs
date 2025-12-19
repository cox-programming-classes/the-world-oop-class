using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class MerchantCommandParser : IParser
{
    private readonly Dictionary<string, Func<string, ICommand>> _commandFactories;

    public MerchantCommandParser()
    {
        _commandFactories = new Dictionary<string, Func<string, ICommand>>
        {
            ["buy"] = arg => new BuyCommand(arg),
            ["sell"] = arg => new SellCommand(arg),
            ["list"] = _ => new ListMerchantItemsCommand(),
            ["inventory"] = _ => new InventoryCommand(),
            ["exit"] = _ => new ExitMerchantCommand(),
            ["leave"] = _ => new ExitMerchantCommand(),
            ["look"] = arg => new LookMerchantCommand(arg)
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