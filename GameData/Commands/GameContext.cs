using The_World.GameData.Areas;

namespace The_World.GameData.Commands;

public class GameContext
{
    public Player Player { get; set; }
    public Area CurrentArea { get; set; }
    public bool KeepPlaying { get; set; } = true;
    
    // TODO: A Library of Items that can be poofed into existence at run-time.

    public GameContext(Player player, Area currentArea)
    {
        Player = player;
        CurrentArea = currentArea;
    }
}