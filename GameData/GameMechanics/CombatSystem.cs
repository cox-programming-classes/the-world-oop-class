using System;
using The_World.GameData;
using The_World.GameData.Creatures;
using System.Collections.Generic;
using System.Linq;

namespace The_World.GameData.GameMechanics;

// State Machine pattern - each combat state is a different record type (Discriminated Union)
public abstract record CombatState
{
    public abstract CombatState HandleTurn(CombatContext combat);
    public abstract string GetStateDescription();
}

// Specific combat states
public record PlayerTurnState : CombatState
{
    public override CombatState HandleTurn(CombatContext combat)
    {
        Console.WriteLine("\n--- Your Turn ---");
        Console.WriteLine("Choose an action: (a)ttack, (d)efend, (u)se item, (r)un");
        
        var input = Console.ReadLine()?.ToLower().Trim();
        
        return input switch
        {
            "a" or "attack" => HandlePlayerAttack(combat),
            "d" or "defend" => HandlePlayerDefend(combat),
            "u" or "use" => HandlePlayerUseItem(combat),
            "r" or "run" => new CombatEndState("You successfully ran away!"),
            _ => this // Stay in same state if invalid input
        };
    }

    private CombatState HandlePlayerAttack(CombatContext combat)
    {
        // Show available targets
        Console.WriteLine("Choose your target:");
        for (int i = 0; i < combat.Enemies.Count; i++)
        {
            var enemy = combat.Enemies[i];
            Console.WriteLine($"{i + 1}. {enemy.Name} (Health: {enemy.Stats.Health})");
        }

        // Get player's target choice
        var targetInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(targetInput, out int targetIndex) || 
            targetIndex < 1 || targetIndex > combat.Enemies.Count)
        {
            Console.WriteLine("Invalid target! Turn wasted.");
            return new EnemyTurnState();
        }

        var targetEnemy = combat.Enemies[targetIndex - 1];
        
        var attackRoll = new Dice(1, 20).Roll(); // d20 attack roll
        var damage = new Dice(1, 6, 2).Roll(); // 1d6+2 damage
        
        Console.WriteLine($"You attack {targetEnemy.Name}! (Roll: {attackRoll})");
        
        if (attackRoll >= 10) // Simple hit threshold
        {
            // Create new StatChart with reduced health
            var newHealth = Math.Max(0, targetEnemy.Stats.Health - damage);
            var newStats = new StatChart { Health = newHealth, Mana = targetEnemy.Stats.Mana };
            
            // Create new creature with updated stats and replace in list
            var updatedEnemy = targetEnemy with { Stats = newStats };
            combat.Enemies[targetIndex - 1] = updatedEnemy;
            
            Console.WriteLine($"Hit! You deal {damage} damage to {updatedEnemy.Name}!");
            Console.WriteLine($"{updatedEnemy.Name} has {updatedEnemy.Stats.Health} health remaining.");
            
            if (updatedEnemy.Stats.Health <= 0)
            {
                Console.WriteLine($"{updatedEnemy.Name} is defeated!");
                combat.Player.AddExperience(updatedEnemy.XP);
                combat.Enemies.RemoveAt(targetIndex - 1); // Remove defeated enemy
                
                // Check if all enemies are defeated
                if (combat.Enemies.Count == 0)
                {
                    return new CombatEndState($"Victory! You defeated all enemies! Total XP gained this combat!");
                }
            }
        }
        else
        {
            Console.WriteLine("Miss! Your attack fails to connect.");
        }
        
        return new EnemyTurnState();
    }

    private CombatState HandlePlayerDefend(CombatContext combat)
    {
        combat.PlayerDefending = true;
        Console.WriteLine("You raise your guard, reducing incoming damage this turn.");
        return new EnemyTurnState();
    }

    private CombatState HandlePlayerUseItem(CombatContext combat)
    {
        Console.WriteLine("Item usage not fully implemented yet.");
        return new EnemyTurnState();
    }

    public override string GetStateDescription() => "Player's turn to act";
}

