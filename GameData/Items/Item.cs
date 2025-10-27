namespace The_World.GameData.Items;

/// <summary>
/// A Generic Item in the game world.
/// This could be anything from a weapon, a piece of armor,
/// a potion, or any other object that the player can interact with.
///
/// TODO: Expand this by creating derived item types
/// such as Weapon, Armor, Consumable, etc.
///
/// TODO: Research - Polymorphism for Item Types.  In Particular, look for the key word "Discriminated Unions".
/// </summary>
/// <param name="name"></param>
/// <param name="description"></param>
/// <param name="weight"></param>
///

public abstract record Item(string name, string description, double weight)
{
    public abstract string Look();
    public static Item CreateNewItem(
        string name,
        string description,
        double weight)
    {
        return new GenericItem(
            name?.Trim() switch { null or "" => "Unknown Item", _ => name.Trim() },
            description?.Trim() switch { null or "" => "No description available.", _ => description.Trim() },
            weight switch { < 0 => 0, _ => weight });
    }
}
public record GenericItem(string Name, string Description, double Weight) : Item(Name, Description, Weight)
{
    public override string Look() => $"{Name} (Weight: {Weight} lbs){Environment.NewLine}{Description}";
}

public record Weapon(
    string Name,
    string Description,
    double Weight,
    int AttackPower,
    int Durability) : Item(Name, Description, Weight)
{
    public override string Look() =>
        $"{Name} (Attack: {AttackPower}, Durability: {Durability}/{Durability})\n{Description}";
}


public record Consumable(
    string Name,
    string Description, 
    double Weight,
    int Uses,
    string Effect) : Item(Name, Description, Weight)
{
    public override string Look() => 
        $"{Name} (Uses: {Uses}, Effect: {Effect})\n{Description}";
}

public enum ArmorSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Hands,
    Shield,
    Ring,
    Necklace
}

public record Armor(
    string Name,
    string Description,
    double Weight,
    int DefenseValue,
    ArmorSlot Slot,
    int Durability = 100) : Item(Name, Description, Weight)
{
    public override string Look() =>
        $"{Name} (Defense: +{DefenseValue}, Slot: {Slot}, Durability: {Durability}%)\n{Description}";
}


/*public string Name { get; private set; } = name?.Trim() switch
    {
        null or "" => "Unknown Item",
        _ => name.Trim()
    };

    public string Description { get; private set; } = description?.Trim() switch
    {
        null or "" => "No description available.",
        _ => description.Trim()
    };

    public double Weight { get; private set; } = weight switch
    {
        < 0 => 0,
        _ => weight
    };

    public static Item CreateNewItem(
        string name,
        string description,
        double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Item name cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentNullException(nameof(description), "Item description cannot be null or empty.");
        }

        return new Item(
            name.Trim(),
            description.Trim(),
            weight);
    }*/