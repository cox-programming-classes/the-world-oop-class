namespace The_World.GameData.NPCs;

/// <summary>
/// Factory class for creating NPC archetypes
/// Demonstrates the Factory Pattern for object creation
/// </summary>
public static class NPCFactory
{
    /// <summary>
    /// Creates a basic merchant NPC
    /// </summary>
    public static Merchant CreateBasicMerchant(
        string name = "Merchant",
        string description = "A friendly trader with various goods for sale.")
    {
        var itemPrices = new Dictionary<string, int>
        {
            ["health_potion"] = 25,
            ["bread"] = 5,
            ["rusty_sword"] = 50
        };
        
        return new Merchant(
            name,
            description,
            "Welcome, traveler! Care to see my wares?",
            itemPrices
        );
    }
    
    /// <summary>
    /// Creates a town guard NPC
    /// </summary>
    public static Guard CreateTownGuard(
        string name = "Town Guard",
        string description = "A sturdy guard in leather armor, watching the area carefully.")
    {
        return new Guard(
            name,
            description,
            "Halt! State your business.",
            "Be careful out there - I've heard reports of goblins in the forest."
        );
    }
}