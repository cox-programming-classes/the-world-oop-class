using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

/// <summary>
/// 
/// </summary>
public interface ICommand
{
    Context Execute(Context context);
    string GetHelpText();
}