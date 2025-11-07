using The_World.GameData.Areas;
using The_World.GameData.Creatures;
using The_World.GameData.Items;

namespace The_World.GameData.GameMechanics;

// Import factory methods for easier access!
using static CreatureFactory;

// TODO: Expand this to build the entire world with multiple areas, creatures, and items.
// TODO: Create an ItemFactory for reusable item archetypes.

public static class WorldBuilder
{
    /// <summary>
    /// Initialize the WHOLE WORLD here.
    /// </summary>
    /// <returns></returns>
    public static Area BuildWorld()
    {
        var startingArea = AreaBuilder
            .FromName("Dark Forest")
            .WithDescription("A gloomy forest filled with towering trees and eerie sounds.")
            .WithCreature(
                "goblin_1",
                BuildGoblinArchetype())
            .WithCreature(
                "goblin_2",
                BuildGoblinArchetype("Goblin Scout", "A nimble goblin with keen eyes, always on the lookout for intruders."))
            .WithCreature(
                "boss_goblin",
                BuildGoblinArchetype("Goblin Warrior", "A fierce goblin clad in makeshift armor, wielding a crude weapon.", 2))
            .WithItem("rusty_sword", 
                ItemFactory.BuildRustySwordArchetype())
            .Build();
        
        var clearingArea = AreaBuilder
            .FromName("Sunny Clearing")
            .WithDescription("A bright clearing bathed in sunlight, with soft grass and colorful flowers.")
            .WithCreature("orc",
                BuildOrcArchetype())
            .WithItem("healing_herb", 
                ItemFactory.BuildHealingHerbArchetype())
            .Build();
        
        // Connect areas
        startingArea = AreaBuilder.FromArea(startingArea)
            .WithConnectedArea("sunny_clearing", clearingArea)
            .Build();
        
        /*
         * TODO: Expand the world by creating more areas and connecting them.
         */
        
        return startingArea;
    }
    
    
}