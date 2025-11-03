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

    /// <summary>
    /// Creates a new Weapon item with a name, description, weight, attackPower, and durability
    /// </summary>
    /// <param name="name"> The weapon's name </param>
    /// <param name="description"> Description of the weapon's appearance and/or function </param>
    /// <param name="weight"> Weight in pounds </param>
    /// <param name="attackPower"> How much damage the weapon deals </param>
    /// <param name="durability"> Current durability </param>
    /// <returns></returns>
    public static Weapon CreateWeapon(string name, string description, double weight, int attackPower,
        int durability = 100)
        => new(
            SanitizeName(name),
            SanitizeDescription(description),
            SanitizeWeight(weight),
            SanitizeAttackPower(attackPower),
            SanitizeDurability(durability));

    /// <summary>
    /// Creates a new Consumable item with a name, description, weight, and effect
    /// </summary>
    /// <param name="name"> The Consumable's name </param>
    /// <param name="description"> Description of the Consumable's appearance and/or function </param>
    /// <param name="weight"> Weight in pounds </param>
    /// <param name="effect"> What the Consumable does to the Player </param>
    /// <returns></returns>
    public static Consumable CreateConsumable(string name, string description, double weight, string effect)
        => new(
            SanitizeName(name),
            SanitizeDescription(description),
            SanitizeWeight(weight),
            SanitizeEffect(effect));

    /// <summary>
    /// Creates a new Weapon item with a name, description, weight, defenseValue, slot and durability
    /// </summary>
    /// <param name="name"> The Armor's name </param>
    /// <param name="description"> Description of the Armor's appearance and/or function </param>
    /// <param name="weight"> Weight in pounds </param>
    /// <param name="defenseValue"> How much damage the armor deflects </param>
    /// <param name="slot"> Where the armor is equipped </param>
    /// <param name="durability"> Current durability </param>
    /// <returns></returns>
    public static Armor CreateArmor(string name, string description, double weight, int defenseValue, ArmorSlot slot,
        int durability = 100)
        => new(
            SanitizeName(name),
            SanitizeDescription(description),
            SanitizeWeight(weight),
            SanitizeDefense(defenseValue),
            slot,
            SanitizeDurability(durability));

    /// <summary>
    /// Creates a new Tool item with a name, description, weight, defenseValue, slot and durability
    /// </summary>
    /// <param name="name"> The tool's name </param>
    /// <param name="description"> Description of the tool's appearance and/or function</param>
    /// <param name="weight"> Weight in pounds</param>
    /// <param name="durability"> Current durability</param>
    /// <returns></returns>
    public static Tools CreateTool(string name, string description, double weight, int durability)
        => new(
            SanitizeName(name),
            SanitizeDescription(description),
            SanitizeWeight(weight),
            SanitizeDurability(durability));

    private static string SanitizeName(string? name) =>
        name?.Trim() switch { null or "" => "Unknown Item", _ => name.Trim() };

    private static string SanitizeDescription(string? description) =>
        description?.Trim() switch { null or "" => "No description available.", _ => description.Trim() };

    private static double SanitizeWeight(double weight) =>
        weight switch { < 0 => 0, _ => weight };

    private static int SanitizeAttackPower(int attackPower) =>
        attackPower switch { < 0 => 1, _ => attackPower };

    private static int SanitizeDurability(int durability) =>
        durability switch { < 0 => 0, > 100 => 100, _ => durability };

    private static int SanitizeDefense(int defense) =>
        defense switch { < 0 => 0, _ => defense };

    private static string SanitizeEffect(string? effect) =>
        effect?.Trim() switch { null or "" => "Unknown Effect", _ => effect.Trim() };
}


/// <summary>
/// Represents a weapon item that can be used in combat
/// </summary>
/// <param name="Name">The weapon's display name, and how it should be referenced in commands</param>
/// <param name="Description">Detailed description of the weapon</param>
/// <param name="Weight">Weight in pounds</param>
/// <param name="AttackPower">Base damage dealt in combat</param>
/// <param name="Durability">How close the weapon is to breaking</param>
public record Weapon(
    string Name,
    string Description,
    double Weight,
    int AttackPower,
    int Durability) : Item(Name, Description, Weight)
{
    public override string Look() =>
        $"{Name} (Attack: {AttackPower}, Durability: {Durability}/100)\n{Description}";
}

/// <summary>
/// Represents a consumable item that can be used by the Player
/// </summary>
/// <param name="Name">The consumable's display name, and how it should be referenced in commands</param>
/// <param name="Description">Detailed description of the consumable</param>
/// <param name="Weight">Weight in pounds</param>
/// <param name="Effect">What the Consumable does to the Player</param>
public record Consumable(
    string Name,
    string Description, 
    double Weight,
    string Effect) : Item(Name, Description, Weight)
{
    public override string Look() => 
        $"{Name} (Effect: {Effect})\n{Description}";
}

/// <summary>
/// Thing that I need to do later- shows where the armor is equipped.
/// maybe I should make it so armor doesn't need to be equipped also. ah well
/// </summary>
public enum ArmorSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Hands,
    Shield,
}

/// <summary>
/// Represents an armor item, which can be equipped to reduce damage done to the Player
/// </summary>
/// <param name="Name">The armor's display name, and how it should be referenced in commands</param>
/// <param name="Description">Detailed description of the consumable</param>
/// <param name="Weight">Weight in pounds</param>
/// <param name="DefenseValue">How much damage the armor reflects</param>
/// <param name="Slot">Where the armor is equipped</param>
/// <param name="Durability">How close the armor is to breaking</param>
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

/// <summary>
/// Represents a tool item, which can be used by the Player for. um. well. something? later
/// </summary>
/// <param name="Name">The tool's display name, and how it should be referenced in commands</param>
/// <param name="Description">Detailed description of the tool</param>
/// <param name="Weight">Weight in pounds</param>
/// <param name="Durability">How close the tool is to breaking</param>
public record Tools(
    string Name,
    string Description,
    double Weight,
    int Durability) : Item(Name, Description, Weight)
{
    public override string Look() =>
        $"{Name} \n{Description}";
}