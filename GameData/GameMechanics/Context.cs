using The_World.GameData.Commands;

namespace The_World.GameData.GameMechanics;

// We put the shared parts of the class in this abstract level 
// We use the parser to handle the commands, the context excecutes them

public abstract class Context
{
    public bool KeepPlaying { get; set; } = true;
    
    /// <summary>
    /// Every Context needs the Player
    /// </summary>
    public Player Player { get; protected init; }
    
    /// <summary>
    /// Any Context needs to be able to Parse its commands.
    /// </summary>
    public IParser Parser { get; protected init; }
}