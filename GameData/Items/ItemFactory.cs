using The_World.GameData.Effects;

namespace The_World.GameData.Items;

public static class ItemFactory
{
    // Existing methods...
    public static Weapon BuildRustySwordArchetype(
        string name = "rusty_sword",
        string description = "Give your enemies tetanus with this one simple trick!",
        double weight = 3.5,
        int attackpower = 3,
        int durability = 10)
    {
        return Item.CreateWeapon(name, description, weight, attackpower, durability);
    }

    public static Weapon BuildIronSwordArchetype(
        string name = "iron_sword",
        string description = "Pretty sharp. You could stab something with that",
        double weight = 3.0,
        int attackpower = 5,
        int durability = 10)
    {
        return Item.CreateWeapon(name, description, weight, attackpower, durability);
    }

    // NEW WEAPON ARCHETYPES
    public static Weapon BuildSteelSwordArchetype(
        string name = "steel_sword",
        string description = "A well-crafted blade that gleams in the light.",
        double weight = 2.8,
        int attackpower = 8,
        int durability = 75)
    {
        return Item.CreateWeapon(name, description, weight, attackpower, durability);
    }

    public static Weapon BuildBattleAxeArchetype(
        string name = "battle_axe",
        string description = "A heavy axe designed for crushing enemies.",
        double weight = 5.2,
        int attackpower = 12,
        int durability = 60)
    {
        return Item.CreateWeapon(name, description, weight, attackpower, durability);
    }

    public static Weapon BuildDaggerArchetype(
        string name = "dagger",
        string description = "A quick, lightweight blade perfect for stealth.",
        double weight = 0.8,
        int attackpower = 4,
        int durability = 80)
    {
        return Item.CreateWeapon(name, description, weight, attackpower, durability);
    }

    // ARMOR ARCHETYPES
    public static Armor BuildLeatherHelmetArchetype(
        string name = "leather_helmet",
        string description = "Basic protection for your head.",
        double weight = 1.5,
        int defenseValue = 2)
    {
        return Item.CreateArmor(name, description, weight, defenseValue, ArmorSlot.HeadSlot);
    }

    public static Armor BuildIronChestplateArchetype(
        string name = "iron_chestplate",
        string description = "Solid iron protection for your torso.",
        double weight = 8.0,
        int defenseValue = 8)
    {
        return Item.CreateArmor(name, description, weight, defenseValue, ArmorSlot.ChestSlot);
    }

    public static Armor BuildWoodenShieldArchetype(
        string name = "wooden_shield",
        string description = "A sturdy wooden shield reinforced with iron bands.",
        double weight = 3.0,
        int defenseValue = 5)
    {
        return Item.CreateArmor(name, description, weight, defenseValue, ArmorSlot.ShieldSlot);
    }

    // MORE CONSUMABLES
    public static Consumable BuildHealingHerbArchetype(
        string name = "Healing herb",
        string description = "A small herb known for its medicinal properties",
        double weight = 0.2,
        int healAmount = 5)
    {
        return Item.CreateConsumable(name, description, weight, new HealingEffect(healAmount));
    }
    public static Consumable BuildHealthPotionArchetype(
        string name = "health_potion",
        string description = "A red potion that restores vitality.",
        double weight = 0.5,
        int healAmount = 15)
    {
        return Item.CreateConsumable(name, description, weight, new HealingEffect(healAmount));
    }
    public static Consumable BuildLargeHealthPotionArchetype(
        string name = "large_health_potion",
        string description = "A large red potion that restores vitality.",
        double weight = 0.5,
        int healAmount = 30)
    {
        return Item.CreateConsumable(name, description, weight, new HealingEffect(healAmount));
    }

    public static Consumable BuildManaPotionArchetype(
        string name = "mana_potion",
        string description = "A blue potion that restores magical energy.",
        double weight = 0.5,
        int manaAmount = 10)
    {
        return Item.CreateConsumable(name, description, weight, new ManaRestoreEffect(manaAmount));
    }
    public static Consumable BuildLargeManaPotionArchetype(
        string name = "mana_potion",
        string description = "A Large blue potion that restores magical energy.",
        double weight = 0.5,
        int manaAmount = 20)
    {
        return Item.CreateConsumable(name, description, weight, new ManaRestoreEffect(manaAmount));
    }

    public static Consumable BuildBreadArchetype(
        string name = "bread",
        string description = "A simple loaf that fills your belly.",
        double weight = 0.3,
        int healAmount = 2)
    {
        return Item.CreateConsumable(name, description, weight, new HealingEffect(healAmount));
    }

    // TOOL ARCHETYPES
    public static Tools BuildLockpickArchetype(
        string name = "lockpick",
        string description = "A thin metal tool for opening locked doors.",
        double weight = 0.1,
        int durability = 50)
    {
        return Item.CreateTool(name, description, weight, durability);
    }

    public static Tools BuildRopeArchetype(
        string name = "rope",
        string description = "50 feet of sturdy hemp rope.",
        double weight = 2.0,
        int durability = 90)
    {
        return Item.CreateTool(name, description, weight, durability);
    }

    public static Tools BuildTorchArchetype(
        string name = "torch",
        string description = "A wooden torch wrapped in oil-soaked cloth.",
        double weight = 1.0,
        int durability = 30)
    {
        return Item.CreateTool(name, description, weight, durability);
    }

    // DECORATION ARCHETYPES
    public static Decoration BuildAncientBookArchetype(
        string name = "ancient_book",
        string description = "A weathered tome filled with mysterious symbols.")
    {
        return Item.CreateDecoration(name, description);
    }

    public static Decoration BuildGoldCoinArchetype(
        string name = "gold_coin",
        string description = "A shiny coin bearing the mark of an ancient kingdom.")
    {
        return Item.CreateDecoration(name, description);
    }

    public static Decoration BuildMysteriousOrbArchetype(
        string name = "mysterious_orb",
        string description = "A glass sphere that seems to swirl with inner light.")
    {
        return Item.CreateDecoration(name, description);
    }
}
