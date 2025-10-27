namespace The_World.GameData.Commands;

public interface ICommand
{
    void Execute(GameContext context);
    string GetHelpText();
}