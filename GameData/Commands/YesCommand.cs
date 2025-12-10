using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class YesCommand : ICommand
{
    public Context Execute(Context c)
    {
        //only place i use a yes/no right now code your own things if you want this 
        if (c is not LoseFightContext context)
        {
            Console.WriteLine("You don't know what you want.");
        }
//hold on 
        return c;
    }
    
    public string GetHelpText() => "agree with something";
}