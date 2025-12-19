using The_World.GameData.GameMechanics;
using The_World.GameData.NPCs;
using System.Linq;
using The_World.GameData.Items;

namespace The_World.GameData.Commands;

public class SellCommand : ICommand
{
    private readonly string _itemName;

    public SellCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't sell anything right now.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Sell what? Try 'sell [item]'");
            ShowInventory(context);
            return context;
        }

        // Find the item in player's inventory
        var item = context.Player.Inventory.FirstOrDefault(i => 
            i.Name.Contains(_itemName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine($"You don't have '{_itemName}' to sell.");
            ShowInventory(context);
            return context;
        }

        // Find a merchant
        var merchant = context.CurrentArea.NPCs.Values
            .OfType<Merchant>()
            .FirstOrDefault();

        if (merchant == null)
        {
            Console.WriteLine("There's no merchant here to sell to.");
            return context;
        }

        // Calculate sell price (Strategy Pattern - different pricing strategies)
        int sellPrice = CalculateSellPrice(item, merchant);

        // Complete the transaction
        context.Player.Inventory.Remove(item);
        context.Player.Money.Earn(sellPrice);

        Console.WriteLine($"You sold {item.Name} for {sellPrice} gold.");
        Console.WriteLine($"You now have {context.Player.Money}.");

        return context;
    }

    // Strategy Pattern - different ways to calculate sell prices
    private int CalculateSellPrice(Item item, Merchant merchant)
    {
        // If merchant normally sells this item, give 50% of retail price
        if (merchant.ItemPrices.TryGetValue(item.Name.ToLower().Replace(" ", "_"), out int retailPrice))
        {
            return retailPrice / 2;
        }

        // Default strategy: base price on item type and weight
        return item switch
        {
            Weapon weapon => weapon.AttackPower * 3,
            Armor armor => armor.DefenseValue * 2,
            Consumable => 5, // Standard price for consumables
            _ => 1 // Minimum price for other items
        };
    }

    private void ShowInventory(GameContext context)
    {
        if (context.Player.Inventory.Count == 0)
        {
            Console.WriteLine("You have nothing to sell.");
        }
        else
        {
            Console.WriteLine("You can sell:");
            foreach (var item in context.Player.Inventory)
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }

    public string GetHelpText() => "sell [item] - Sell an item to a merchant";
}