using The_World.GameData.Commands;
using The_World.GameData.Creatures;

namespace The_World.GameData.GameMechanics;

public class FightContext : Context
{
    public GameContext Game { get; private set; }
    
    public List<Creature> Creatures { get; private set; }

    public FightContext(Player player, List<Creature> creatures, GameContext game)
    {
        Creatures = creatures;
        Player = player;
        Game = game;
        Parser = new FightCommandParser();
    }
}