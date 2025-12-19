using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class ContinueCommand : ICommand
{
    public Context Execute(Context context)
    {
        if (context is WinFightContext winContext)
        {
            Console.WriteLine("You return to exploring the area.");
            Console.WriteLine(winContext.Game.CurrentArea.LookAround());
            return winContext.Game; // Transition back to GameContext
        }
        
        Console.WriteLine("You can't do that right now.");
        return context;
    }

    public string GetHelpText() => "continue - Return to exploring after victory";
}