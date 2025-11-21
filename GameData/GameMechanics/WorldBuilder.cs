using The_World.GameData.Areas;
using The_World.GameData.Creatures;
using The_World.GameData.Commands; // Add this for GameContext
using The_World.GameData; // Add this for Player

namespace The_World.GameData.GameMechanics;

public class WorldBuilder
{
    /// <summary>
    /// Initialize the WHOLE WORLD here.
    /// </summary>
    /// <returns></returns>
    public static GameContext BuildWorld(string playerName)
    {
        // Create a default player
        var defaultPlayer = Player.CreateNewPlayer(playerName, "Adventurer");
        
        var startingArea = AreaBuilder
            .FromName("Dark Forest")
            .WithDescription("A gloomy forest filled with towering trees and eerie sounds. You notice a hole in the ground leading downward.")
            .WithCreature("goblin_1", BuildGoblinArchetype())
            .WithCreature(
                "goblin_2",
                BuildGoblinArchetype("Goblin Scout", "A nimble goblin with keen eyes, always on the lookout for intruders."))
            .WithCreature(
                "boss_goblin",
                BuildGoblinArchetype("Goblin Warrior", "A fierce goblin clad in makeshift armor, wielding a crude weapon.", 2))
            .WithItem("health_potion", BuildHealthPotionArchetype())//testing my things you can remove this if you want - Anne
            .WithItem("rusty_sword", BuildRustySwordArchetype())
            .Build();

        var caveArea = AreaBuilder
            .FromName("Mountain Cave")
            .WithDescription("A dark, damp cave with three passages: a hole above leading up, an exit to the east, and a steep climb upward to the west.")
            .WithCreature("cave_bat", BuildGoblinArchetype("Giant Bat", "A large bat with leathery wings.", 2))
            .WithItem("glowing_crystal", Item.CreateDecoration("Glowing Crystal", "A mysterious crystal that emits a soft blue light."))
            .Build();

        var fieldArea = AreaBuilder
            .FromName("Open Field")
            .WithDescription("A vast field of tall grass swaying in the breeze. To the west, you see the entrance to a cave.")
            .WithItem("healing_herb", 
                BuildHealingHerbArchetype(
                "Healing Herb", 
                "A small herb known for its medicinal properties.", 
                0.2,
                2))
            .Build();

        var dungeonArea = AreaBuilder
            .FromName("Ancient Dungeon")
            .WithDescription("A stone dungeon with moss-covered walls. You can climb down to return to the cave below.")
            .WithCreature("dungeon_guard", BuildGoblinArchetype("Skeleton Warrior", "An ancient skeleton in rusted armor.", 3))
            .Build();

        // Use the proper Builder pattern to construct the world
        return WorldBuilder.Create()
            .WithArea("forest", forestArea) // human-readable words are not good unique identifiers (i.e. key names); use e globally unique identifier (GUID.something) 
            .WithArea("cave", caveArea)
            .WithArea("field", fieldArea)
            .WithArea("dungeon", dungeonArea)
            .WithConnection("forest", "down", "cave", "up")
            .WithConnection("cave", "east", "field", "west")
            .WithConnection("cave", "climb", "dungeon", "down")
            .WithStartingArea("forest")
            .Build();
        
        return new GameContext(defaultPlayer, startingArea);
        
    }
    
    /// <summary>
    /// Helper method to build a Goblin creature archetype.
    /// You might use this in multiple areas.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    private static Creature BuildGoblinArchetype(
        string name = "Goblin", 
        string description = "A small, green humanoid creature with sharp teeth and a mischievous grin.",
        int level = 1)
    {
        return Creature.CreateNewCreature(
            name,
            description,
            new(10+(2*level), 0), // TODO: Stats should probably scale with level better
            level,
            5+(double.Exp(level/100.0)*10)); // TODO: XP scales with level this math sucks
    }
}