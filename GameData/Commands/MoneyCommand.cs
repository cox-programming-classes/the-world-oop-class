using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class MoneyCommand : ICommand
{
    public Context Execute(Context context)
    {
        Console.WriteLine($"You have {context.Player.Money}.");
        return context;
    }

    public string GetHelpText() => "money - Check how much gold you have";
}