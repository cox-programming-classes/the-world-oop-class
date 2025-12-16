using The_World.GameData.Areas;
using The_World.GameData.Creatures;
using The_World.GameData.NPCs;
using The_World.GameData.Items;
using static The_World.GameData.Creatures.CreatureFactory;
using static The_World.GameData.Items.ItemFactory;

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
            .WithItem("health_potion", BuildHealthPotionArchetype())
            .WithItem("rusty_sword", BuildRustySwordArchetype())
            .WithNPC("merchant", NPCFactory.CreateBasicMerchant("Forest Trader", "A traveling merchant who braves the dangerous forest."))
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
        
        return new GameContext(defaultPlayer, startingArea);
    }
}