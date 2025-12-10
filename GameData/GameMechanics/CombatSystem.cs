using System;
using The_World.GameData;
using The_World.GameData.Creatures;

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
        var attackRoll = new Dice(1, 20).Roll(); // d20 attack roll
        var damage = new Dice(1, 6, 2).Roll(); // 1d6+2 damage
        
        Console.WriteLine($"You attack! (Roll: {attackRoll})");
        
        if (attackRoll >= 10) // Simple hit threshold
        {
            // Create new StatChart with reduced health
            var newHealth = Math.Max(0, combat.Enemy.Stats.Health - damage);
            var newStats = new StatChart { Health = newHealth, Mana = combat.Enemy.Stats.Mana };
            
            // Create new creature with updated stats
            combat.Enemy = combat.Enemy with { Stats = newStats };
            
            Console.WriteLine($"Hit! You deal {damage} damage to {combat.Enemy.Name}!");
            Console.WriteLine($"{combat.Enemy.Name} has {combat.Enemy.Stats.Health} health remaining.");
            
            if (combat.Enemy.Stats.Health <= 0)
            {
                combat.Player.AddExperience(combat.Enemy.XP);
                return new CombatEndState($"You defeated {combat.Enemy.Name}! Gained {combat.Enemy.XP} XP!");
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
    public override CombatState HandleTurn(CombatContext combat)
    {
        Console.WriteLine($"\n--- {combat.Enemy.Name}'s Turn ---");
        
        var attackRoll = new Dice(1, 20).Roll();
        var damage = new Dice(1, 4, 1).Roll(); // Enemy does 1d4+1 damage
        
        Console.WriteLine($"{combat.Enemy.Name} attacks! (Roll: {attackRoll})");
        
        if (attackRoll >= 8) // Enemies have easier time hitting
        {
            if (combat.PlayerDefending)
            {
                damage = Math.Max(1, damage / 2); // Defending reduces damage
                combat.PlayerDefending = false; // Reset defending state
                Console.WriteLine("Your defense reduces the damage!");
            }
            
            // Reduce player health directly
            var currentHealth = combat.Player.Stats.Health;
            var newHealth = Math.Max(0, currentHealth - damage);
            combat.Player.Stats.Health = newHealth;
            
            Console.WriteLine($"Hit! {combat.Enemy.Name} deals {damage} damage to you!");
            Console.WriteLine($"You have {combat.Player.Stats.Health} health remaining.");
            
            if (combat.Player.Stats.Health <= 0)
            {
                return new CombatEndState("You have been defeated! Game Over.");
            }
        }
        else
        {
            Console.WriteLine($"{combat.Enemy.Name}'s attack misses!");
        }
        
        return new PlayerTurnState();
    }

    public override string GetStateDescription() => "Enemy's turn to act";
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
    public Creature Enemy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool PlayerDefending { get; set; } = false;
    public CombatState CurrentState { get; set; }

    public CombatContext(Player player, Creature enemy)
    {
        Player = player;
        Enemy = enemy;
        CurrentState = new PlayerTurnState(); // Combat starts with player turn
    }

    public void RunCombat()
    {
        Console.WriteLine($"\n🗡️ Combat begins with {Enemy.Name}! 🗡️");
        Console.WriteLine($"Your Health: {Player.Stats.Health} | {Enemy.Name} Health: {Enemy.Stats.Health}");
        
        while (IsActive)
        {
            CurrentState = CurrentState.HandleTurn(this);
        }
    }
}
