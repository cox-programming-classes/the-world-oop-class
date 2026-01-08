using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class ExitMerchantCommand : ICommand
{
    public Context Execute(Context c)
    {
        if (c is not MerchantContext context)
        {
            return c;
        }

        Console.WriteLine($"You finish your business with {context.Merchant.Name}.");
        return context.Game; // Return to GameContext
    }

    public string GetHelpText() => "exit - Stop talking to the merchant";
}