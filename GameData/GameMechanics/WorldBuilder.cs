using The_World.GameData.Areas;
using The_World.GameData.Creatures;
using The_World.GameData.Items;

namespace The_World.GameData.GameMechanics;

using static CreatureFactory;
using static ItemFactory;

/// <summary>
/// Proper implementation of the Builder Design Pattern for creating game worlds.
/// This allows for flexible, step-by-step world construction.
/// </summary>
public class WorldBuilder
{
    private readonly Dictionary<string, Area> _areas = new();
    private string? _startingAreaKey;

    #region Fluent Interface Methods

    /// <summary>
    /// Add a new area to the world being built.
    /// </summary>
    public WorldBuilder WithArea(string key, Area area)
    {
        _areas[key?.Trim() switch //overwrites does not throw exception if u have key twice
        {
            null or "" => throw new ArgumentException("Area key cannot be null or empty.", nameof(key)),
            _=>key
        }]= area ?? throw new ArgumentNullException(nameof(area));
        return this;
        
        /* if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Area key cannot be null or empty.", nameof(key));
        
        if (_areas.ContainsKey(key))
            throw new ArgumentException($"Area with key '{key}' already exists.", nameof(key));

        _areas[key] = area ?? throw new ArgumentNullException(nameof(area));
        return this;
        */
    }

    /// <summary>
    /// Connect two areas bidirectionally.
    /// </summary>
    public WorldBuilder WithConnection(string fromAreaKey, string direction, string toAreaKey, string reverseDirection)
    {
        if (!_areas.ContainsKey(fromAreaKey))
            throw new ArgumentException($"Area '{fromAreaKey}' not found.");
        if (!_areas.ContainsKey(toAreaKey))
            throw new ArgumentException($"Area '{toAreaKey}' not found.");

        var fromArea = _areas[fromAreaKey];
        var toArea = _areas[toAreaKey];

        // Create bidirectional connection by rebuilding both areas
        _areas[fromAreaKey] = AreaBuilder.FromArea(fromArea)
            .WithConnectedArea(direction, toArea)
            .Build();

        _areas[toAreaKey] = AreaBuilder.FromArea(toArea)
            .WithConnectedArea(reverseDirection, fromArea)
            .Build();

        return this;
    }

    /// <summary>
    /// Set which area the player starts in.
    /// </summary>
    public WorldBuilder WithStartingArea(string areaKey)
    {
        if (!_areas.ContainsKey(areaKey))
            throw new ArgumentException($"Starting area '{areaKey}' not found.");
        
        _startingAreaKey = areaKey;
        return this;
    }

    #endregion

    #region Build Method

    /// <summary>
    /// Complete the world construction and return the starting area.
    /// </summary>
    public Area Build()
    {
        if (_areas.Count == 0)
            throw new InvalidOperationException("Cannot build world with no areas.");
        
        // Use specified starting area or default to first added area
        var startingKey = _startingAreaKey ?? _areas.Keys.First();
        return _areas[startingKey];
    }

    #endregion

    #region Static Factory Methods

    /// <summary>
    /// Create a new WorldBuilder instance.
    /// </summary>
    public static WorldBuilder Create() => new();

    /// <summary>
    /// Build the default game world.
    /// </summary>
    public static Area BuildWorld()
    {
        // Create individual areas (same as your original code)
        var forestArea = AreaBuilder
            .FromName("Dark Forest")
            .WithDescription("A gloomy forest filled with towering trees and eerie sounds. You notice a hole in the ground leading downward.")
            .WithCreature("goblin_1", BuildGoblinArchetype())
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
                "oh no........."))
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
            .WithConnection("cave", "west", "dungeon", "down")
            .WithStartingArea("forest")
            .Build();
    }

    #endregion
}
