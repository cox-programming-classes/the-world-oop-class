using The_World.GameData.GameMechanics;
using The_World.GameData.Items;


namespace The_World.GameData.Commands;

public class UseCommand : ICommand
{
    private readonly string _itemName;

    public UseCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't do that right now!");
            return c;
        }
        
        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Use what? Try 'use [item]'");
            return context;
        }

        // Find the item in the player's inventory
        var item = context.Player.Inventory.FirstOrDefault(i => 
            i.Name.Equals(_itemName, StringComparison.OrdinalIgnoreCase));
   
        if (item is null)
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

        return context;
    }

    public string GetHelpText() => "use [item] - Use an item";
}