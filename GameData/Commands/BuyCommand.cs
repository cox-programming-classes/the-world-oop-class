using The_World.GameData.GameMechanics;

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
        if (c is not MerchantContext context)
        {
            Console.WriteLine("You can only buy items when talking to a merchant.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Buy what? Try 'buy [item]'");
            ShowAvailableItems(context);
            return context;
        }

        // Use the merchant from the context (no need to search for one)
        var merchant = context.Merchant;

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
        var item = context.Game.SpawnItem(_itemName);
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

        return context; // Stay in MerchantContext
    }

    private void ShowAvailableItems(MerchantContext context)
    {
        Console.WriteLine("Available items:");
        foreach (var (itemName, price) in context.Merchant.ItemPrices)
        {
            Console.WriteLine($"  {itemName} - {price} gold");
        }
    }

    public string GetHelpText() => "buy [item] - Purchase an item from the merchant";
}
