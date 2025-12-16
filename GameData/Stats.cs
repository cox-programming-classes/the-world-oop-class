namespace The_World.GameData;

/// <summary>
/// Represents the health and mana statistics of an entity.
/// TODO: Expand this to include more stats as needed.
/// TODO: Research - Structs vs Records for lightweight data containers.
/// </summary>
public record StatChart
{
    private int _health;

    public int Health
    {
        get => _health;
        set => _health = value switch
            // initial value setter
            {
                < 0 => 0,
                > 100 => 100,
                _ => value
            };
    }

    public void RestoreHealth(int amount)
    {
        if (amount > 0)
        {
            Health += amount;
        }
    }

    private int _mana;

    public int Mana
    {
        get => _mana;
        set => _mana = value switch
        {
            < 0 => 0,
            > 50 => 50,
            _ => value
        };

    }
    public void RestoreMana(int amount)
    {
        if (amount > 0)
        {
            Mana += amount;
        }
    }
}
// TODO: Use Dice rolls to determine starting stats based on Player Class or Creature Type?
    // TODO: add Properties which are Dice for things like "Attack Power", "Defense", etc.