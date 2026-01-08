using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class LookMerchantCommand : ICommand
{
    private readonly string _target;

    public LookMerchantCommand(string target = "")
    {
        _target = target?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not MerchantContext context)
        {
            return c;
        }

        if (string.IsNullOrWhiteSpace(_target))
        {
            Console.WriteLine(context.Merchant.Look());
        }
        else if (context.Merchant.ItemPrices.ContainsKey(_target))
        {
            var item = context.Game.SpawnItem(_target);
            if (item != null)
            {
                Console.WriteLine($"{item.Look()}\nPrice: {context.Merchant.ItemPrices[_target]} gold");
            }
        }
        else
        {
            Console.WriteLine($"The merchant doesn't have '{_target}' for sale.");
        }

        return context;
    }

    public string GetHelpText() => "look [item] - Examine merchant or their items";
}