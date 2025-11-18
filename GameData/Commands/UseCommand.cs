using The_World.GameData.Items;

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

        // Find the item in the player's inventory
        var item = context.Player.Inventory.FirstOrDefault(i => 
            i.Name.Equals(_itemName, StringComparison.OrdinalIgnoreCase));
   
        if (item == null)
        {
            Console.WriteLine("You don't have an item with that name!");
        }

        else if (item is Consumable consumable)
        {
            context.Player.Inventory.Remove(item);
            Console.WriteLine($"You consumed the {item.Name}.");
            
            string result = consumable.Effect.Apply(context);
            Console.WriteLine(result);
        }
        
        else
        {
            Console.WriteLine("Please don't eat that :(");
        }
        
    }

    public string GetHelpText() => "use [item] - Use an item";
}