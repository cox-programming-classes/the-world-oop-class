using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class QuitCommand : ICommand
{
    public Context Execute(Context context)
    {
        context.KeepPlaying = false;
        Console.WriteLine("Thanks for playing!");
        return context;
    }

    public string GetHelpText() => "quit - Exit game";
}