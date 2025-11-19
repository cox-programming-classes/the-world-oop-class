using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class InventoryCommand : ICommand
{
    public void Execute(GameContext context)
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
    }

    public string GetHelpText() => "inventory - Show your items";
}