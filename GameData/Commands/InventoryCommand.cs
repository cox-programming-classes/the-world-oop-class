using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class InventoryCommand : ICommand
{
    public Context Execute(Context context)
    {
        if (context.Player.Inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
        }
        else
        {
            Console.WriteLine("You are carrying:");
            foreach (var item in context.Player.Inventory)
            {
                Console.WriteLine($"  {item.Name} ({item.Weight} lbs)");
            }
        }
        return context;
    }

    public string GetHelpText() => "inventory - List what items you are carrying";
}