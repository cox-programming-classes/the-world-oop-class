namespace The_World.GameData.NPCs;

/// <summary>
/// Base class for all Non-Player Characters
/// Uses Encapsulation to manage NPC state and behavior
/// </summary>
public abstract record NPC(
    string Name,
    string Description,
    string Greeting)
{
    /// <summary>
    /// What the player sees when they look at this NPC
    /// </summary>
    public virtual string Look() => $"{Name}\n{Description}";
    
    /// <summary>
    /// What the NPC says when first approached
    /// Abstract method - each NPC type must implement their own interaction
    /// </summary>
    public abstract string Interact();
    
    /// <summary>
    /// Basic greeting message
    /// </summary>
    public virtual string Greet() => Greeting;
}

/// <summary>
/// A merchant NPC that can trade with players
/// </summary>
public record Merchant(
    string Name,
    string Description,
    string Greeting,
    Dictionary<string, int> ItemPrices) : NPC(Name, Description, Greeting)
{
    public override string Interact()
    {
        return $"{Greet()}\n\"Take a look at my wares! I have {ItemPrices.Count} items for sale.\"";
    }
}

/// <summary>
/// A guard NPC that provides information or warnings
/// </summary>
public record Guard(
    string Name,
    string Description,
    string Greeting,
    string WarningMessage) : NPC(Name, Description, Greeting)
{
    public override string Interact()
    {
        return $"{Greet()}\n\"{WarningMessage}\"";
    }
}