using The_World.GameData.Commands;
using The_World.GameData.Creatures;
using System.Linq; // ADD THIS - needed for .Sum() and .First()

namespace The_World.GameData.GameMechanics;

public class WinFightContext : Context
{
    public GameContext Game { get; private set; }
    private bool _hasShownVictory = false;
    
    public WinFightContext(Player player, List<Creature> defeatedCreatures, GameContext game)
    {
        Player = player;
        Game = game;
        Parser = new CommandParser(); // Use regular parser
        
        // Store the player's level before adding XP to check for level ups
        int previousLevel = player.Level.Value;
        
        // Calculate and award experience points
        double totalXP = defeatedCreatures.Sum(creature => creature.XP);
        player.AddExperience(totalXP);
        
        // Display victory message
        Console.WriteLine("\n🎉 === VICTORY! === 🎉");
        Console.WriteLine("You have defeated your enemies!");
        Console.WriteLine($"You gained {totalXP:F1} experience points!");
        
        // Show defeated creatures
        if (defeatedCreatures.Count == 1)
        {
            Console.WriteLine($"The {defeatedCreatures.First().Name} has been defeated.");
        }
        else
        {
            Console.WriteLine($"You defeated {defeatedCreatures.Count} creatures:");
            foreach (var creature in defeatedCreatures)
            {
                Console.WriteLine($"  - {creature.Name} ({creature.XP:F1} XP)");
            }
        }
        
        // Check for level up
        CheckForLevelUp(previousLevel, player.Level.Value);
        
        // Handle loot drops
        HandleLootDrops(defeatedCreatures);
        
        // Show current stats
        Console.WriteLine($"\nCurrent Status:");
        Console.WriteLine($"Level: {player.Level.Value} | Health: {player.Stats.Health} | XP: {player.Level.Experience:F1}");
        
        Console.WriteLine("\nPress any key or enter any command to return to exploring...");
        _hasShownVictory = true;
    }
    
    // REMOVE THIS METHOD - it's causing issues with the main game loop
    // The transition should happen automatically after the constructor
    
    private void CheckForLevelUp(int previousLevel, int currentLevel)
    {
        if (currentLevel > previousLevel)
        {
            Console.WriteLine($"\n⭐ LEVEL UP! ⭐");
            Console.WriteLine($"You are now level {currentLevel}!");
            
            // Add stat increases on level up
            var healthIncrease = 10;
            var manaIncrease = 5;
            
            Player.Stats.Health = Math.Min(100, Player.Stats.Health + healthIncrease);
            Player.Stats.Mana = Math.Min(50, Player.Stats.Mana + manaIncrease);
            
            Console.WriteLine($"Your health increased by {healthIncrease}! Your mana increased by {manaIncrease}!");
        }
    }
    
    
    private void HandleLootDrops(List<Creature> defeatedCreatures)
    {
        // Simple loot drop system using dice rolls
        var lootDice = new Dice(1, 100); // d100 for loot chance
        
        foreach (var creature in defeatedCreatures)
        {
            var lootRoll = lootDice.Roll();
            
            // 30% chance to drop loot per creature
            if (lootRoll <= 30)
            {
                // Use the ItemLibrary from GameContext to spawn random loot
                var availableItems = Game.GetAvailableItems();
                if (availableItems.Count > 0)
                {
                    var randomItemKey = availableItems[Random.Shared.Next(availableItems.Count)];
                    var lootItem = Game.SpawnItem(randomItemKey);
                    
                    if (lootItem != null)
                    {
                        // Use a safe key that won't conflict
                        var lootKey = $"{lootItem.Name.ToLower()}_{Random.Shared.Next(1000)}";
                        Game.CurrentArea.Items.TryAdd(lootKey, lootItem);
                        Console.WriteLine($"The {creature.Name} dropped: {lootItem.Name}!");
                    }
                }
            }
        }
    }
}
