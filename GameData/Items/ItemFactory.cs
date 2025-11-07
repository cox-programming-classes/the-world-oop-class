using The_World.GameData.Items;
namespace The_World.GameData.GameMechanics;

public static class ItemFactory
{
    public static Weapon BuildRustySwordArchetype(
        string name = "rusty_sword",
        string description = "Give your enemies tetanus with this one simple trick!",
        double weight = 3.5,
        int attackpower = 3,
        int durability = 10)
    {
        return Item.CreateWeapon(
            name,
            description,
            weight,
            attackpower,
            durability);
    }

    public static Weapon BuildIronSwordArchetype(
        string name = "iron_sword",
        string description = "Pretty sharp. You could stab something with that",
        double weight = 3.0,
        int attackpower = 5,
        int durability = 10)
    {
        return Item.CreateWeapon(
            name,
            description,
            weight,
            attackpower,
            durability);
    }

    public static Consumable BuildHealingHerbArchetype(
        string name = "Healing herb",
        string description = "A small herb known for its medicinal properties",
        double weight = 0.2,
        string effect = "+2 hp"
    )
    {
        return Item.CreateConsumable(
            name,
            description,
            weight,
            effect);
    }
}