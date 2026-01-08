using The_World.GameData.Commands;
using The_World.GameData.NPCs;

namespace The_World.GameData.GameMechanics;

public class MerchantContext : Context
{
    public GameContext Game { get; private set; }
    public Merchant Merchant { get; private set; }

    public MerchantContext(Player player, Merchant merchant, GameContext game)
    {
        Player = player;
        Merchant = merchant;
        Game = game;
        Parser = new MerchantCommandParser();

        // Show that we've entered trading mode
        Console.WriteLine($"\n💰 --- TRADING WITH {merchant.Name.ToUpper()} --- 💰");
        ShowMerchantInterface();
    }

    private void ShowMerchantInterface()
    {
        Console.WriteLine($"Your Gold: {Player.Money}"); 
        Console.WriteLine("\nAvailable commands:");
        Console.WriteLine("  buy [item] - Purchase an item");
        Console.WriteLine("  sell [item] - Sell from your inventory");
        Console.WriteLine("  list - Show merchant's inventory");
        Console.WriteLine("  inventory - Show your items");
        Console.WriteLine("  exit - Leave merchant");
    }
}    