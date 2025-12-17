using The_World.GameData.GameMechanics;
using The_World.GameData.NPCs;  
using System.Linq;              

namespace The_World.GameData.Commands;


public class BuyCommand : ICommand
{
    private readonly string _itemName;

    public BuyCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't buy anything right now.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Buy what? Try 'buy [item]'");
            ShowAvailableItems(context);
            return context;
        }

        // Find a merchant in the current area
        var merchant = context.CurrentArea.NPCs.Values
            .OfType<Merchant>()
            .FirstOrDefault();

        if (merchant == null)
        {
            Console.WriteLine("There's no merchant here to buy from.");
            return context;
        }

        // Check if merchant has the item and get price
        if (!merchant.ItemPrices.TryGetValue(_itemName, out int price))
        {
            Console.WriteLine($"The merchant doesn't sell '{_itemName}'.");
            ShowAvailableItems(context);
            return context;
        }

        // Check if player can afford it
        if (!context.Player.Money.CanAfford(price))
        {
            Console.WriteLine($"You need {price} gold to buy {_itemName}, but you only have {context.Player.Money}.");
            return context;
        }

        // Try to create the item from the game's item library
        var item = context.SpawnItem(_itemName);
        if (item == null)
        {
            Console.WriteLine($"Sorry, {_itemName} is out of stock.");
            return context;
        }

        // Complete the transaction
        context.Player.Money.Spend(price);
        context.Player.Inventory.Add(item);
        
        Console.WriteLine($"You bought {item.Name} for {price} gold.");
        Console.WriteLine($"You now have {context.Player.Money}.");

        return context;
    }

    private void ShowAvailableItems(GameContext context)
    {
        var merchant = context.CurrentArea.NPCs.Values.OfType<Merchant>().FirstOrDefault();
        if (merchant != null)
        {
            Console.WriteLine("Available items:");
            foreach (var (itemName, price) in merchant.ItemPrices)
            {
                Console.WriteLine($"  {itemName} - {price} gold");
            }
        }
    }

    public string GetHelpText() => "buy [item] - Purchase an item from a merchant";
}