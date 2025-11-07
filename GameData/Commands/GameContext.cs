using The_World.GameData.Areas;

namespace The_World.GameData.Commands;

public class GameContext
{
    public Player Player { get; set; }
    public Area CurrentArea { get; set; }
    public bool KeepPlaying { get; set; } = true;

    public GameContext(Player player, Area currentArea)
    {
        Player = player;
        CurrentArea = currentArea;
    }
}