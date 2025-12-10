namespace The_World.GameData.Commands;
/// <summary>
/// Abstract Parser for different CommandParser Implementations based on state!
/// this can be an Interface, because we only really need to define the Parse method.
/// </summary>
public interface IParser
{
    public ICommand Parse(string input);
    public List <string> GetAvailableCommands();
}