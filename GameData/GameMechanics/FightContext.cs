using The_World.GameData.Commands;
using The_World.GameData.Creatures;

namespace The_World.GameData.GameMechanics;

public class FightContext : Context
{
    public GameContext Game { get; private set; }
    public List<Creature> Creatures { get; private set; }
    public List<Creature> DefeatedCreatures { get; private set; } = new List<Creature>();

    public FightContext(Player player, List<Creature> creatures, GameContext game)
    {
        Creatures = creatures;
        Player = player;
        Game = game;
        Parser = new FightCommandParser();
        
        // Show that combat has started and available commands
        Console.WriteLine("\n🗡️ --- COMBAT MODE ACTIVATED --- 🗡️");
        ShowCombatStatus();
        ShowAvailableCommands();
    }
    
    private void ShowCombatStatus()
    {
        Console.WriteLine($"Your Health: {Player.Stats.Health}");
        Console.WriteLine("Enemies in combat:");
        for (int i = 0; i < Creatures.Count; i++)
        {
            var creature = Creatures[i];
            Console.WriteLine($"  {i + 1}. {creature.Name} (Health: {creature.Stats.Health})");
        }
    }
    
    private void ShowAvailableCommands()
    {
        Console.WriteLine("\nWhat do you want to do?");
        Console.WriteLine("Available commands:");
        Console.WriteLine("  (a)ttack [target] - Attack a creature");
        Console.WriteLine("  (d)efend - Raise your guard");
        Console.WriteLine("  (u)se [item] - Use an item from inventory");
        Console.WriteLine("  (r)un - Attempt to flee from combat");
        Console.WriteLine("  (l)ook [target] - Look around or at specific target");
        Console.WriteLine("  (h)elp - Show help message");
    }
}