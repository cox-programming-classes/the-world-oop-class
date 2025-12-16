using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class GetCommand : ICommand
{
    private readonly string _itemName;

    public GetCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }
    
    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't do that right now");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Get what? Try 'get [item]'");
            return c;
        }

        if (context.CurrentArea.Items.TryGetValue(_itemName, out var item))
        {
            context.CurrentArea.Items.Remove(_itemName);
            context.Player.Inventory.Add(item);
            Console.WriteLine($"You pick up the {item.Name}.");
        }
        else
        {
            Console.WriteLine($"There is no '{_itemName}' here to pick up.");
        }
        return c;
    }

    public string GetHelpText() => "get [item] - Pick up an item";
}