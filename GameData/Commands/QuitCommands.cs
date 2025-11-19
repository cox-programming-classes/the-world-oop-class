using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class QuitCommand : ICommand
{
    public void Execute(GameContext context)
    {
        context.KeepPlaying = false;
        Console.WriteLine("Thanks for playing!");
    }

    public string GetHelpText() => "quit - Exit game";
}