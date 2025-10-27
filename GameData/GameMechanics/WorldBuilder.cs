using The_World.GameData.Areas;
using The_World.GameData.Creatures;
using The_World.GameData.Items;

namespace The_World.GameData.GameMechanics;

// Import factory methods for easier access!
using static CreatureFactory;

// TODO: Expand this to build the entire world with multiple areas, creatures, and items.
// TODO: Create an ItemFactory for reusable item archetypes.
// TODO: Research - WorldBuilder is currently a "static" class, and not a proper implementation of the Builder Design Pattern.  
// With the help of the AI Chatbot, figure out how to implement the WorldBuilder properly and explain the process

public static class WorldBuilder
{
    /// <summary>
    /// Initialize the WHOLE WORLD here.
    /// </summary>
    /// <returns></returns>
    public static Area BuildWorld()
    {
        // Create all areas first (without connections)
        var forestArea = AreaBuilder
            .FromName("Dark Forest")
            .WithDescription(
                "A gloomy forest filled with towering trees and eerie sounds. You notice a hole in the ground leading downward.")
            .WithCreature("goblin_1", BuildGoblinArchetype())
            .WithItem("rusty_sword",
                new Item("Rusty Sword", "An old and worn sword, still sharp enough to be dangerous.", 3.5))
            .Build();

        var caveArea = AreaBuilder
            .FromName("Mountain Cave")
            .WithDescription(
                "A dark, damp cave with three passages: a hole above leading up, an exit to the east, and a steep climb upward to the west.")
            // TODO: Instead of reusing the Goblin archetype, create a Bat archetype~
            .WithCreature("cave_bat", BuildGoblinArchetype("Giant Bat", "A large bat with leathery wings.", 2))
            .WithItem("glowing_crystal",
                new Item("Glowing Crystal", "A mysterious crystal that emits a soft blue light.", 1.0))
            .Build();

        var fieldArea = AreaBuilder
            .FromName("Open Field")
            .WithDescription(
                "A vast field of tall grass swaying in the breeze. To the west, you see the entrance to a cave.")
            .WithItem("healing_herb",
                new Item("Healing Herb", "A small herb known for its medicinal properties.", 0.2))
            .Build();

        var dungeonArea = AreaBuilder
            .FromName("Ancient Dungeon")
            .WithDescription("A stone dungeon with moss-covered walls. You can climb down to return to the cave below.")
            .WithCreature("dungeon_guard",
                BuildGoblinArchetype("Skeleton Warrior", "An ancient skeleton in rusted armor.", 3))
            .Build();

        // Now connect them all - this creates the bidirectional links
        // TODO: Notice we rebuild each area to add connections
        forestArea = AreaBuilder.FromArea(forestArea)
            .WithConnectedArea("down", caveArea) // hole leads down to cave
            .Build();

        caveArea = AreaBuilder.FromArea(caveArea)
            .WithConnectedArea("up", forestArea) // hole leads up to forest  
            .WithConnectedArea("east", fieldArea) // exit leads to field
            .WithConnectedArea("climb", dungeonArea) // climb up to dungeon
            .Build();

        fieldArea = AreaBuilder.FromArea(fieldArea)
            .WithConnectedArea("west", caveArea) // back to cave
            .Build();

        dungeonArea = AreaBuilder.FromArea(dungeonArea)
            .WithConnectedArea("down", caveArea) // climb down to cave
            .Build();

        return forestArea; // Start in the forest
    }

}
