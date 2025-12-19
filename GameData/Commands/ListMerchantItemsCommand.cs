using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class ListMerchantItemsCommand : ICommand
{
    public Context Execute(Context c)
    {
        if (c is not MerchantContext context)
        {
            Console.WriteLine("You can only list items when talking to a merchant.");
            return c;
        }

        Console.WriteLine($"\n{context.Merchant.Name}'s Inventory:");
        foreach (var (itemName, price) in context.Merchant.ItemPrices)
        {
            Console.WriteLine($"  {itemName} - {price} gold");
        }
        Console.WriteLine($"\nYour gold: {context.Player.Money}");
        
        return context;
    }

    public string GetHelpText() => "list - Show merchant's available items";
}