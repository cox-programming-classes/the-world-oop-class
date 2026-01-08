using The_World.GameData.Commands;
using The_World.GameData.Creatures;
using System.Linq;

namespace The_World.GameData.GameMechanics;

public class WinFightContext : Context
{
    public GameContext Game { get; private set; }
    
    public WinFightContext(Player player, List<Creature> defeatedCreatures, GameContext game)
    {
        Player = player;
        Game = game;
        Parser = new SimpleReturnParser(game); // Simple parser that just returns to game
        
        // Your existing victory logic...
        int previousLevel = player.Level.Value;
        double totalXP = defeatedCreatures.Sum(creature => creature.XP);
        player.AddExperience(totalXP);
        
        Console.WriteLine("\n🎉 === VICTORY! === 🎉");
        Console.WriteLine("You have defeated your enemies!");
        Console.WriteLine($"You gained {totalXP:F1} experience points!");
        
        if (defeatedCreatures.Count == 1)
        {
            Console.WriteLine($"The {defeatedCreatures.First().Name} has been defeated.");
        }
        
        CheckForLevelUp(previousLevel, player.Level.Value);
        HandleLootDrops(defeatedCreatures);
        
        Console.WriteLine($"\nLevel: {player.Level.Value} | Health: {player.Stats.Health}");
        Console.WriteLine("\nPress any key to return to exploring...");
    }
    
    private void CheckForLevelUp(int previousLevel, int currentLevel)
    {
        if (currentLevel > previousLevel)
        {
            Console.WriteLine($"\n⭐ LEVEL UP! You are now level {currentLevel}! ⭐");
            Player.Stats.Health = Math.Min(100, Player.Stats.Health + 10);
            Player.Stats.Mana = Math.Min(50, Player.Stats.Mana + 5);
        }
    }
    
    private void HandleLootDrops(List<Creature> defeatedCreatures)
    {
        var lootDice = new Dice(1, 100);
        foreach (var creature in defeatedCreatures)
        {
            if (lootDice.Roll() <= 30) // 30% chance
            {
                var availableItems = Game.GetAvailableItems();
                if (availableItems.Count > 0)
                {
                    var randomItemKey = availableItems[Random.Shared.Next(availableItems.Count)];
                    var lootItem = Game.SpawnItem(randomItemKey);
                    if (lootItem != null)
                    {
                        var lootKey = $"{lootItem.Name.ToLower()}_{Random.Shared.Next(1000)}";
                        Game.CurrentArea.Items.TryAdd(lootKey, lootItem);
                        Console.WriteLine($"The {creature.Name} dropped: {lootItem.Name}!");
                    }
                }
            }
        }
    }
}

// Simple parser - any input returns to game
public class SimpleReturnParser : IParser
{
    private readonly GameContext _game;
    
    public SimpleReturnParser(GameContext game)
    {
        _game = game;
    }
    
    public ICommand Parse(string input)
    {
        return new SimpleReturnCommand(_game);
    }
    
    public List<string> GetAvailableCommands()
    {
        return new List<string> { "any key" };
    }
}

// Simple command - just returns to game
public class SimpleReturnCommand : ICommand
{
    private readonly GameContext _game;
    
    public SimpleReturnCommand(GameContext game)
    {
        _game = game;
    }
    
    public Context Execute(Context context)
    {
        Console.WriteLine("Returning to exploration...\n");
        Console.WriteLine(_game.CurrentArea.LookAround());
        return _game;
    }
    
    public string GetHelpText() => "Return to game";
}
