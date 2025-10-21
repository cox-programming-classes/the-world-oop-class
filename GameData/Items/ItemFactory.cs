using The_World.GameData.Items;
namespace The_World.GameData.GameMechanics;

public static class ItemFactory
{
    public static Item BuildRustySwordArchetype(
        string name = "rusty_sword",
        string description = "Give your enemies tetanus with this one simple trick!",
        double weight = 3.5)
    {
        return Item.CreateNewItem(
            name,
            description,
            weight
        );
    }

    public static Item BuildIronSwordArchetype(
        string name = "Iron Sword",
        string description = "Pretty sharp. You could stab something with that",
        double weight = 3.0)
    {
        return Item.CreateNewItem(
            name,
            description,
            weight
        );
    }

    public static Item BuildHealingHerbArchetype(
        string name = "Healing herb",
        string description = "A small herb known for its medicinal properties",
        double weight = 0.2
    )
    {
        return Item.CreateNewItem(
            name,
            description,
            weight);
    }
}