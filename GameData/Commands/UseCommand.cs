using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class UseCommand : ICommand
{
    private readonly string _itemName;

    public UseCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public void Execute(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Use what? Try 'use [item]'");
            return;
        }

        Console.WriteLine($"You attempt to use '{_itemName}'.");
        Console.WriteLine("Item usage system not yet implemented.");
    }

    public string GetHelpText() => "use [item] - Use an item";
}