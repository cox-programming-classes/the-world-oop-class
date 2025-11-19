using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class GetCommand : ICommand
{
    private readonly string _itemName;

    public GetCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public void Execute(GameContext context)
    {
        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Get what? Try 'get [item]'");
            return;
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
    }

    public string GetHelpText() => "get [item] - Pick up an item";
}