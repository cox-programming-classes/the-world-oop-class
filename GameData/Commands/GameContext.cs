using The_World.GameData.Areas;
using The_World.GameData.Items;
using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class GameContext
{
    public Player Player { get; set; }
    public Area CurrentArea { get; set; }
    public bool KeepPlaying { get; set; } = true;
    
    // NEW: A Library of Items that can be spawned into the world at runtime
    public Dictionary<string, Func<Item>> ItemLibrary { get; private set; } = new();

    public GameContext(Player player, Area currentArea)
    {
        Player = player;
        CurrentArea = currentArea;
        PopulateItemLibrary(); // Initialize the library when GameContext is created
    }
    
    // NEW: Method to populate the item library with all archetypes
    private void PopulateItemLibrary()
    {
        // Weapons
        ItemLibrary["rusty_sword"] = () => ItemFactory.BuildRustySwordArchetype();
        ItemLibrary["iron_sword"] = () => ItemFactory.BuildIronSwordArchetype();
        ItemLibrary["steel_sword"] = () => ItemFactory.BuildSteelSwordArchetype();
        ItemLibrary["battle_axe"] = () => ItemFactory.BuildBattleAxeArchetype();
        ItemLibrary["dagger"] = () => ItemFactory.BuildDaggerArchetype();
        
        // Armor
        ItemLibrary["leather_helmet"] = () => ItemFactory.BuildLeatherHelmetArchetype();
        ItemLibrary["iron_chestplate"] = () => ItemFactory.BuildIronChestplateArchetype();
        ItemLibrary["wooden_shield"] = () => ItemFactory.BuildWoodenShieldArchetype();
        
        // Consumables
        ItemLibrary["healing_herb"] = () => ItemFactory.BuildHealingHerbArchetype();
        ItemLibrary["health_potion"] = () => ItemFactory.BuildHealthPotionArchetype();
        ItemLibrary["mana_potion"] = () => ItemFactory.BuildManaPotionArchetype();
        ItemLibrary["bread"] = () => ItemFactory.BuildBreadArchetype();
        
        // Tools
        ItemLibrary["lockpick"] = () => ItemFactory.BuildLockpickArchetype();
        ItemLibrary["rope"] = () => ItemFactory.BuildRopeArchetype();
        ItemLibrary["torch"] = () => ItemFactory.BuildTorchArchetype();
        
        // Decorations
        ItemLibrary["ancient_book"] = () => ItemFactory.BuildAncientBookArchetype();
        ItemLibrary["gold_coin"] = () => ItemFactory.BuildGoldCoinArchetype();
        ItemLibrary["mysterious_orb"] = () => ItemFactory.BuildMysteriousOrbArchetype();
        // Add these lines to your PopulateItemLibrary() method in GameContext:

// More Bones
        ItemLibrary["random_bone"] = () => ItemFactory.BuildRandomBoneArchetype();

// More armor? idk does a witch hat help u  
        ItemLibrary["witch_hat"] = () => ItemFactory.BuildWitchHatArchetype();

// yum
        ItemLibrary["raspberry_bar"] = () => ItemFactory.BuildRaspberryBarArchetype();

    }
    
    // NEW: Method to spawn an item from the library
    public Item? SpawnItem(string itemKey)
    {
        if (ItemLibrary.TryGetValue(itemKey, out var factory))
        {
            return factory();
        }
        return null;
    }
    
    // NEW: Method to get all available item keys
    public List<string> GetAvailableItems()
    {
        return ItemLibrary.Keys.ToList();
    }
}