public record EnemyTurnState : CombatState
{
    private int currentEnemyIndex = 0;

    public override CombatState HandleTurn(CombatContext combat)
    {
        // Each enemy gets a turn
        if (currentEnemyIndex >= combat.Enemies.Count)
        {
            // All enemies have acted, reset for next round
            return new PlayerTurnState();
        }

        var currentEnemy = combat.Enemies[currentEnemyIndex];
        Console.WriteLine($"\n--- {currentEnemy.Name}'s Turn ---");
        
        var attackRoll = new Dice(1, 20).Roll();
        var damage = new Dice(1, 4, 1).Roll(); // Enemy does 1d4+1 damage
        
        Console.WriteLine($"{currentEnemy.Name} attacks! (Roll: {attackRoll})");
        
        if (attackRoll >= 8) // Enemies have easier time hitting
        {
            if (combat.PlayerDefending)
            {
                damage = Math.Max(1, damage / 2); // Defending reduces damage
                Console.WriteLine("Your defense reduces the damage!");
            }
            
            // Reduce player health directly
            var currentHealth = combat.Player.Stats.Health;
            var newHealth = Math.Max(0, currentHealth - damage);
            combat.Player.Stats.Health = newHealth;
            
            Console.WriteLine($"Hit! {currentEnemy.Name} deals {damage} damage to you!");
            Console.WriteLine($"You have {combat.Player.Stats.Health} health remaining.");
            
            if (combat.Player.Stats.Health <= 0)
            {
                return new CombatEndState("You have been defeated! Game Over.");
            }
        }
        else
        {
            Console.WriteLine($"{currentEnemy.Name}'s attack misses!");
        }

        // Move to next enemy
        currentEnemyIndex++;
        
        // If more enemies need to act, stay in EnemyTurnState
        if (currentEnemyIndex < combat.Enemies.Count)
        {
            return new EnemyTurnState { currentEnemyIndex = currentEnemyIndex };
        }
        
        // All enemies acted, reset defending state and go to player turn
        combat.PlayerDefending = false;
        return new PlayerTurnState();
    }

    public override string GetStateDescription() => "Enemies' turn to act";
}

public record CombatEndState(string EndMessage) : CombatState
{
    public override CombatState HandleTurn(CombatContext combat)
    {
        Console.WriteLine($"\n--- Combat Ended ---");
        Console.WriteLine(EndMessage);
        combat.IsActive = false;
        return this;
    }

    public override string GetStateDescription() => "Combat has ended";
}

// Combat context holds all the data needed during combat
public class CombatContext
{
    public Player Player { get; set; }
    public List<Creature> Enemies { get; set; } // Changed from single Enemy to List<Creature>
    public bool IsActive { get; set; } = true;
    public bool PlayerDefending { get; set; } = false;
    public CombatState CurrentState { get; set; }

    // Constructor for single enemy (backwards compatibility)
    public CombatContext(Player player, Creature enemy)
    {
        Player = player;
        Enemies = new List<Creature> { enemy };
        CurrentState = new PlayerTurnState();
    }

    // Constructor for multiple enemies
    public CombatContext(Player player, List<Creature> enemies)
    {
        Player = player;
        Enemies = new List<Creature>(enemies); // Create a copy to avoid external modifications
        CurrentState = new PlayerTurnState();
    }

    public void RunCombat()
    {
        var enemyNames = string.Join(", ", Enemies.Select(e => e.Name));
        Console.WriteLine($"\n🗡️ Combat begins with {enemyNames}! 🗡️");
        Console.WriteLine($"Your Health: {Player.Stats.Health}");
        
        foreach (var enemy in Enemies)
        {
            Console.WriteLine($"{enemy.Name} Health: {enemy.Stats.Health}");
        }
        
        while (IsActive && Enemies.Count > 0)
        {
            CurrentState = CurrentState.HandleTurn(this);
        }
    }
}
